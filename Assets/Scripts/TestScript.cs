using Optimization;
using UnityEngine;

public class TestScript : MonoBehaviour
{
	[SerializeField] private OptimizationSettings settings;

	private void Start()
	{
		Optimization.Optimization.OptimizeView(settings);
	}
}
