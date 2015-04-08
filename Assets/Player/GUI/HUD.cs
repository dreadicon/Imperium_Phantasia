using UnityEngine;
using System.Collections;
using FourX;

[RequireComponent(typeof(MouseCursor))]
public class HUD : MonoBehaviour {

	//Unfixed variables
	private ObjectAffiliation currentHover;
	public bool MenuOpen = false;

	//Fixed variables
	private LocalPlayer player;
	public InfoBox infoBox;
	private Sidebar sidebar;
	private SelectionGrid unitHighlight;
	private SelectionGrid cursorHighlight;
	private NavMeshPath path;
	private MouseCursor cursor;

	// Used for initialization
	void Start () {
		player = transform.parent.GetComponent<LocalPlayer>();
		infoBox = GetComponentInChildren<InfoBox> ();
		sidebar = GetComponentInChildren<Sidebar> ();
		unitHighlight = GetComponentInChildren<SelectionGrid> ();
		cursor = GetComponent<MouseCursor> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (player && player.localPlayer) {
			UpdateAffiliation();
		}
	}

	public void UpdateAffiliation () {
		if (player.hoverObject != currentHover) {
			currentHover = player.hoverObject;
			if (player.selectedObject) {
				if (player.selectedObject.getOwner () == player) {
					//Handle pathing render
					if (currentHover == ObjectAffiliation.Passable)
						cursor.UpdateCursor (CursorState.Move);
					else if (currentHover == ObjectAffiliation.Enemy)
						cursor.UpdateCursor (CursorState.Attack);
					else 
						cursor.UpdateCursor (CursorState.Default);

				}
			} else {

				cursor.UpdateCursor (CursorState.Select);
			}

		}
	}

	public void Deselect() {
		//infoBox.HideBox ();
		infoBox.Clear ();
	}
	
}
