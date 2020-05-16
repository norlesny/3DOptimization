using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
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
