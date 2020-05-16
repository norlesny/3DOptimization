using System.Collections.Generic;
using UnityEngine;

namespace MyCamera.Providers
{
	public class InspectorCamerasProvider : CameraProviderBehaviour
	{
		[SerializeField] private Camera[] cameras;

		public override IEnumerator<Camera> GetEnumerator()
		{
			return ((IEnumerable<Camera>) cameras).GetEnumerator();
		}
	}
}
