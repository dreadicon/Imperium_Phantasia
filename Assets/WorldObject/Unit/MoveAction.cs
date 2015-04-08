using UnityEngine;
using System.Collections;

public class MoveAction : Action<Vector3, Unit> {

	protected override void action (Vector3 arguments) {
		worldObject.moving = true;
		worldObject.navAgent.destination = arguments;
	}
}



