using UnityEngine;
using System.Collections;


public static class Utils 
{
	public static string HashToString(ExitGames.Client.Photon.Hashtable hash)
	{
		string output = "";
		foreach (object key in hash.Keys)
		{
			if (key is ExitGames.Client.Photon.Hashtable)
			{
				output += "{" + HashToString((ExitGames.Client.Photon.Hashtable)key) + "}";
			}
			else
			{
				output += "{" + key.ToString() + ":" + hash[key] + "}";
			}
		}
		return output;
	}

	public static string HashToString(Hashtable hash)
	{
		string output = "";
		foreach (object key in hash.Keys)
		{
			output += "{" + key.ToString() + ":" + hash[key] + "}";
		}
		return output;
	}

	public static void RandomizeIntArray(int[] arr)
	{
		for (int i = arr.Length - 1; i > 0; i--) {
			int r = Random.Range(0,i);
			int tmp = arr[i];
			arr[i] = arr[r];
			arr[r] = tmp;
		}
	}
}
