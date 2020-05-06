using UnityEngine;

namespace DefaultNamespace
{
	public interface ICamerasProvider
	{
		Camera[] Cameras { get; }
	}
}
