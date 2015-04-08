using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FourX;

public enum CursorPosition {TopLeft, Middle, Top, Left, Right, Bottom};

public class MouseCursor : MonoBehaviour {

	public class CustomCursor {

		public CustomCursor(Texture2D[] tex, bool anim, CursorPosition pos) {
			cursor = tex;
			isAnimated = anim;

			//Calculate hotspot position based on 0,0 being top right of cursor icon.
			//I'm not usually a fan of switch statements, but this is only called at start, and makes things tidy.
			hotSpot = new Vector2();
			switch (pos) {
			case CursorPosition.TopLeft:
				hotSpot = Vector2.zero;
				break;
			case CursorPosition.Middle:
				hotSpot.x = tex [0].width / 2;
				hotSpot.y = tex [0].height / 2;
				break;
			case CursorPosition.Top:
				hotSpot.x = tex [0].width / 2;
				hotSpot.y = 0;
				break;
			case CursorPosition.Bottom:
				hotSpot.x = tex[0].width / 2;
				hotSpot.y = tex[0].height;
				break;
			case CursorPosition.Right:
				hotSpot.x = tex[0].width;
				hotSpot.y = tex[0].height / 2;
				break;
			case CursorPosition.Left:
				hotSpot.x = 0;
				hotSpot.y = tex[0].height / 2;
				break;
			default:
				hotSpot = Vector2.zero;
				break;
			}
		}

		public Texture2D[] cursor;
		public bool isAnimated;
		public Vector2 hotSpot;

	}

	public Dictionary<CursorState, CustomCursor> cursors = new Dictionary<CursorState, CustomCursor> {};

	private bool cursorAnimated;
	private int currentFrame = 0;

	private Vector2 hotSpot = Vector2.zero;
	private CursorMode cursorMode = CursorMode.Auto;

	public Animation testCursor;

	public CursorState activeState;
	public CustomCursor activeCursor;
	public Texture2D[] selectCursor, leftCursor, rightCursor, upCursor, downCursor, defaultCursor;
	public Texture2D[] moveCursors, attackCursors, harvestCursors;

	// Use this for initialization
	void Start () {
		cursors [CursorState.Attack] = new CustomCursor (attackCursors, true, CursorPosition.TopLeft);
		cursors [CursorState.Select] = new CustomCursor (selectCursor, false, CursorPosition.Middle);
		cursors [CursorState.Move] = new CustomCursor (moveCursors, true, CursorPosition.Middle);

	}
	
	// Update is called once per frame
	void OnGUI () {
		DrawCursor ();
	}

	public void DrawCursor () {
		if (cursorAnimated) {
			currentFrame = (int)Time.time % activeCursor.cursor.Length;
			Cursor.SetCursor (activeCursor.cursor[currentFrame], activeCursor.hotSpot, cursorMode);
		}
	}

	public void UpdateCursor (CursorState state) {
		if (state != activeState) {
			if(state == CursorState.Default) {
				Cursor.SetCursor(null, Vector2.zero, cursorMode);
			}else {
			activeCursor = cursors [state];
			activeState = state;
			Cursor.SetCursor (activeCursor.cursor[0], activeCursor.hotSpot, cursorMode);
			}
		}

	}
}
