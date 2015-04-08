/* This class exists to handle any and all actions a WorldObject can perform. 
 * Anything and everything that makes a change that can't be undone must inherit this.
 * Moreover, the game works by assuming any impactful change will go through an action.
 * This is one of the only points of communication used, so any action that other players'
 * clients should be aware of must also use this system, or implement a custom RPC/Unity
 * handler to manage distributing updated info to the other players.
 * 
 * Note also that generic AI behavior for the given action should also go here.
 * 
 */

using UnityEngine;
using System;
using System.Collections;
using FourX;

// T is the data type of the serialized object, which must be known in order to deserialize it.
[RequireComponent(typeof(WorldObject))]
public class Action<T, W> : BaseAction where W : WorldObject {

	protected W worldObject;

	protected Type type;

	protected virtual void Start () {
		worldObject = GetComponent<W> ();
		worldObject.EnableAction (this);
	}

	public void SendCommand( T arguments) {
		if (enabled) {

			byte[] binary = NetworkManager.SerializeBinary<T> (arguments);

			// Prepend the ID
			Debug.Log("binary object action args: " + binary.ToString());

			var id = new byte[1];
			id[0] = actionID;
			worldObject.SendAction(binary, id);
		}
	}
	
	public override void RecieveCommand ( byte[] arguments) {
		// Start at 1 rather than 0, as the first byte is the action's ID.
		T deserializedArgs = NetworkManager.DeserializeBinary<T> (arguments);
		action ( deserializedArgs);

	}

	protected virtual void action (T arguments) { }

}

public abstract class BaseAction : MonoBehaviour {
	public abstract void RecieveCommand (byte[] arguments);
	public byte actionID;
}

interface IAction<T> {

}
