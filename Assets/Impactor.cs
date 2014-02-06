using UnityEngine;
using System.Collections;

public class Impactor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Vector2 force = new Vector2 (-40f, 0.2f);
		rigidbody2D.AddForce (force);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
