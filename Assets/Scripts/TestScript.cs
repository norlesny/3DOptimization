using DefaultNamespace;
using UnityEngine;

public class TestScript : MonoBehaviour
{
	[SerializeField] private OptimalizationSettings settings;

	private void Start()
	{
		Optimalization.OptimizeView(settings);
	}
}
