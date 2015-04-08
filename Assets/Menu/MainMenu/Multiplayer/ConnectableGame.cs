using UnityEngine;
using System.Collections;

public class ConnectableGame : MonoBehaviour {

	public HostData hostData;

	public void Select () {
		NetworkManager.Instance.targetHostConnection = hostData;
		Debug.Log ("selecting game " + hostData.gameName + " : " + hostData.comment);
	}
}
