using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using FourX;

[RequireComponent(typeof(NetworkView))]
public class NetworkManager : MonoBehaviour {

	public string gameName = "PhantasticaImperiumGame";
	private bool refreshing = false;
	public HostData[] hostData;
	public HostData targetHostConnection;
	public string selectedMap = "Map";

	//prepended to network view IDs to prevent collisions
	int lastLevelPrefix = 0;
	
	// if desired, this can be changed to Host, for centralized server setup. by default it is set to 'All', so there is no host.
	// A couple other things also need changed. Will fully implement later.
	public RPCMode GameModeRPC;

	private NetworkView nView;

	public static NetworkManager Instance;

	void Awake () {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad(transform.gameObject);
		}
		GameModeRPC = RPCMode.All;
		nView = GetComponent<NetworkView> ();
		nView.group = 1;
	}

	// Connection manager
	public void RefreshHostList () {
		MasterServer.RequestHostList (gameName);
		//refreshing = true;
		Debug.Log ("Refreshing host list");

	}

	public void Connect() {
		if (targetHostConnection != null) {
			Network.Connect(targetHostConnection);
			Debug.Log("connecting to game");
		}
	}

	//Server Functions
	public void StartServer () {
		if (!Network.isServer && !Network.isClient) {
			Debug.Log ("Initializing sever");
			Network.InitializeServer (32, 25017, !Network.HavePublicAddress ());
			MasterServer.RegisterHost (gameName, "test game", "this is a test");
		}

	}

	// StartGame
	public void StartGame () {
		Network.RemoveRPCsInGroup (0);
		Network.RemoveRPCsInGroup (1);
		nView.RPC ("LoadLevel", RPCMode.AllBuffered, selectedMap, lastLevelPrefix + 1);
	}

	[RPC]
	IEnumerator LoadLevel (string level, int levelPrefix) {
		lastLevelPrefix = levelPrefix;
		// Disable message sending over standard channel
		Network.SetSendingEnabled (0, false);
		// Halt message execution till level is loaded
		Network.isMessageQueueRunning = false;
		// set prefix for net views for this level to avoid collisions
		Network.SetLevelPrefix (levelPrefix);

		Application.LoadLevel (level);
		yield return null;

		// resume message execution
		Network.isMessageQueueRunning = true;
		// Enable message sending over standard channel
		Network.SetSendingEnabled(0, true);

		var gameObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
		foreach (var GObject in gameObjects)
			GObject.SendMessage("OnNetworkLoadedLevel", SendMessageOptions.DontRequireReceiver);

	}

	//Serialization 
	public bool SendObject<T> (T sendItem, string target) {
		if (typeof(T).IsSerializable) {
			var memStream = new MemoryStream ();
			var binFormatter = new BinaryFormatter();

			binFormatter.Serialize (memStream, sendItem);
			byte[] serializedObject = memStream.ToArray ();
			
			memStream.Close ();

			nView.RPC(target, RPCMode.All, serializedObject);

			return true;
		} else {
			return false;
		}
	}

	public static byte[] SerializeBinary<T> (T sendItem) {
		var memStream = new MemoryStream ();
		var binFormatter = new BinaryFormatter ();
		if (!typeof(T).IsSerializable) {

			if (typeof(T) == typeof(Vector3)) {
				//Handle Unity's stupid non-serializable Vector3 type. I mean, seriously!?
				var surrogateSelector = new SurrogateSelector ();
				surrogateSelector.AddSurrogate (typeof(Vector3), new StreamingContext (StreamingContextStates.All),
				                               new Vector3Surrogate ());
				binFormatter.SurrogateSelector = surrogateSelector;

			} else {
				Debug.Log ("Type Not Serializable!");
				return null;
			}
		}
		binFormatter.Serialize (memStream, sendItem);
		
		byte[] serializedObject = memStream.ToArray ();
		memStream.Close ();
		return serializedObject;
	}
	
	public static T DeserializeBinary<T> (byte[] byteStream, int start = 0) {
		var memStream = new MemoryStream ();
		var binFormatter = new BinaryFormatter ();
		if (typeof(T) == typeof(Vector3)) {
			var surrogateSelector = new SurrogateSelector();
			surrogateSelector.AddSurrogate(typeof (Vector3), new StreamingContext(StreamingContextStates.All),
			                               new Vector3Surrogate());
			binFormatter.SurrogateSelector = surrogateSelector;
		}

		
		memStream.Write (byteStream, start, byteStream.Length);
		
		memStream.Seek (start, SeekOrigin.Begin);


		
		return (T)binFormatter.Deserialize (memStream);
	}


	//Unity Events
	void OnServerInitialized() {
		Debug.Log ("Server Initialized");
	}

	void OnMasterServerEvent(MasterServerEvent e) {
		if (e == MasterServerEvent.RegistrationSucceeded) {
			Debug.Log ("Registration Successful");
		} else if (e == MasterServerEvent.HostListReceived)
			hostData = MasterServer.PollHostList ();
	}

	void OnConnectedToServer() {
		Debug.Log ("Server Joined");
	}

	void OnDisconnectedFromServer () {
		Application.LoadLevel (ResourceManager.DisconnectScene);
	}


}

