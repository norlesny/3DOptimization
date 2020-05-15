using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
	public class FirstPersonViewCamerasProvider : MonoBehaviour, IEnumerable<Vector3>
	{
		[SerializeField] private Camera targetCamera;
		[SerializeField] private int rollCasts = 4;
		[SerializeField] private int yawCasts = 4;

		private IEnumerator<Vector3> enumerator;

		private void OnDrawGizmos()
		{
			var startPoint = transform.position;
			foreach (Vector3 d in this)
			{
				Gizmos.DrawLine(startPoint, startPoint + d);
			}
		}

		[ContextMenu("test")]
		private void test()
		{
			foreach (var v in this)
			{
				Debug.Log(v);
			}
		}

		public IEnumerator<Vector3> GetEnumerator()
		{
			var forward = transform.forward.normalized;
			var up = transform.up.normalized;
			float rollDelta = 180f / rollCasts;
			float yawDelta = 360f / yawCasts;
			for (int j = 0; j < rollCasts; j++)
			{
				for (int i = 0; i < yawCasts; i++)
				{
					yield return Quaternion.AngleAxis(rollDelta * j, forward) *
						(Quaternion.AngleAxis(yawDelta * i, up) * forward);
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
