using UnityEngine;
using System.Collections;

public class FireBolt : MonoBehaviour {

	private Vector3 target;
	private float speed = 10f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (new Vector3 (0, 0, 2));
		transform.Translate (target * speed);
	}

	void OnTriggerEnter(Collider other) {
		Destroy (this);
	}

	public void setTargetPosition(Vector3 pos){
		target = pos;
	}

}
