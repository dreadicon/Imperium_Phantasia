using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using FourX;

public class Players : MonoBehaviour {

	public static Players Instance;

	public NetworkView nView;

	public GameObject LocalPlayerPrefab;

	public GameObject PlayerPrefab;

	private string PlayerSetupRPC;

	void Awake () {
		if (Instance == null) {
			Instance = this;
			PlayerSetupRPC = "PlayerSetup";
		}

	}



	void Start () {


	}

	//The Relation class contains the relationships from one player to all others. 
	//Generally, it is just a container class with some checking helpers to ensure there is always a proper entry for all players.
	public class Relation {

		public Relation (Player player) {
			this.subject = player;
			foreach (Player otherPlayer in Players.Instance.players) {
				this.opinion[otherPlayer] = ResourceManager.DefaultRelations;
				this.history[otherPlayer] = new List<string> {};
				this.relationship[otherPlayer] = DiplomaticState.Unknown;
			}
		}

		public void AddPlayer(Player player, 
		                      DiplomaticState newRelationship = DiplomaticState.Unknown, 
		                      float newOpinion = 9001f, 
		                      List<string> newHistory = null) {

			if (player != subject) {
				if(newHistory == null) newHistory = new List<string> {};
				if(newOpinion == 9001f) newOpinion = ResourceManager.DefaultRelations; //Unity was complainign when I put this in the default field. 
				history [player] = newHistory;
				opinion [player] = newOpinion;
				relationship [player] = newRelationship;
			}
		}

		public Player subject;
		public Dictionary<Player, List<string>> history = new Dictionary<Player, List<string>> {};
		public Dictionary<Player, float> opinion = new Dictionary<Player, float> {};
		public Dictionary<Player, DiplomaticState> relationship = new Dictionary<Player, DiplomaticState> {};
	}



	//Public Variables
	public List<Player> players = new List<Player> {};

	public Dictionary<NetworkViewID, Player> networkMap = new Dictionary<NetworkViewID, Player> {};

	//Public Methods
	public void InitiatePlayerSetup (PlayerPregame pregamePlayer) {
		if (GameController.localPlayer == null) {
			var viewID = Network.AllocateViewID ();
			GameController.LocalPlayerID = viewID;
			nView.RPC (PlayerSetupRPC, RPCMode.All, pregamePlayer.name, pregamePlayer.faction, pregamePlayer.color.Vector3FromColor (), viewID);

		} else {
			Debug.Log("Player setup not initiated: local player already exists!");
		}
	}


	[RPC]
	public void PlayerSetup (string name, string faction, Vector3 color, NetworkViewID id, NetworkMessageInfo info) {
		if (name == "" || name == null) 
			return;
		foreach(Player existingPlayer in players)
			if (existingPlayer.name == name || existingPlayer.color == color.ColorFromVector3() || networkMap.ContainsKey(id)) 
				return; //Name or Color already taken; add player fails.

		Player player;
		Debug.Log ("Sender: " + info.sender.ToString ());
		Debug.Log ("reciever: " + Network.player.ToString ());
		if (id == GameController.LocalPlayerID) {
			Debug.Log("creating local player");
			var localPlayer = Instantiate(LocalPlayerPrefab);
			player = localPlayer.GetComponent<LocalPlayer> ();
			GenericPlayerSetup(player, name, faction, color, id);

			GameController.localPlayer = localPlayer.GetComponent<LocalPlayer> ();
		} else {
			Debug.Log("creating remote player");
			var playerObject = Instantiate(PlayerPrefab);
			player = playerObject.GetComponent<Player> ();
			GenericPlayerSetup(player, name, faction, color, id);
		}

	}

	private void GenericPlayerSetup (Player player, string name, string faction, Vector3 color, NetworkViewID id) {
		var playerNetView = (NetworkView)player.gameObject.AddComponent<NetworkView> ();
		playerNetView.viewID = id;
		playerNetView.observed = player;
		player.name = name;
		//TODO:set player faction
		player.color = color.ColorFromVector3();
		AddPlayerRelations(player);
		players.Add (player);
		networkMap.Add (id, player);
		player.transform.parent = GameObject.FindGameObjectWithTag ("Players").transform;
	}
	
	//Polymorphic stub for accepting just the Player object, and generating the rest
	public void AddPlayerRelations (Player player) {
		Relation newRelation = new Relation (player);
		SetPlayerRelations (newRelation);
	}

	public void SetPlayerRelations (Relation relation) {
		foreach (KeyValuePair<Player, Relation> player in relations) {
			player.Value.AddPlayer(relation.subject);
		}
	}

	public bool isHostle (Player player1, Player player2) {
		if (relations.ContainsKey(player1))
			if (relations [player1].relationship [player2] == DiplomaticState.Enemy)
				return true;
		return false;
	}

	public DiplomaticState GetAffiliation (Player playerFrom, Player playerTo) {
		Relation relation = GetRelation (playerFrom);
		if (relation != null)
			return relation.relationship [playerTo];
		//if GetRelation returns null, return the Unknown diplomatic state. probably should also have some kind of debug log too.
		return DiplomaticState.Unknown;
	}


	public void CheckAllReady() {
		foreach (var player in players) {
			if(player.ready == false)
				return;
		}
		ReadyEndTurn = true;
	}

	//Private Variables and Methods
	public bool ReadyEndTurn = false;
	private Dictionary<Player, Relation> relations = new Dictionary<Player, Relation>{};
	
	private Relation GetRelation(Player player) {
		if (relations.ContainsKey (player)) 
			return relations [player];
		return null;
	}



}

