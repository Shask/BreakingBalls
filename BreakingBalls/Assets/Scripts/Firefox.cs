using UnityEngine;
using System.Collections;

public class Firefox : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Spawn ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Spawn(){
		
	}

	void OnTriggerEnter(Collider other) {
		Destroy (this);
	}
}
