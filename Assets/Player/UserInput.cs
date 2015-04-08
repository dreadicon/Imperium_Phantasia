using UnityEngine;
using System.Collections;
using FourX;

public class UserInput : MonoBehaviour {

	private LocalPlayer player;
	private RTSCamera gameCamera;
	private bool hudHover = false;
	public ObjectType cursorLocation;
	private ObjectAffiliation cursorAffiliation;
	private Vector3 mouseHitPoint = ResourceManager.InvalidPosition;
	public GameObject mouseObject;

	private bool mouseUp = false;

	// Use this for initialization
	void Start () {
		player = GetComponent<LocalPlayer> ();
		gameCamera = Camera.main.GetComponent<RTSCamera> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (player) {
			MouseHitUpdate ();
			HandleInput();
			gameCamera.MoveCamera();
			//mouseUp = false;
		}

	}

	void OnGUI () {
		if (player) {
			if(Event.current.type == EventType.mouseUp)
				mouseUp = true;
		}
	}

	//stub for event system to set hudHover state.
	public void mouseOverSidebar(bool state) {
		hudHover = state;
		cursorLocation = ObjectType.HUD;
	}

	// handle any pressed keys as applicable
	private void HandleInput() {
		// if buttons are pressed, handle click animations and orders/selections
		if (Input.anyKey) {
			//Camera panning
			if (Input.GetKey(UserSettings.PanUp)){
				gameCamera.PanUp();
			} else if (Input.GetKey(UserSettings.PanDown)){
				gameCamera.PanDown();
			}
			if (Input.GetKey(UserSettings.PanLeft)) {
				gameCamera.PanLeft();
			} else if (Input.GetKey(UserSettings.PanRight)){
				gameCamera.PanRight();
			}
			if(Input.GetKey(UserSettings.RotateCam)) 
				gameCamera.Rotate(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));

			if(Input.GetKeyDown(KeyCode.Escape))
			   OpenPauseMenu();

			// determine if mouse is in-bounds (not over a UI element)
			if (!hudHover) {
				//if(mouseUp) {
					//Mouse Actions
					if(Input.GetKey(UserSettings.Select))
						LeftMouseClick();
					else if (Input.GetKey(UserSettings.Action))
						RightMouseClick();
				//}



			} else {
				// any special key-based actions that only work while over the GUI go here
				
			}
		} 


	}

	private void LeftMouseClick() {
			if (cursorLocation == ObjectType.WorldObject) {
				WorldObject worldObject = mouseObject.GetComponent<WorldObject> (); 
				if (player.selectedObject != worldObject)
					player.SelectObject (worldObject);
				if (cursorAffiliation == ObjectAffiliation.Own) {


				} else if (cursorAffiliation == ObjectAffiliation.Enemy) {


				}
			} else 
				player.Deselect ();

	}

	private void RightMouseClick() {
		if (player.selectedObject && player.selectedObject.getOwner() == player) {
			Debug.Log("valid order");
			player.selectedObject.MouseAction(mouseObject, mouseHitPoint);
		}

	}

	//this function handles the mouse position and all things related to said position,
	//including things like what object it is over, edge scrolling, etc.
	private void MouseHitUpdate() {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			mouseHitPoint = hit.point;
			if(mouseHitPoint != ResourceManager.InvalidPosition) {
				mouseObject = hit.collider.gameObject;
				if(mouseObject.name == "Terrain") {
					cursorLocation = ObjectType.Map;
				} else {

					WorldObject worldObject = mouseObject.GetComponent<WorldObject>();
					if (worldObject) {
						cursorLocation = ObjectType.WorldObject;
						Player owner = worldObject.getOwner();
						if(owner == player) {
							cursorAffiliation = ObjectAffiliation.Own;
						} else if (Players.Instance.isHostle(player, owner)) {
							cursorAffiliation = ObjectAffiliation.Enemy;
						}
					} else cursorLocation = ObjectType.Other;
				}
			} else cursorLocation = ObjectType.Invalid;
		}

		//Handle edge panning for cursor if enabled.
		if (UserSettings.EnableEdgePanning) {
			if (Input.mousePosition.x <= Screen.width * UserSettings.ScrollEdge) {			
				gameCamera.PanLeft();
				cursorAffiliation = ObjectAffiliation.Left;
			} else if (Input.mousePosition.x >= Screen.width * (1 - UserSettings.ScrollEdge)) {
				gameCamera.PanRight();
				cursorAffiliation = ObjectAffiliation.Right;
			}

			if (Input.mousePosition.y >= Screen.height * (1 - UserSettings.ScrollEdge)) {	
				gameCamera.PanUp();
				cursorAffiliation = ObjectAffiliation.Up;
			} else if (Input.mousePosition.y <= Screen.height * UserSettings.ScrollEdge) { 		
				gameCamera.PanDown();
				cursorAffiliation = ObjectAffiliation.Down;
			}
		}

		//Move camera up/down using mouse wheel scroll, but only if not over the sidebar (if scrolling is ever needed in the game)
		if (cursorLocation != ObjectType.HUD && Input.GetAxis ("Mouse ScrollWheel") != 0)
			gameCamera.UpdateHeight (Input.GetAxis ("Mouse ScrollWheel"));

		player.hoverObject = cursorAffiliation;
	}

	private void OpenPauseMenu() {
		Time.timeScale = 0.0f;
		GetComponentInChildren< PauseMenu >().Show();
		GetComponent< UserInput >().enabled = false;
		Cursor.visible = true;
		player.hud.MenuOpen = true;
	}
	
	
}
