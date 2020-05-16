using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
	public class InspectorCamerasProvider : MonoBehaviour, ICamerasProvider
	{
		[SerializeField] private Camera[] cameras;

		public IEnumerator<Camera> GetEnumerator()
		{
			return ((IEnumerable<Camera>) cameras).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
