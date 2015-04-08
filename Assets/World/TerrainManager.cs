using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Terrain))]
public class TerrainManager : MonoBehaviour {

	private Terrain terrain;

	// Use this for initialization
	void Start () {
		terrain = GetComponent<Terrain> ();
	}

}
