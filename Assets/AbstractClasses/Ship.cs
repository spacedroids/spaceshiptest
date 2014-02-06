using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {
	
	protected PID angularVelocityController;
	protected PID rotationZeroController;
	protected PID linearVelocityController;
	
	public Vector2 spawn;
	//Engine Specs
	public float rotationAngleCoeff = 0.02f;
	public float rotationVelocityCoeff = 0.02f;
	public float maxRotationThrust = 1f;
	public float linearVelocityCoeff = 0.02f;
	public float maxMainThrust = 1f;
	public float mass = 1f;
	//Autopilot
	 //Rotation
	protected bool rotationAutoPilot; //is it engaged?
	protected float acceptableRotationErr = 5f;
	protected float acceptableRotationVel = 1f;
	 //Movement
	protected bool moveAutoPilot;

	protected virtual void Start () {}
	protected virtual void Awake() {}
	protected virtual void Update () {}

	protected void thrust(Vector2 force) {
		rigidbody2D.AddForce (force);
	}
	
	protected void move(Vector2 target) {
		target = target - (Vector2)transform.position;
		float distance = target.magnitude;
		float correctionForDistance = linearVelocityController.GetOutput(distance);
		float force = correctionForDistance * linearVelocityCoeff;
		force = Mathf.Clamp (force, -maxMainThrust, maxMainThrust);
		Vector2 direction = target.normalized * force;
		Debug.Log ("Moving power: " + force + " dir: " + target.normalized);
		rigidbody2D.AddForce (direction);
	}

	//Rotate towards a fixed point in space, retarget if ship moves around it
	protected void rotateTowards(Vector2 target) {
		target = target - (Vector2)transform.position;
		setFacing (target);
	}
	
	//Rotate towards a facing relative to the ship, don't retarget based on ship movement
	protected void setFacing(Vector2 target) {
		Vector2 facing = transform.up;
		
		//How big is the angle between the ship facing and the target?
		float error = Vector3.Angle (facing, target);
		
		//Use cross product to determine which direction to rotate
		Vector3 cross = Vector3.Cross(facing, target);
		float direction = 1;
		//cross product's z component decides direction of rotation
		if(cross.z < 0) {
			direction *= -1;
		}
		
		rotate (error * direction, rotationAngleCoeff, angularVelocityController);
		if (rotationDone (error)) {
			rotationAutoPilot = false;
		}
	}

	protected bool rotationDone (float distance) {
		if (distance < acceptableRotationErr && Mathf.Abs(rigidbody2D.angularVelocity) < acceptableRotationVel) {
			Debug.Log ("Target rotation reached! Within: " + distance + " at spd: " + rigidbody2D.angularVelocity);
			return true;
		}
		return false;
	}

	protected void setRotationZero() {
		Debug.Log ("zeroing: " + rigidbody2D.angularVelocity);
		float angularVelocityError = -rigidbody2D.angularVelocity;
		rotate (angularVelocityError, rotationVelocityCoeff, rotationZeroController);
	}

	protected void rotate(float error, float power, PID controller) {
		float torqueCorrectionForAngularVelocity = controller.GetOutput(error);
		float torque = power * torqueCorrectionForAngularVelocity;
		torque = Mathf.Clamp (torque, -maxRotationThrust, maxRotationThrust);
		Debug.Log ("Error: " + error + " force: " + torque);
		rigidbody2D.AddTorque(torque);
	}
	
	protected virtual void FixedUpdate() {}
}
