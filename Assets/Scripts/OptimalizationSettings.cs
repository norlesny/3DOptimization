using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
	[Serializable]
	public struct OptimalizationSettings
	{
		[SerializeField] private CameraProviderBehaviour camerasProvider;

		public IEnumerable<Camera> Cameras => camerasProvider;
		public int horizontalRaycastDensity;
		public int verticalRaycastDensity;
	}
}
