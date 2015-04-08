using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerController : MonoBehaviour {
	
	// Player Handling
	public float gravity = 20;
	public float speed = 8;
	public float acceleration = 30;
	public float jumpHeight = 12;
	
	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;

	private Animator PAnim;
	
	private PlayerPhysics playerPhysics;
	

	void Start () {
		playerPhysics = GetComponent<PlayerPhysics>();
		PAnim = GetComponent<Animator> ();
	}
	
	void Update () {

		if (!playerPhysics.grounded) 
			PAnim.SetBool ("Jump", true);

		targetSpeed = Input.GetAxisRaw("Horizontal") * speed;
		currentSpeed = IncrementTowards(currentSpeed, targetSpeed,acceleration);
		if (currentSpeed > 0 && playerPhysics.grounded) {
			PAnim.CrossFade("StickFigureRun",0.0f);
			PAnim.SetBool ("Run", true);
			PAnim.SetBool ("RunBack", false);
		}
		if (currentSpeed <0 && playerPhysics.grounded) {
			PAnim.CrossFade("StickFigureRunBack",0.0f);
			PAnim.SetBool ("Run", true);
			PAnim.SetBool ("RunBack", true);
		} 
		if (currentSpeed ==0 && playerPhysics.grounded) {
			PAnim.CrossFade("StickFigureIddle",0.0f);
			PAnim.SetBool ("Run", false);
			PAnim.SetBool ("RunBack", false);
		} 

		if (playerPhysics.grounded) {
			PAnim.SetBool ("Jump", false);
			amountToMove.y = 0;

			// Jump
			if (Input.GetButtonDown("Jump")) {

				amountToMove.y = jumpHeight;	
			}
		}
		
		amountToMove.x = currentSpeed;
		amountToMove.y -= gravity * Time.deltaTime;
		playerPhysics.Move(amountToMove * Time.deltaTime);
	}
	
	// Increase n towards target by speed
	private float IncrementTowards(float n, float target, float a) {
		if (n == target) {
			return n;	
		}
		else {
			float dir = Mathf.Sign(target - n); // must n be increased or decreased to get closer to target
			n += a * Time.deltaTime * dir;
			return (dir == Mathf.Sign(target-n))? n: target; // if n has now passed target then return target, otherwise return n
		}
	}
}
