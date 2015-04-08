using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FourX {

	// Enums
	public enum ObjectType {WorldObject, Map, Shroud, HUD, Other, Invalid}
	public enum ObjectAffiliation {
		Own, Enemy, Ally, Neutral, 
		Other, Unknown,
		Passable, Impassible,
		Left, Right, Up, Down,
		HUD
	}

	public enum CursorState {Select, Attack, Move, Harvest, Left, Right, Up, Down, Default}

	public enum DiplomaticState {Enemy, Ally, Neutral, NoContact, Unknown}
	
	public enum SpaceType {Radius, Square, Blob, Other}

	// Tags
	public class Tags : MonoBehaviour {
		public const string gameController = "GameController";
		public const string player = "Player";
		public const string mainCamera = "MainCamera";
	}

	// Global resources for game
	public static class ResourceManager  {

		// Invalid identifiers
		private static Vector3 invalidPosition = new Vector3(-99999, -99999, -99999);
		public static Vector3 InvalidPosition { get { return invalidPosition; } }
		private static Bounds invalidBounds = new Bounds(new Vector3(-99999, -99999, -99999), new Vector3(0, 0, 0));
		public static Bounds InvalidBounds { get { return invalidBounds; } }

		public static int MoveTolerance = 2;

		public static float DefaultRelations = 50f;
		
		//private static GUISkin selectBoxSkin; //This needs replaced with the new UI Canvas system.
		//public static GUISkin SelectBoxSkin { get { return selectBoxSkin; } }

		public static GameController game;

		private static float mapScale = 1f; //Map's scale in relation to 1 kilometer. So, 0.1 would be 100 meters to a square, 10 would be 10 kilometers to a square.
		public static float MapScale { get { return mapScale; } }

		public static readonly string DisconnectScene = "MainMenu";

	}

	public static class GraphicManager {
		public static Dictionary<ObjectAffiliation, Color> affiliationColoring = new Dictionary<ObjectAffiliation, Color> {};
	}
}

