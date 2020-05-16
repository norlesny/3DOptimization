using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
	public class FirstPersonViewCamerasProvider : CameraProviderBehaviour
	{
		[SerializeField] private Camera targetCamera;
		[SerializeField] private int rollCasts = 3;

		[SerializeField]
		[Tooltip("Should be an odd value to prevent duplicate rotations (still one will be duplicated - forward)")]
		private int yawCasts = 3;

		[SerializeField] private bool drawRays;

		private void OnDrawGizmos()
		{
			if (drawRays)
			{
				DrawDirectionRaysGizmo();
			}
		}

		private void DrawDirectionRaysGizmo()
		{
			foreach (Camera c in this)
			{
				Transform cTransform = c.transform;
				Gizmos.DrawLine(cTransform.position, cTransform.position + cTransform.forward);
			}
		}

		public override IEnumerator<Camera> GetEnumerator()
		{
			var camera = targetCamera;
			var cameraTransform = camera.transform;
			var forward = cameraTransform.forward;
			var up = cameraTransform.up;
			float rollDelta = 180f / rollCasts;
			float yawDelta = 360f / yawCasts;
			for (int j = 0; j < rollCasts; j++)
			{
				for (int i = 0; i < yawCasts; i++)
				{
					cameraTransform.forward = Quaternion.AngleAxis(rollDelta * j, forward) *
						(Quaternion.AngleAxis(yawDelta * i, up) * forward);
					yield return camera;
				}
			}

			cameraTransform.forward = forward;
		}
	}
}
