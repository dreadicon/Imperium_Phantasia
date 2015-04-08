using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MultiplayerMenu : Menu {

	public GameObject gameListText;

	public Transform availableGamesPanel;

	private int waitTimer = 400;

	public bool connecting = false;

	protected override void Start () {
		base.Start ();
		Hide ();
	}

	int tick = 0;
	protected override void Update () {

		if (enabled ) {
			if(waitTimer > 600) {
				RefreshView();
				waitTimer = 0;
			} else 
				waitTimer++;
		}
		if (connecting) {
			if(tick >180) {
				ConnectionTesterStatus status = Network.TestConnection();
				Debug.Log(status.ToString());
				if(status == ConnectionTesterStatus.PublicIPIsConnectable || status == ConnectionTesterStatus.NATpunchthroughFullCone)
					transform.parent.GetComponentInChildren<GameSetupMenu> ().Join();
				tick = 0;
			}
			tick++;
		}

	}

	public void OpenGamesWindow() {

	}

	public void RefreshView () {
		if (NetworkManager.Instance.hostData != null) {
			var children = new List<GameObject> ();
			foreach (Transform child in availableGamesPanel) 
				children.Add (child.gameObject);
			children.ForEach (child => Destroy (child));
			foreach (HostData host in NetworkManager.Instance.hostData) {
				var gameText = (GameObject)Instantiate(gameListText);
				gameText.GetComponent<Text>().text = host.gameName;
				gameText.GetComponent<ConnectableGame>().hostData = host;
				gameText.transform.SetParent(availableGamesPanel);
			}
		}
	}

	public void StartServer () {
		if(!Network.isClient)
			NetworkManager.Instance.StartServer ();
	}

	public void RefreshServerList() {
		NetworkManager.Instance.RefreshHostList ();
		RefreshView();
	}

	public void JoinGame () {
		if (NetworkManager.Instance.targetHostConnection != null && !Network.isServer) {
			transform.parent.GetComponentInChildren<GameSetupMenu> ().Show();
			NetworkManager.Instance.Connect ();
			connecting = true;
		}
	}


}
