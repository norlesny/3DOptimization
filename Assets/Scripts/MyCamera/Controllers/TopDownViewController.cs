using UnityEngine;

namespace MyCamera.Controllers
{
	public class TopDownViewController : MonoBehaviour
	{
		[SerializeField] private float speed = 2.0f;

		private void Update()
		{
			var direction = Vector2.zero;
			foreach ((KeyCode, Vector2) x in new[]
			{
				(KeyCode.A, Vector2.left), (KeyCode.D, Vector2.right),
				(KeyCode.W, Vector2.up), (KeyCode.S, Vector2.down)
			})
			{
				if (Input.GetKey(x.Item1))
				{
					direction += x.Item2;
				}
			}

			transform.position += new Vector3(direction.x, 0, direction.y) * speed * Time.deltaTime;
		}
	}
}
