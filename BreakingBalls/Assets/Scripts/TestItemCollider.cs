using UnityEngine;
using System.Collections;

public class TestItemCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void  OnCollisionEnter(Collision other)
	{
		Debug.Log ("couou2");
	}
	void  OnTriggerEnter (Collider other)
			{
			Debug.Log ("Coucou");
		}
}
