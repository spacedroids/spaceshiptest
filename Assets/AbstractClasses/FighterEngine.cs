using UnityEngine;
using System.Collections;

public class FighterEngine : Engine {

	public GameObject particlePrefab;
	public GameObject particle;

	public Vector2 position;
	public Quaternion rotation;
	public Transform parent;
	
	public void Init(Vector2 position, Quaternion rotation, Transform parent) {
		this.position = position;
		this.rotation = rotation;
		this.parent = parent;
	}


	public override void Start() {
		particlePrefab = (GameObject)Resources.Load("FighterEngineParticles",typeof(GameObject));
		particle = (GameObject)Instantiate (particlePrefab, position, rotation);
		particle.transform.parent = parent;
	}

	public void animate() {
		if (particle.particleSystem.isStopped) {
			particle.particleSystem.Play ();
		}
	}


	void Update () {
	
	}
}
