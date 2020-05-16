using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
	public class FirstPersonViewCamerasProvider : MonoBehaviour, IEnumerable<Camera>
	{
		[SerializeField] private Camera targetCamera;
		[SerializeField] private int rollCasts = 4;
		[SerializeField] private int yawCasts = 4;
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

		public IEnumerator<Camera> GetEnumerator()
		{
			var originForward = targetCamera.transform.forward;
			var forward = targetCamera.transform.forward.normalized;
			var up = targetCamera.transform.up.normalized;
			float rollDelta = 180f / rollCasts;
			float yawDelta = 360f / yawCasts;
			for (int j = 0; j < rollCasts; j++)
			{
				for (int i = 0; i < yawCasts; i++)
				{
					targetCamera.transform.forward = Quaternion.AngleAxis(rollDelta * j, forward) *
						(Quaternion.AngleAxis(yawDelta * i, up) * forward);
					yield return targetCamera;
				}
			}
			targetCamera.transform.forward = originForward;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
