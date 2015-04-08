using UnityEngine;
using System.Collections;
using FourX;

[RequireComponent(typeof(UserInput))]
public class LocalPlayer : Player {

	public HUD hud;
	//The player's currently selected object
	public WorldObject selectedObject;// { get; private set; }
	//The object the player's cursor is over's relation to the player
	public ObjectAffiliation hoverObject;

	protected override void Awake () {
		base.Awake ();
		localPlayer = true;
	}

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		hoverObject = ObjectAffiliation.Other;
		hud = GetComponentInChildren<HUD> ();
	
	}

	public void SelectObject(WorldObject newObject) {
		if(selectedObject)
			selectedObject.SetSelection (false, this);
		
		hud.Deselect ();
		
		selectedObject = newObject;
		
		if (selectedObject) {
			hud.infoBox.enabled = true;
			selectedObject.SetSelection (true, this);
		}
	}
	
	public void Deselect() {
		selectedObject = null;
		hud.Deselect ();
	}
}
