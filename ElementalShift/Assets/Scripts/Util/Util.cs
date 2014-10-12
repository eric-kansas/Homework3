using UnityEngine;
using System.Collections;

public static class Util {

	public static T[] ConvertArrayToType<T>(object[] toConvert) {
		T[] result = new T[toConvert.Length];
		for (int i = 0; i < toConvert.Length; i++)
		{
			object comp = toConvert[i];
			if (comp is T) {
				result[i] = (T)comp;
			}
		}
		return result;
	}
}
