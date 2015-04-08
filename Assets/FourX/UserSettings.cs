using UnityEngine;
using System.Collections;

namespace FourX {
	public static class UserSettings {

		//These are settings for Camera
		public static bool EnableEdgePanning = true;
		public static bool EnableZoom = true;
		public static float MaximumZoom = 1f;
		public static float MinimumZoom = 20f;
		public static float MaxCameraHeight = 60f;
		public static float MinCameraHeight = 3f;
		//amount of the screen that will activate the scrolling, where 0.1 is 10% of the screen
		public static float ScrollEdge = 0.06f;
		//Rotate speed used for rotating the screen using fixed input (buttons rather than mouse position/movement)
		public static float FixedRotateSpeed = 50;
		public static float DynamicRotateSpeed = 100;
		public static float DynamicRotateAmount = 10;
		public static float PanSpeed = 10;

		//Keybindings
		public static KeyCode PanLeft = KeyCode.A;
		public static KeyCode PanRight = KeyCode.D;
		public static KeyCode PanUp = KeyCode.W;
		public static KeyCode PanDown = KeyCode.S;
		public static KeyCode Select = KeyCode.Mouse0;
		public static KeyCode Action = KeyCode.Mouse1;
		public static KeyCode RotateCam = KeyCode.Mouse2;

		//Player Name default
		public static string DefaultName = "Ruler";
	}
}

