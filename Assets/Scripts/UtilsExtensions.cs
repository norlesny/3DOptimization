using System.Collections.Generic;
using System.Text;

namespace DefaultNamespace
{
	public static class UtilsExtensions
	{
		public static string ToStringExt<T>(this IEnumerable<T> array)
		{
			var stringBuilder = new StringBuilder("Vector3[]: { ");
			int length = 0;
			foreach (T t in array)
			{
				stringBuilder.Append(t + ", ");
				length++;
			}

			stringBuilder.Remove(stringBuilder.Length - 2, 2);
			stringBuilder.Append(" } [" + length + "]");
			return stringBuilder.ToString();
		}
	}
}
