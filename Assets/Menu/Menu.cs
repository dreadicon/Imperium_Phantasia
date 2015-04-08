using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Canvas))]
public class Menu : MonoBehaviour {

	protected Canvas menuCanvas;

	protected virtual void Awake () {
		menuCanvas = GetComponent<Canvas> ();
	}

	protected virtual void Start () {

	}

	protected virtual void Update () {
		if (Input.GetKeyDown (KeyCode.Escape))
			Back ();
	}

	public void NewGame() {
		Application.LoadLevel ("Map");
		Time.timeScale = 1.0f;
	}
	
	public void ExitGame() {
		Application.Quit ();
	}

	public void Show() {
		menuCanvas.enabled = true;
		enabled = true;
	}

	public void Hide() {
		menuCanvas.enabled = false;
		enabled = false;
	}

	public virtual void Back () {

	}

}
