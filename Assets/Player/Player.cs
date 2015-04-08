using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FourX;


public class Player : MonoBehaviour {

	public bool localPlayer { get; protected set; }

	public string playerName;

	public Color color;

	public Faction faction;

	public bool ready { get; private set; }

	protected virtual void Awake () {
	}

	protected virtual void Start () {

	}
	
	// Update is called once per frame
	protected virtual void Update () {
	
	}



	public void ReadyEndTurn () {
		ready = true;
		Players.Instance.CheckAllReady ();
	}

	public void NotReadyEndTurn () {
		ready = false;
	}


}
