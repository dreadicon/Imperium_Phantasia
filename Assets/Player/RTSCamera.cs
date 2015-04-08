using UnityEngine;
using System.Collections;
using FourX;
using UnityEngine.EventSystems;

public class RTSCamera : MonoBehaviour {
	
	private bool moved = false;
	private bool rotated = false;
	public float vertical = 0;
	public Vector3 movement;
	public Vector3 rotation;

	void Start () {
		movement = new Vector3 (0, 0, 0);
		rotation = new Vector3 (0, 0, 0);
	}
	
	void Update () {

	}

	public void PanRight () {
		movement.x += UserSettings.PanSpeed;
		moved = true;
	}

	public void PanLeft () {
		movement.x -= UserSettings.PanSpeed;
		moved = true;
	}

	public void PanUp () {
		movement.z += UserSettings.PanSpeed;
		moved = true;
	}

	public void PanDown () {
		movement.z -= UserSettings.PanSpeed;
		moved = true;
	}

	public void UpdateHeight(float height) {
		vertical = -UserSettings.PanSpeed * height;
		moved = true;
	}

	public void Rotate (float x, float y) {
		rotation.x = -x * UserSettings.DynamicRotateAmount;
		rotation.y = y * UserSettings.DynamicRotateAmount;
		rotated = true;
	}

	public void MoveCamera () {
		if (moved) {
			Vector3 origin = Camera.main.transform.position;
			Vector3 destination = origin;
			//Transform movement given from relative to global
			movement = Camera.main.transform.TransformDirection (movement);
			movement.y = vertical;
			destination += movement;

			//limit away from ground movement to be between a minimum and maximum distance
			if (destination.y > UserSettings.MaxCameraHeight) {
				destination.y = UserSettings.MaxCameraHeight;
			} else if (destination.y < UserSettings.MinCameraHeight) {
				destination.y = UserSettings.MinCameraHeight;
			}

			//Perform camera movement update
			Camera.main.transform.position = Vector3.MoveTowards(origin, destination, Time.deltaTime * UserSettings.PanSpeed);

			//clear movement for next frame
			movement.Set(0,0,0);
			moved = false;
		}
		if (rotated) {
			Vector3 origin = Camera.main.transform.eulerAngles;
			Vector3 destination = origin;
			destination.x += rotation.x;
			destination.y += rotation.y;

			Camera.main.transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime * UserSettings.DynamicRotateSpeed);

			rotation = Vector3.zero;
			rotated = false;
		}
	}
}


//Old code for camera. had some interesting zoom features, so might want to use some of it later.
/*
		//initialPosition = transform.position;      
		//initialRotation = transform.eulerAngles;

		// panning     
		if ( Input.GetMouseButton( 2 ) ) {
			destination.x -= Input.GetAxis("Mouse Y") * rotateSpeed;
			destination.y += Input.GetAxis("Mouse X") * rotateSpeed;

			//Code for panning instead of look for use with middle mouse. Might use this later somehow.
			//transform.Translate(Vector3.right * Time.deltaTime * PanSpeed * (Input.mousePosition.x - Screen.width * 0.5f) / (Screen.width * 0.5f), Space.World);
			//transform.Translate(Vector3.forward * Time.deltaTime * PanSpeed * (Input.mousePosition.y - Screen.height * 0.5f) / (Screen.height * 0.5f), Space.World);
		}

		if ( Input.GetKey("a") || Input.mousePosition.x <= Screen.width * ScrollEdge)  {             
				transform.Translate(Vector3.right * Time.deltaTime * -PanSpeed, Space.Self );   
			}
		else if ( Input.GetKey("d") || Input.mousePosition.x >= Screen.width * (1 - ScrollEdge)) {            
				transform.Translate(Vector3.right * Time.deltaTime * PanSpeed, Space.Self );              
			}
			
		if ( Input.GetKey("w") || Input.mousePosition.y >= Screen.height * (1 - ScrollEdge) ) {            
				transform.Translate(Vector3.forward * Time.deltaTime * PanSpeed, Space.Self );             
			}
		else if ( Input.GetKey("s") || Input.mousePosition.y <= Screen.height * ScrollEdge ) {         
				transform.Translate(Vector3.forward * Time.deltaTime * -PanSpeed, Space.Self );   

			}  
			
			if ( Input.GetKey("q") ) {
				transform.Rotate(Vector3.up * Time.deltaTime * -rotateSpeed, Space.World);
			}
			else if ( Input.GetKey("e")) {
				transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed, Space.World);
			}

		
		// zoom in/out
		CurrentZoom -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 1000 * ZoomZpeed;
		
		CurrentZoom = Mathf.Clamp( CurrentZoom, zoomRange.x, zoomRange.y );
		
		transform.position = new Vector3( transform.position.x, transform.position.y - (transform.position.y - (initialPosition.y + CurrentZoom)) * 0.1f, transform.position.z );
		
		float x = transform.eulerAngles.x - (transform.eulerAngles.x - (initialRotation.x + CurrentZoom * ZoomRotation)) * 0.1f;
		x = Mathf.Clamp( x, zoomAngleRange.x, zoomAngleRange.y );
		
		transform.eulerAngles = new Vector3( x, transform.eulerAngles.y, transform.eulerAngles.z );

		//if a change in position is detected perform the necessary update
		if (destination != origin) {
			Camera.main.transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime * rotateSpeed);
		}
*/
