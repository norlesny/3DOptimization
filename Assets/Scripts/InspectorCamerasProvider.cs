using UnityEngine;

namespace DefaultNamespace
{
	public class InspectorCamerasProvider : MonoBehaviour, ICamerasProvider
	{
		[SerializeField] private Camera[] cameras;

		public Camera[] Cameras => cameras;
	}
}
