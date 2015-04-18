using UnityEngine;
using System.Collections;
[RequireComponent (typeof (PlayerController2))]
public class PlayerInput2 : MonoBehaviour {
	

	float moveSpeed = 6;
	float gravity = -20;
	Vector3 velocity;
	
	PlayerController2 controller;

	void Start () {
		controller = GetComponent<PlayerController2>();
	}
	

	void Update () {
		
		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		
		velocity.x = input.x * moveSpeed;
		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime);
	}
}
