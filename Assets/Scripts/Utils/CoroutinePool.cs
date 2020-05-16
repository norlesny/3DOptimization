using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
	public class CoroutinePool
	{
		private static CoroutinePoolBehaviour pool;

		private static CoroutinePoolBehaviour Pool
		{
			get
			{
				if (pool == null)
				{
					pool = new GameObject(nameof(CoroutinePool)).AddComponent<CoroutinePoolBehaviour>();
				}

				return pool;
			}
		}

		public static void RunDelayed(UnityAction action)
		{
			Pool.StartCoroutine(delayedAction(action));
		}

		private static IEnumerator delayedAction(UnityAction action)
		{
			yield return new WaitForEndOfFrame();
			action.Invoke();
		}

		private class CoroutinePoolBehaviour : MonoBehaviour
		{
			// Nothing
		}
	}
}
