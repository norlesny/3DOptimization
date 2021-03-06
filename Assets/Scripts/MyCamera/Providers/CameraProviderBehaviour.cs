using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyCamera.Providers
{
	public abstract class CameraProviderBehaviour : MonoBehaviour, ICamerasProvider
	{
		public abstract IEnumerator<Camera> GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
