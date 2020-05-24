using MyCamera.Providers;
using Optimization;
using UnityEngine;

public class TestScript : MonoBehaviour
{
	[SerializeField] private OptimizationSettings settings;

	private void Start()
	{
		if (settings.camerasProvider == null)
		{
			settings.camerasProvider = FindObjectOfType<CameraProviderBehaviour>();
		}
		Debug.Log($"Launching with {settings.camerasProvider.name}", settings.camerasProvider);
		Optimization.Optimization.OptimizeView(settings);
	}
}
