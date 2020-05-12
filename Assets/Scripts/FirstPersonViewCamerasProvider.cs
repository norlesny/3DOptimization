using UnityEngine;

namespace DefaultNamespace
{
	public class FirstPersonViewCamerasProvider : MonoBehaviour
	{
		private int numberOfCasts = 4;

		private void OnDrawGizmos()
		{
			var startPoint = transform.position;
			var endPoint = startPoint + transform.forward.normalized;
			float rotationAngle = 360f / numberOfCasts;
			float rotation = 0f;
			Vector3 transformUp = transform.up;
			for (int i = 0; i < numberOfCasts; i++)
			{
				endPoint = Quaternion.AngleAxis(rotationAngle, transformUp) * endPoint;
				Gizmos.DrawLine(startPoint, endPoint);				
			}
		}
	}
}
