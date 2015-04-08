using UnityEngine;
using System.Collections;

public class AnimationHashes : MonoBehaviour {

	// Here we store the hash tags for various strings used in our animators.
	public int dyingState;
	public int locomotionState;
	public int deadBool;
	public int speedFloat;
	public int aimWeightFloat;
	public int angularSpeedFloat;
	
	
	void Awake ()
	{
		dyingState = Animator.StringToHash("Base Layer.TestDie");
		locomotionState = Animator.StringToHash("Base Layer.Locomotion");
		deadBool = Animator.StringToHash("Dead");
		speedFloat = Animator.StringToHash("Speed");
		aimWeightFloat = Animator.StringToHash("AimWeight");
		angularSpeedFloat = Animator.StringToHash("AngularSpeed");
	}
}
