using UnityEngine;
using System.Collections;
using FourX;

public class PlayerPregameGUI : MonoBehaviour {

	public PlayerPregame player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	/*
	void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info) {
		Vector3 color = Vector3.zero;
		string name = "";
		string faction = "";
		bool human = false;
		if (stream.isWriting) {
			color = player.color.Vector3FromColor();
			stream.Serialize(ref color);
		} else {
			stream.Serialize(ref color);
			player.color = color.ColorFromVector3();
		}
	}
	*/

}
