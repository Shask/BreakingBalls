﻿using UnityEngine;
using System.Collections;

public class Piege : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other) {
		other.GetComponent<PlayerController> ().Respawn (other.transform.position.x - 10);
	}	                                 
}
