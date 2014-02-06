using UnityEngine;
using System.Collections;

public class Fighter : Ship {

	public FighterEngine engine;
	
	protected override void Start() {
		engine = ScriptableObject.CreateInstance<FighterEngine> ();
		engine.Init (transform.position, transform.rotation, transform);
		engine.Start ();
		}

	

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
		mass = 20;
		rigidbody2D.mass = mass;
		//Engine Settings
		maxRotationThrust = 1;
		maxMainThrust = 6;
		//Autopilot Settings
		rotationAngleCoeff = 0.02f;
		rotationVelocityCoeff = 0.2f;
		linearVelocityCoeff = 1.4f;

		base.Awake ();
	}

	private bool mouseClicked = false;
	private Vector3 mousePosition;
	private Vector2 target;

	protected override void Update ()
	{
		if (Input.GetButtonDown ("Fire2")) {
			mouseClicked = true;
			mousePosition = Input.mousePosition;
			//Translate clicked location from screen coords into world coords and make it relative to ship's position
			target = Camera.main.ScreenToWorldPoint(mousePosition);
		}

		engine.animate ();

	}

	protected override void FixedUpdate() {

		if (mouseClicked) {
			rotationAutoPilot = true;
			mouseClicked = false;
		}
		if (rotationAutoPilot) {
			rotateTowards (target);
			move (target);
		} else {
			setRotationZero();
		}
		base.FixedUpdate ();
	}

}
