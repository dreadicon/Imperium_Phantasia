using UnityEngine;
using System.Collections;

namespace FourX {
	public class PlayerPregame : MonoBehaviour {

		void Start () {
			DontDestroyOnLoad (transform.gameObject);
		}
	// Class used to hold player data from the pregame screen, used later to initialize the players.
		public string name;
		public Color color;
		public string faction;
		public bool human;
	}
}
