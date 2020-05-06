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
		
		private static void ReplaceMeshes(IDictionary<MeshCollider, HashSet<int>> hitTriangles)
		{
			if (!hitTriangles.Values.Any())
			{
				Debug.Log("No triangles hit");
				return;
			}

			List<MeshCollider> allMeshColliders = GameObject.FindObjectsOfType<MeshCollider>().ToList();

			foreach (KeyValuePair<MeshCollider, HashSet<int>> kvp in hitTriangles)
			{
				MeshCollider meshCollider = kvp.Key;
//			Instantiate(meshCollider.gameObject).SetActive(false);
				Mesh oldMesh = meshCollider.sharedMesh;
				Mesh mesh = CopyMesh(oldMesh);
				int[] oldTriangles = mesh.triangles;
				var newTriangles = new List<int>();
				foreach (int triangleIndex in kvp.Value)
				{
					newTriangles.Add(oldTriangles[triangleIndex * 3 + 0]);
					newTriangles.Add(oldTriangles[triangleIndex * 3 + 1]);
					newTriangles.Add(oldTriangles[triangleIndex * 3 + 2]);
				}

				mesh.triangles = newTriangles.ToArray();
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
