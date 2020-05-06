using System;
using UnityEngine;

namespace DefaultNamespace
{
	[Serializable]
	public struct OptimalizationSettings
	{
		[SerializeField] private InspectorCamerasProvider camerasProvider;

		public Camera[] Cameras => camerasProvider != null ? camerasProvider.Cameras : null;
		public int horizontalRaycastDensity;
		public int verticalRaycastDensity;
	}
}
