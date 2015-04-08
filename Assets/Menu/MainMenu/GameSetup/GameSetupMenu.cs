using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using FourX;

public class GameSetupMenu : Menu {

	public Dictionary<PlayerPregame, PlayerPregameGUI> pregamePlayerMap = new Dictionary<PlayerPregame, PlayerPregameGUI> {};
	public GameObject playerSetupPrefab;

	public PlayerPregame playerPregame;

	public List<string> playerAddQueue = new List<string> {};

	public GameObject pregamePlayers;

	public NetworkView nView;

	public List<Color> playerColors = new List<Color> {
		Color.red,
		Color.blue,
		Color.green,
		Color.yellow,
		Color.cyan,
		Color.black,
		Color.white,
		Color.magenta
	};

	private string map = "Map";

	protected override void Awake () {
		base.Awake ();
		nView = GetComponent<NetworkView> ();

		Hide ();
	}

	// Use this for initialization
	protected override void Start () {
	}

	int tick = 0;
	// Update is called once per frame
	protected override void Update () {
		if (playerAddQueue.Count > 0)
			AddPlayers ();
	}

	public void Join () {
		string name = Network.player.guid.ToString();
		Debug.Log ("Joining game as " + name);
		nView.RPC ("AddPlayer", RPCMode.AllBuffered, name);
	}

	[RPC]
	public void AddPlayer (string name, NetworkMessageInfo info) {
		//Use a queue so that the player gets added in the update event.
		//Required because uGUI elements don't seem to like being instantiated whenever
		//it is that RPCs get called.
		playerAddQueue.Add (name);
		GameController.Instance.playerCount++;
		Debug.Log ("players connected: " + GameController.Instance.playerCount.ToString ());
	}

	public void AddPlayerNormal (string name) {
		Debug.Log ("add player called for " + name);
		var player = Instantiate<PlayerPregame> (playerPregame);
		player.name = name;
		player.color = playerColors [pregamePlayerMap.Count];
		player.human = true;

		var playerObject = (GameObject)Instantiate (playerSetupPrefab);
		playerObject.transform.SetParent(pregamePlayers.transform);
		pregamePlayerMap.Add (player, playerObject.GetComponent<PlayerPregameGUI> ());
		playerObject.GetComponentInChildren<InputField> ().text = name;
		playerObject.transform.FindChild ("Color").GetComponent<Image>().color = player.color;
		if (name == Network.player.guid.ToString ())
			GameController.Instance.playerInfo = player;
	}

	public void TestAddPlayer() {
		string name = Network.player.guid.ToString();
		nView.RPC ("AddPlayer", RPCMode.AllBuffered, name);
	}

	public void AddPlayers () {
		foreach (var name in playerAddQueue) {
			AddPlayerNormal (name);
		}
		playerAddQueue.Clear();
	}


	public void SelectMap () {

	}


	void OnConnectedToServer() {
		Join ();
	}

	void OnServerInitialized() {
		Join ();
	}

}


