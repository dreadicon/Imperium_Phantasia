using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using FourX;

[RequireComponent(typeof(NetworkView))]
public class WorldObject : MonoBehaviour {

	//public static Dictionary<string, Type> interfaces;


	public string objectName = "default";
	public Texture2D icon;
	
	public bool selected { get; protected set; }

	public Player owner;

	//protected Bounds selectionBounds;
	//protected Mesh locationMesh;
	//protected Rect playingArea = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
	protected float space; // provided in square kilometers.

	// Network tracking for mapping RPCs to actions
	public string ActionRPCString { get; private set; }
	public NetworkView nView;
	private Dictionary<byte, BaseAction> actionMapping;
	

	public Player getOwner(){ 
		return owner;
	}

	protected virtual void Awake() {
		nView = GetComponent<NetworkView> ();
		GameController.Instance.AddWorldObject (this, nView.viewID);
		actionMapping = new Dictionary<byte, BaseAction> {};
		ActionRPCString = "ObjectAction";
		selected = false;
		//selectionBounds = ResourceManager.InvalidBounds;
		//CalculateBounds();
	}

	protected virtual void Start () {

	}

	protected virtual void Update() {
		
	}

	protected virtual void FixedUpdate() {

	}

	protected virtual void OnDisable () {
		foreach (var actionMap in actionMapping) {
			actionMap.Value.enabled = false;
		}
		actionMapping.Clear();
	}

	// Action functions
	public void EnableAction(BaseAction action) {
		byte count = (byte)actionMapping.Count;
		if (actionMapping.ContainsKey (count)) 
			count += 1;
		action.actionID = count;
		actionMapping.Add (count, action);
		action.enabled = true;
	}

	public void DisableAction (BaseAction action) {
		actionMapping.Remove (action.actionID);
		action.enabled = false;
	}

	// Network Action functions

	public void SendAction (byte[] serializedParams, byte[] id) {
		nView.RPC (ActionRPCString, NetworkManager.Instance.GameModeRPC, serializedParams, id);

	}

	[RPC]
	public void ObjectAction (byte[] serializedParams, byte[] id) {
		BaseAction action = actionMapping [id[0]];
		action.RecieveCommand(serializedParams);
	}

	// Universal functions concerning local player
	public virtual void SetSelection(bool select, LocalPlayer player) {
		if(player == owner) {
			selected = select;
		}
	}

	public virtual void MouseAction(GameObject hitObject, Vector3 hitPoint) {

	}


	// Helper functions
	/*
	public void CalculateBounds() {
		selectionBounds = new Bounds(transform.position, Vector3.zero);
		foreach (Renderer r in GetComponentsInChildren<Renderer>()) {
			selectionBounds.Encapsulate(r.bounds);
		}
	}
	*/

}
