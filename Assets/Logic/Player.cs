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
		linearVelocityController = gameObject.AddComponent<PID> ();
		linearVelocityController.myName = "Linear Velocity";
		linearVelocityController.Kp = 5;
		linearVelocityController.Kd = 10f;
		rotationZeroController = gameObject.AddComponent<PID> ();
		rotationZeroController.myName = "Rotation Zero";
		rotationZeroController.Kp = 3;
		rotationZeroController.Kd = 0.3f;

		spawn = new Vector2 (0f, 0f);
		mass = 100000;
		rigidbody2D.mass = mass;
		//Engine Settings
		maxRotationThrust = 5000;
		maxMainThrust = 30000;
		//Autopilot Settings
		rotationAngleCoeff = 100;
		rotationVelocityCoeff = 1000;
		linearVelocityCoeff = 7000;

		//Thrusters
			//  ^
			//0/\1
			//3||2
		rotationThrusters [0] = new Vector2 (-0.2f, 0.55f);
		rotationThrusters [1] = new Vector2(0.2f, 0.55f);
		rotationThrusters [2] = new Vector2(0.49f, -0.4f);
		rotationThrusters [3] = new Vector2(-0.49f, -0.4f);


		base.Awake ();
	}

	protected override void Start () {
		base.Start ();
	}


	private bool allStop;
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
				allStop = true;
			} else if(Input.GetKeyDown ("g")) {
				allStop = false;
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
			rotationAutoPilot = true;
			mouseClicked = false;
		}
		if (allStop) {
			halt();
			setRotationZero();
		} else if (rotationAutoPilot) {
			rotateTowards (target);
			move (target);
		}
		base.FixedUpdate ();
	}
}