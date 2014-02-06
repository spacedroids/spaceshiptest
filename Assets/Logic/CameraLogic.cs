using UnityEngine;
using System.Collections;

public class CameraLogic : MonoBehaviour {

	public Transform target;
	public float distance;
	
	// Use this for initialization
	void Start () {
	
	}

	public void Awake()	
	{	
		camera.orthographicSize = (Screen.height / 100f / 2.0f); // 100f is the PixelPerUnit that you have set on your sprite. Default is 100.	
	}

	// Update is called once per frame
	void Update(){

		Vector3 newPosition = target.transform.position;
		newPosition.z -= distance;
		transform.position = newPosition;

	}}