using UnityEngine;

namespace Utils
{
	public class GeneratePointsOnSphere : MonoBehaviour
	{
		[SerializeField] private GameObject prefab;
		[SerializeField] private int numberOfPoints;
		[SerializeField] private float radius;

		[ContextMenu("Generate")]
		private void Generate()
		{
			var pointsContainer = new GameObject("Points").transform;
			pointsContainer.SetParent(transform, false);
			prefab.SetActive(true);
			for (int i = 0; i < numberOfPoints; i++)
			{
				GameObject instance = Instantiate(prefab, pointsContainer);
				instance.transform.position = transform.position + Random.onUnitSphere * radius;
			}
			prefab.SetActive(false);
		}
	}
}
