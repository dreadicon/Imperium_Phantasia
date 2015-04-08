using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FourX;


//TODO: probably should rework this class to be volatile, moving persistent variables 
// to the NetworkManager, as that seems more appropriate. 
public class GameController : MonoBehaviour {

	public static GameController Instance = null;
	// Local player info
	public static NetworkViewID LocalPlayerID;

	private static LocalPlayer localplayer = null;
	public static LocalPlayer localPlayer {
		get {
			return localplayer;
		}
		set {
			localplayer = value;
			Instance.setupLocalPlayer();
		}
	}

	////////////////Test stuff; rework later////////////////

	public GameObject testUnitPrefab;

	// TODO: replace this with dynamic player start locations
	private Stack<Vector3> spawnPoints = new Stack<Vector3> (
		new[] {
		new Vector3(30f, 0f, 3f),
		new Vector3(38f, 0f, 3f),
		new Vector3(20f, 0f, 3f)
	}
	);
	
	public Vector3 GetNextSpawnPoint () {
		var point = spawnPoints.Pop();
		if (point != null)
			return point;
		return Vector3.zero;
	}


	//////////////// public variables////////////////


	public string MapName;

	public NetworkView nView;

	//public List<PlayerPregame> pregamePlayers = new List<PlayerPregame> {};
	public PlayerPregame playerInfo;
	public int playerCount = 1;

	// Indicates that the game is live; not longer in any kind of start menu
	public bool GameLive;

	////////////////Private variables////////////////
	private bool turnTransitioning = false;

	private bool networkActive = false;



	private Dictionary<NetworkViewID, WorldObject> idMapping = new Dictionary<NetworkViewID, WorldObject> {};



	public bool TurnTransition () {
		return turnTransitioning;
	}

	// Use this for initialization
	void Awake () {
		GameLive = false;
		Instance = this;
		DontDestroyOnLoad(transform.gameObject);
		nView = GetComponent<NetworkView> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (GameLive && Players.Instance.ReadyEndTurn)
			turnTransitioning = true;
		//if(networkActive)

	}

	public WorldObject GetWorldObject (NetworkViewID id) {
		if (idMapping.ContainsKey (id))
			return idMapping [id];
		return null;
	}

	public void AddWorldObject ( WorldObject newObject, NetworkViewID id) {
		idMapping.Add (id, newObject);
	}

	public void RemoveWorldObject (NetworkViewID id) {
		idMapping.Remove (id);
	}

	public void setupLocalPlayer () {
		if (localPlayer != null) {
			nView.RPC("SpawnUnit", RPCMode.AllBuffered, LocalPlayerID, Network.AllocateViewID());
		}
	}

	[RPC]
	public void SpawnUnit (NetworkViewID player, NetworkViewID id) {
		var position = GetNextSpawnPoint ();
		if (position == Vector3.zero)
			position = GetNextSpawnPoint ();
		var newUnit = (GameObject)Instantiate (testUnitPrefab, position, Quaternion.identity);
		newUnit.AddComponent<NetworkView> ().viewID = id;
		Player owner = Players.Instance.networkMap [player];
		newUnit.AddComponent<Unit> ().owner = owner;
		newUnit.AddComponent<MoveAction> ();
		newUnit.transform.parent = owner.transform.FindChild ("Units").transform;

	}

	void OnNetworkLoadedLevel () {
		Players.Instance.InitiatePlayerSetup (playerInfo);
	}

	void OnPlayerConnected() {


	}
	
	void OnPlayerDisconnected() {
		playerCount--;
		Debug.Log ("players connected: " + playerCount.ToString ());
	}

}
