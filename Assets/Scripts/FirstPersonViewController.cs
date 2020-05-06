using UnityEngine;

public class FirstPersonViewController : MonoBehaviour
{
	[SerializeField] private float speedH = 2.0f;
	[SerializeField] private float speedV = 2.0f;

	private float yaw;
	private float pitch;

	private Transform trans;

	private void Start()
	{
		trans = transform;
	}

	private void Update()
	{
		yaw += speedH * Input.GetAxis("Mouse X");
		pitch -= speedV * Input.GetAxis("Mouse Y");

		trans.eulerAngles = new Vector3(pitch, yaw, 0.0f);
	}
}
