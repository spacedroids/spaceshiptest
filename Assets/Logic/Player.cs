using UnityEngine;
using System.Collections;

public class Player : Ship {

	private Vector3 mousePosition;
	private bool mouseClicked = false;
	private Vector2 target;
	
	protected Vector2[] rotationThrusters = new Vector2[4];

	protected override void Awake() {
		//PID Settings
		angularVelocityController = gameObject.AddComponent<PID> ();
		angularVelocityController.myName = "Angular Velocity";
		angularVelocityController.Kp = 5;
		angularVelocityController.Kd = 7;
		angularVelocityController.multiplier = 100;
		linearDistanceController = gameObject.AddComponent<PID> ();
		linearDistanceController.myName = "Distance Control";
		linearDistanceController.Kp = 5;
		linearDistanceController.Kd = 10f;
		linearDistanceController.multiplier = 7000;
		angularRotationController = gameObject.AddComponent<PID> ();
		angularRotationController.myName = "Rotation Zero";
		angularRotationController.Kp = 3;
		angularRotationController.Kd = 0.3f;
		angularRotationController.multiplier = 1000;
		linearVelocityController = gameObject.AddComponent<PID> ();
		linearVelocityController.myName = "Velocity Control";
		linearVelocityController.Kp = 10;
		linearVelocityController.Kd = 0.3f;
		linearVelocityController.multiplier = 140000;

		spawn = new Vector2 (0f, 0f);
		mass = 100000;
		rigidbody2D.mass = mass;
		//Engine Settings
		maxRotationThrust = 5000;
		maxMainThrust = 30000;
		//Autopilot Settings


		//Thrusters
			//  ^
			//0/\1
			//3||2
		rotationThrusters [0] = new Vector2 (-0.2f, 0.55f);
		rotationThrusters [1] = new Vector2(0.2f, 0.55f);
		rotationThrusters [2] = new Vector2(0.49f, -0.4f);
		rotationThrusters [3] = new Vector2(-0.49f, -0.4f);

		//Initialize behavior state
		currentState = States.Waiting;
		previousState = States.Waiting;
		base.Awake ();
	}

	protected override void Start () {
		base.Start ();
	}

	// Update is called once per frame
	protected override void Update () {

		if (Input.anyKey) {
			//Put the horizontal axis into a vector used for horizontal thrust
			Vector2 vert = new Vector2(Input.GetAxis("Horizontal") * maxMainThrust, 0f);
			base.thrust(vert);
			//Same for vertical axis
			Vector2 horz = new Vector2(0f, Input.GetAxis ("Vertical") * maxMainThrust);
			base.thrust(horz);

			float manualTorque = 200000f;
			if(Input.GetKeyDown("r")) {
				rigidbody2D.AddTorque(-manualTorque);
			} else if(Input.GetKeyDown("e")) {
				rigidbody2D.AddTorque(manualTorque);
			} else if(Input.GetKeyDown ("h")) {
				setState(States.Halting);
			} else if(Input.GetKeyDown ("g")) {
				setState(States.Moving);
			}
		}

		if (Input.GetButtonDown ("Fire1")) {
			mouseClicked = true;
			mousePosition = Input.mousePosition;
			//Translate clicked location from screen coords into world coords and make it relative to ship's position
			target = Camera.main.ScreenToWorldPoint(mousePosition);
		}

	}

	protected override void FixedUpdate() {
		if (mouseClicked) {
			setState(States.Moving);
			rotationAutoPilot = true;
			mouseClicked = false;
		}
		if (getState() == States.Halting) {
			halt();
			setRotationZero();
			if(notMoving()) {
				setState(States.Waiting);
			}
		} else if (getState() == States.Moving) {
			setFacing (target);
			move (target);
		}
		base.FixedUpdate ();
	}
}