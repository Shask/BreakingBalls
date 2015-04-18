using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerController : MonoBehaviour {
	
	// Player Handling
	public float gravity = 20;
	public float speed = 8;
	public float acceleration = 30;
	public float jumpHeight = 12;

	public int playerNo = 1;
	
	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;
	private bool backwards = false;
	private Animator PAnim;
	
	private PlayerPhysics playerPhysics;

	public int nbRespawn = 0;

	private bool isMoving = true;
	private float delayMoving;

	void Start () {
		playerPhysics = GetComponent<PlayerPhysics>();
		PAnim = GetComponent<Animator> ();
	}
	
	void Update () {
		if (!isMoving) {
			delayMoving -= Time.deltaTime;
			if (delayMoving <= 0)
				isMoving = true;
			else
				return;
		}
		if (playerPhysics.movementStopped) {
			targetSpeed=0;
			currentSpeed=0;
		}
		targetSpeed = Input.GetAxisRaw ("Horizontal") * speed;
		currentSpeed = IncrementTowards (currentSpeed, targetSpeed, acceleration);



		if (currentSpeed < 0) {
			Quaternion therotation = transform.localRotation;
			therotation.y = 180;
			transform.localRotation = therotation;
		}
		if (currentSpeed > 0) {
			Quaternion therotation = transform.localRotation;
			therotation.y = 0;
			transform.localRotation= therotation;
		}


		if (playerPhysics.grounded) {
			amountToMove.y = 0;
			if (currentSpeed == 0) {
				PAnim.CrossFade ("StickFigureIddle", 0.0f);
				PAnim.SetBool ("Run", false);
			} else 
			{
				PAnim.CrossFade ("StickFigureRun", 0.0f);
				PAnim.SetBool ("Run", true);

			}
			if (Input.GetButtonDown ("Jump")) {
				PAnim.SetTrigger ("Jump");
				amountToMove.y = jumpHeight;	
			}
		}

			


		
		amountToMove.x = Mathf.Abs(currentSpeed);
		amountToMove.y -= gravity * Time.deltaTime;
		playerPhysics.Move (amountToMove * Time.deltaTime);

		if (transform.position.y < -15) {
			Respawn ();
		}
		
	}

	public void Respawn()
	{
		if (!isMoving) {
			return;
		}

		isMoving = false;
		delayMoving = 0.1f;

		nbRespawn ++;

		GameObject[] platforms = GameObject.FindGameObjectsWithTag ("Platform");
		float cameraPositionX = Camera.main.transform.position.x;
		GameObject respawnPlatform = platforms [0];
		float newDistance = Mathf.Abs(cameraPositionX - (respawnPlatform.GetComponent<BoxCollider>().center.x * respawnPlatform.transform.localScale.x));

		foreach (GameObject p in platforms) {
			float positionX = p.GetComponent<BoxCollider>().center.x * p.transform.localScale.x;
			float distance = Mathf.Abs (cameraPositionX - positionX);
			if(distance < newDistance){
				respawnPlatform = p;
				newDistance = distance;
			}
		}
		Debug.Log (playerNo + " - " + respawnPlatform.name);
		Vector3 scale = respawnPlatform.transform.localScale;
		Vector3 newPosition = respawnPlatform.GetComponent<BoxCollider> ().center;
		transform.position = new Vector3 (newPosition.x * scale.x, (newPosition.y + respawnPlatform.GetComponent<BoxCollider> ().size.y + 5) * scale.y, newPosition.z * scale.y);

		Text playerMalusText = GameObject.Find("P"+playerNo+"MalusText").GetComponent<Text>();
		playerMalusText.text = "Malus : +" + (nbRespawn * 2) + "s";
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
