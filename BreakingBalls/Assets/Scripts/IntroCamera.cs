using UnityEngine;
using System.Collections;

public class IntroCamera : MonoBehaviour {

	private Animation anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animation> ();
		anim.Play ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
