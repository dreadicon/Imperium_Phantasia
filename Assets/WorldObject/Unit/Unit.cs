using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FourX;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(MoveAction))]
public class Unit : WorldObject {

	public float turnSpeed = 15f;
	public float moveSpeed = 5.5f;
	public float speedDampTime = 0.1f;

	public float movement = 1f;
	public float attack = 20f;

	Vector2 smoothDeltaPosition = Vector2.zero;
	Vector2 velocity = Vector2.zero;
	
	protected Animator anim;
	protected AnimationHashes hash;

	public bool moving;
	
	protected Vector3 currentDestination;
	protected Vector3 finalDestination;
	protected Quaternion targetRotation;

	public NavMeshAgent navAgent { get; protected set; }

	// Use this for initialization
	protected override void Awake () {
		base.Awake ();
		navAgent = GetComponent<NavMeshAgent> ();
		anim = GetComponent<Animator> ();
		hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<AnimationHashes>();
		navAgent.updatePosition = false;

	}

	protected override void Start () {
		base.Start ();
		//Bindings for actions
		EnableAction (GetComponent<MoveAction>());
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
	}

	protected override void FixedUpdate() {
		base.FixedUpdate ();

		//Only handle movement if it's not at it's destination already.
		if (moving ){//&& ResourceManager.game.TurnTransition()) {
			//Move();
			NavmeshMovement ();
		}

	}

	public override void SetSelection(bool select, LocalPlayer player) {
		// The GUI portion of this will be separated out later into a loadable config.
		if(player == owner) {
			selected = select;
			if(select) {
				InfoBox infoBox = GameController.localPlayer.hud.infoBox;
				var textElements = new List<List<string>> {};
				textElements.Add(new List<string> { 
					"Moves", moveSpeed.ToString() 
				});
				textElements.Add(new List<string> {
					"Attack", attack.ToString()
				});

				infoBox.AppendTextElements(textElements);
				infoBox.SetTitle(objectName);
			}
		} else {
			if (select) {

			}
		}

	}

	public override void MouseAction(GameObject hitObject, Vector3 hitPoint) {
		base.MouseAction(hitObject, hitPoint);
		if (owner && owner.localPlayer && selected) {
			if (hitObject.name == "Terrain" && hitPoint != ResourceManager.InvalidPosition) {
				MoveTo(hitPoint);
			} 
		}
	}

	protected void MoveTo(Vector3 target) {
		float y = target.y + transform.position.y;
		GetComponent<MoveAction> ().SendCommand (new Vector3(target.x, y, target.z));
	}

	private void DrawPath() {

	}

	protected void NavmeshMovement () {
		Vector3 worldDeltaPosition = navAgent.nextPosition - transform.position;
		
		// Map 'worldDeltaPosition' to local space
		float dx = Vector3.Dot (transform.right, worldDeltaPosition);
		float dy = Vector3.Dot (transform.forward, worldDeltaPosition);
		Vector2 deltaPosition = new Vector2 (dx, dy);
		
		// Low-pass filter the deltaMove
		float smooth = Mathf.Min(1.0f, Time.deltaTime/0.15f);
		smoothDeltaPosition = Vector2.Lerp (smoothDeltaPosition, deltaPosition, smooth);
		
		// Update velocity if time advances
		if (Time.deltaTime > 1e-5f)
			velocity = smoothDeltaPosition / Time.deltaTime;
		
		bool move = velocity.magnitude > 0.5f && navAgent.remainingDistance > navAgent.radius;
		
		// Update animation parameters
		anim.SetBool("move", move);
		anim.SetFloat ("velx", velocity.x);
		anim.SetFloat ("vely", velocity.y);
		
		//GetComponent<LookAt>().lookAtTargetPosition = navAgent.steeringTarget + transform.forward;

		//Check if done moving
		if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
			if (!navAgent.hasPath || navAgent.velocity.sqrMagnitude == 0f)
				moving = false;
		
	}
	
	void OnAnimatorMove ()
	{
		// Update position to agent position
		transform.position = navAgent.nextPosition;
	}
}
