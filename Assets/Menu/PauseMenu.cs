using UnityEngine;
using System.Collections;

public class PauseMenu : Menu {



	protected override void Awake () {
		base.Awake ();
		menuCanvas.enabled = false;
		GetComponent<PauseMenu> ().enabled = false;
	}

	protected override void Update () {
		base.Update ();
	}

	public override void Back () {
		Time.timeScale = 1.0f;
		var player = transform.parent.GetComponent<LocalPlayer> ();
		if (player) {
			player.GetComponent<UserInput> ().enabled = true;
			player.hud.MenuOpen = false;
		}
		Hide ();
	}


	public void ReturnToMainMenu () {
		Application.LoadLevel ("MainMenu");
	}
	
}
