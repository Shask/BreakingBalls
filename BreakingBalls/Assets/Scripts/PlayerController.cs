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

		targetSpeed = Input.GetAxisRaw ("Horizontal") * speed;
		currentSpeed = IncrementTowards (currentSpeed, targetSpeed, acceleration);
		if (currentSpeed > 0 && playerPhysics.grounded) {
			PAnim.CrossFade ("StickFigureRun", 0.0f);
			PAnim.SetBool ("Run", true);
			PAnim.SetBool ("RunBack", false);
		}
		if (currentSpeed < 0 && playerPhysics.grounded) {
			PAnim.CrossFade ("StickFigureRunBack", 0.0f);
			PAnim.SetBool ("Run", true);
			PAnim.SetBool ("RunBack", true);
		} 
		if (currentSpeed == 0 && playerPhysics.grounded) {
			PAnim.CrossFade ("StickFigureIddle", 0.0f);
			PAnim.SetBool ("Run", false);
			PAnim.SetBool ("RunBack", false);
		} 

		if (playerPhysics.grounded) {
			amountToMove.y = 0;

			// Jump
			if (Input.GetButtonDown ("Jump")) {
				PAnim.SetTrigger ("Jump");
				amountToMove.y = jumpHeight;	
			}
		}
		
		amountToMove.x = currentSpeed;
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
		float newDistance = Mathf.Abs(cameraPositionX - respawnPlatform.transform.position.x - (respawnPlatform.GetComponent<BoxCollider>().center.x * respawnPlatform.transform.localScale.x));

		foreach (GameObject p in platforms) {
			float positionX = p.transform.position.x + p.GetComponent<BoxCollider>().center.x * p.transform.localScale.x;
			float distance = Mathf.Abs (cameraPositionX - positionX);
			if(distance < newDistance){
				respawnPlatform = p;
				newDistance = distance;
			}
		}
		Debug.Log (playerNo + " - " + respawnPlatform.name);

		Vector3 scale = respawnPlatform.transform.localScale;
		Vector3 newPosition = respawnPlatform.GetComponent<BoxCollider> ().center;

		Debug.Log ("positionX : " + newPosition.x * scale.x);
		Debug.Log ("positionBoxColliderX : " + respawnPlatform.GetComponent<BoxCollider> ().center.x);
		Debug.Log ("positionPlatformX : " + respawnPlatform.transform.position.x);
		Debug.Log ("positionPlatformLocalX : " + respawnPlatform.transform.localPosition.x);

		transform.position = new Vector3 (respawnPlatform.transform.position.x + (newPosition.x * scale.x), respawnPlatform.transform.position.y + (newPosition.y + respawnPlatform.GetComponent<BoxCollider> ().size.y + 5) * scale.y, transform.position.z);

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
