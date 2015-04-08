using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {

	private int playerIDcounter = 0;

	private int[,] provinceMap;

	private int[,] playerMap;

	private List<Province> provinces = new List<Province> {};

	private Dictionary<int, Player> playerID = new Dictionary<int, Player> {};

	private Dictionary<int, Province> provinceID = new Dictionary<int, Province> {};

	private Dictionary<Player, List<Province>> provincesByPlayer = new Dictionary<Player, List<Province>> {};

	private NetworkView nView;



	void Awake () {
		nView = GetComponent<NetworkView> ();
	}

	// Use this for initialization
	void Start () {
	
	}




	public Player GetOwner(int x, int y) {
		return playerID [playerMap [x, y]];
	}

	public Province GetProvince(int x, int y) {
		return provinceID [provinceMap [x, y]];
	}

	public List<Province> GetPlayerProvinces (Player player) {
		if (provincesByPlayer.ContainsKey (player)) {
			return provincesByPlayer [player];
		} else
			return null;
	}

}
