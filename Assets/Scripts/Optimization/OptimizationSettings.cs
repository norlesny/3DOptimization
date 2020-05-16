using System;
using System.Collections.Generic;
using MyCamera.Providers;
using UnityEngine;

namespace Optimization
{
	[Serializable]
	public struct OptimizationSettings
	{
		[SerializeField] private CameraProviderBehaviour camerasProvider;

		public IEnumerable<Camera> Cameras => camerasProvider;
		public int horizontalRaycastDensity;
		public int verticalRaycastDensity;
	}
}
