using System.Collections.Generic;
using UnityEngine;

namespace MyCamera.Providers
{
	public class TopDownViewCamerasProvider : CameraProviderBehaviour
	{
		[SerializeField] private Camera targetCamera;
		[SerializeField] private float width = 1f;
		[SerializeField] private float height = 1f;
		[SerializeField] private int horizontalPoints = 3;
		[SerializeField] private int verticalPoints = 3;
		[SerializeField] private bool drawGizmo;

		public override IEnumerator<Camera> GetEnumerator()
		{
			var camera = targetCamera;
			var cameraTransform = camera.transform;
			var startPosition = cameraTransform.position;
			foreach (Vector3 p in GetCameraPositions(startPosition))
			{
				cameraTransform.position = p;
				yield return camera;
			}

			cameraTransform.position = startPosition;
		}

		private IEnumerable<Vector3> GetCameraPositions(Vector3 cameraPosition)
		{
			var positions = new List<Vector3>();
			var horizontalDelta = width / (horizontalPoints - 1);
			var verticalDelta = height / (verticalPoints - 1);
			var halfWidth = width / 2;
			var halfHeight = height / 2;
			for (int i = 0; i < horizontalPoints; i++)
			{
				float horizontalOffset = horizontalPoints > 1 ? -halfWidth + horizontalDelta * i : 0;
				for (int j = 0; j < verticalPoints; j++)
				{
					float verticalOffset = verticalPoints > 1 ? -halfHeight + verticalDelta * j : 0;
					positions.Add(cameraPosition + new Vector3(horizontalOffset, 0, verticalOffset));
				}
			}

			return positions;
		}

		private void OnDrawGizmosSelected()
		{
			if (drawGizmo)
			{
				DrawCameraPositionsGizmo();
			}
		}

		private void DrawCameraPositionsGizmo()
		{
			Vector3 cameraPosition = targetCamera.transform.position;
			Gizmos.DrawWireCube(cameraPosition, new Vector3(width, 0, height));
			foreach (Vector3 p in GetCameraPositions(cameraPosition))
			{
				Gizmos.DrawSphere(p, 0.05f);
			}
		}
	}
}
