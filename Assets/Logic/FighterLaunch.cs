using UnityEngine;
using System.Collections;

public class FighterLaunch : MonoBehaviour {

	public Transform fighterPrefab;

	private Transform fighter;
	private Transform[] squad = new Transform[5];

	// Use this for initialization
	void Start () {
		float x = 1;
		float y = 1;
		for(int i=0;i<squad.Length;i++){
			squad[i] = Instantiate (fighterPrefab) as Transform;
			x += 0.2f;
			y += 0.2f;
			Vector2 spawn = new Vector2 (x, y);
			squad[i].transform.position = spawn;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
