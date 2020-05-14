using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
	public static class Optimalization
	{
		public static void OptimizeView(OptimalizationSettings settings)
		{
			IDictionary<MeshCollider, HashSet<int>> hitTriangles = GetHitTriangles(settings);
			ReplaceMeshes(hitTriangles);
		}

		private static IDictionary<MeshCollider, HashSet<int>> GetHitTriangles(OptimalizationSettings settings)
		{
			var hitTriangles = new Dictionary<MeshCollider, HashSet<int>>();
			float horizontalStep = 1f / settings.horizontalRaycastDensity;
			float verticalStep = 1f / settings.verticalRaycastDensity;
			float x = 0f;
			while (x <= 1f)
			{
				float y = 0f;
				while (y <= 1f)
				{
					foreach (Camera camera in settings.Cameras)
					{
						RaycastThroughPoint(new Vector3(x, y), hitTriangles, camera);
					}

					y += verticalStep;
				}

				x += horizontalStep;
			}

			return hitTriangles;
		}

		private static void RaycastThroughPoint(Vector3 point, IDictionary<MeshCollider, HashSet<int>> hitTriangles,
			Camera camera)
		{
			Ray ray = camera.ViewportPointToRay(point);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				var meshCollider = hit.collider as MeshCollider;
				if (meshCollider == null || meshCollider.sharedMesh == null)
				{
					return;
				}

				HashSet<int> triangleIndexes;
				if (!hitTriangles.TryGetValue(meshCollider, out triangleIndexes))
				{
					triangleIndexes = new HashSet<int>();
					hitTriangles.Add(meshCollider, triangleIndexes);
				}

				triangleIndexes.Add(hit.triangleIndex);
			}
		}

		private static Mesh CreateNewMesh(MeshCollider collider, HashSet<int> usedTriangles)
		{
			var sharedMesh = collider.sharedMesh;
			var sharedMeshVertices = sharedMesh.vertices;
			int[] sharedMeshTriangles = sharedMesh.triangles;
			Vector3[] sharedMeshNormals = sharedMesh.normals;
			Vector2[] sharedMeshUv = sharedMesh.uv;
			Vector2[] sharedMeshUv2 = sharedMesh.uv2;
			Vector4[] sharedMeshTangents = sharedMesh.tangents;
			var sharedMeshVertexCount = sharedMesh.vertexCount;

			//Debug.Log("sharedMeshVertices:" + sharedMeshVertices.ToStringExt());
			//Debug.Log("sharedMeshTriangles:" + sharedMeshTriangles.ToStringExt());
			//Debug.Log("usedTriangles:" + usedTriangles.ToStringExt());

			var usedVertices = new List<int>();
			foreach (int triangleIndex in usedTriangles)
			{
				usedVertices.Add(sharedMeshTriangles[triangleIndex * 3 + 0]);
				usedVertices.Add(sharedMeshTriangles[triangleIndex * 3 + 1]);
				usedVertices.Add(sharedMeshTriangles[triangleIndex * 3 + 2]);
			}
			//Debug.Log("usedVertices:" + usedVertices.ToStringExt());

			var usedVerticesSorted = new HashSet<int>(usedVertices).ToList();
			usedVerticesSorted.Sort();
			//Debug.Log("usedVerticesSorted:" + usedVerticesSorted.ToStringExt());


			var newVertices = new List<Vector3>();
			var newNormals = new List<Vector3>();
			var newUv = new List<Vector2>();
			var newUv2 = new List<Vector2>();
			var newTangents = new List<Vector4>();
			var verticesMapping = new Dictionary<int, int>();
			for (int i = 0; i < usedVerticesSorted.Count; i++)
			{
				int index = usedVerticesSorted[i];
				newVertices.Add(sharedMeshVertices[index]);
				newNormals.Add(sharedMeshNormals[index]);
				newUv.Add(sharedMeshUv[index]);
				newUv2.Add(sharedMeshUv2[index]);
				newTangents.Add(sharedMeshTangents[index]);
				verticesMapping.Add(index, i);
			}
			//Debug.Log("newVertices:" + newVertices.ToStringExt());

			var newTriangles = new List<int>();
			foreach (int triangleIndex in usedTriangles)
			{
				newTriangles.Add(verticesMapping[sharedMeshTriangles[triangleIndex * 3 + 0]]);
				newTriangles.Add(verticesMapping[sharedMeshTriangles[triangleIndex * 3 + 1]]);
				newTriangles.Add(verticesMapping[sharedMeshTriangles[triangleIndex * 3 + 2]]);
			}


			Mesh mesh = CopyMesh(collider.sharedMesh);
			mesh.triangles = newTriangles.ToArray();
			mesh.vertices = newVertices.ToArray();
			mesh.normals = newNormals.ToArray();
			mesh.uv = newUv.ToArray();
			mesh.uv2 = newUv2.ToArray();
			mesh.tangents = newTangents.ToArray();
			return mesh;
		}

		private static void ReplaceMeshes(IDictionary<MeshCollider, HashSet<int>> hitTriangles)
		{
			if (!hitTriangles.Values.Any())
			{
				Debug.Log("No triangles hit");
				return;
			}

			List<MeshCollider> allMeshColliders = Object.FindObjectsOfType<MeshCollider>().ToList();
			Debug.Log($"Vertex count before: {allMeshColliders.Sum(collider => collider.sharedMesh.vertexCount)}");
			Debug.Log(
				$"Triangles count before: {allMeshColliders.Sum(collider => collider.sharedMesh.triangles.Length / 3)}");

			foreach (KeyValuePair<MeshCollider, HashSet<int>> kvp in hitTriangles)
			{
				MeshCollider meshCollider = kvp.Key;
				Mesh oldMesh = meshCollider.sharedMesh;
				Mesh mesh = CreateNewMesh(meshCollider, kvp.Value);
				meshCollider.sharedMesh = mesh;

				var meshFilter = meshCollider.transform.GetComponent<MeshFilter>();
				if (meshFilter != null && meshFilter.sharedMesh == oldMesh)
				{
					meshFilter.sharedMesh = mesh;
				}

				allMeshColliders.Remove(meshCollider);
			}

			foreach (MeshCollider meshCollider in allMeshColliders)
			{
				meshCollider.gameObject.SetActive(false);
			}


			CoroutinePool.RunDelayed(() =>
			{
				List<MeshCollider> meshColliders = Object.FindObjectsOfType<MeshCollider>().ToList();
				Debug.Log($"Vertex count after: {meshColliders.Sum(collider => collider.sharedMesh.vertexCount)}");
				Debug.Log(
					$"Triangles count after: {meshColliders.Sum(collider => collider.sharedMesh.triangles.Length / 3)}");
			});
		}

		private static Mesh CopyMesh(Mesh toCopy)
		{
			return new Mesh
			{
				vertices = toCopy.vertices,
				triangles = toCopy.triangles,
				uv = toCopy.uv,
				normals = toCopy.normals,
				colors = toCopy.colors,
				tangents = toCopy.tangents
			};
		}
	}
}
