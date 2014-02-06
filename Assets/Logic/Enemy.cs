using UnityEngine;
using System.Collections;

public class Enemy : Ship {

	// Use this for initialization
	protected override void Start () {
//		This overrides a value on a parent class
//		spawn = new Vector2 (0f, 2f);
		base.Start ();
	}
	
	// Update is called once per frame
	protected override void Update () {
	
	}
}
