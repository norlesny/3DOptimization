using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CountVerticesTriangles : MonoBehaviour {

	[ContextMenu("Count")]
	private void Count()
	{
		List<MeshFilter> meshFilters = Object.FindObjectsOfType<MeshFilter>().ToList();
		Debug.Log("Vertex count: "+meshFilters.Sum(filter => filter.mesh.vertexCount));
		Debug.Log(
			"Triangles count: "+meshFilters.Sum(filter => filter.mesh.triangles.Length / 3));
	}
}
