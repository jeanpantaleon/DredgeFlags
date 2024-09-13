using UnityEngine;

namespace DredgeFlags
{
    public class Loader
	{
		/// <summary>
		/// This method is run by Winch to initialize your mod
		/// </summary>
		public static void Initialize()
		{
			var gameObject = new GameObject(nameof(DredgeFlags));
			gameObject.AddComponent<DredgeFlags>();
			GameObject.DontDestroyOnLoad(gameObject);
		}
	}
}