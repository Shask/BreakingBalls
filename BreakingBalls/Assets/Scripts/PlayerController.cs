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

	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;
	private bool backwards = false;
	private Animator PAnim;

	private PlayerItemController playerItemController;
	private PlayerPhysics playerPhysics;

	public int nbRespawn = 0;
	public int playerNo;

	private bool isMoving = true;
	private float delayMoving;

	private string inputHorizontal,inputJump;


	public static float[,] respawnDownPosition = { {-200, 0}, {-10, -10}, {140, -25}, {240, -30}, {1000, -10}};
	public int respawnLevel = 0;



	private GameObject respawnIcon;
	private float respawnIconDelay = 0;
	private Text playerMalusText;

	public bool isWin = false;
	public float winTime;
	private GameControl gc;

	void Start () {
		playerPhysics = GetComponent<PlayerPhysics> ();
		PAnim = GetComponent<Animator> ();
		playerNo = (int)(this.name [this.name.Length - 1]) - 48;

		inputHorizontal = "Horizontal" + playerNo;
		inputJump = "Jump" + playerNo;

		Debug.Log(inputJump + " " + inputHorizontal);
		
		playerItemController = GetComponent<PlayerItemController> (); 
		respawnIcon = GameObject.Find ("RespawnImage" + playerNo);
		respawnIcon.SetActive (false);
		playerMalusText = GameObject.Find ("P" + playerNo + "MalusText").GetComponent<Text> ();		
		gc = GameObject.Find ("Main Camera").GetComponent<GameControl> ();
	}
	
	void Update () {
		//Si l'item a pris fin, on le delete

		if (respawnIcon.activeSelf) {
			respawnIconDelay -= Time.deltaTime;
			if(respawnIconDelay <= 0){
				respawnIcon.SetActive (false);
			}
		}

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

		if (!isWin) {
			targetSpeed = Input.GetAxisRaw (inputHorizontal) * speed;
		} else {
			if(targetSpeed > 0)
				targetSpeed -= Time.deltaTime * 3;
			else
				targetSpeed = 0;
			//PAnim.CrossFade("StickFigureWin", 0.0f);
		}
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
			} else {
				PAnim.CrossFade ("StickFigureRun", 0.0f);
				PAnim.SetBool ("Run", true);

			}
			if (!isWin && Input.GetButtonDown (inputJump)) {
				PAnim.SetTrigger ("Jump");
				amountToMove.y = jumpHeight;	
			}
		}

		amountToMove.x = Mathf.Abs(currentSpeed);
		amountToMove.y -= gravity * Time.deltaTime;
		playerPhysics.Move (amountToMove * Time.deltaTime);

		// Respawn si tombe
		if (transform.position.x >= respawnDownPosition [respawnLevel + 1,0]) {
			respawnLevel++;
		}
		if (transform.position.y < respawnDownPosition[respawnLevel,1]) {
			Respawn (transform.position.x - 5);
		}

		//Fin
		if (isWin == false && transform.position.x >= 730) {
			isWin = true;
			winTime = gc.counter + (nbRespawn*2);
			playerMalusText.text = string.Format ("{0:#0}:{1:00}.{2:00}",
			                                      Mathf.Floor (winTime / 60),
			                                      Mathf.Floor (winTime) % 60,
			                                      Mathf.Floor ((winTime * 100) % 100));
			playerMalusText.color = Color.red;
			playerMalusText.fontSize += 2;
		}
	}

	public void Respawn(float referencePosX)
	{
		if (isWin || !isMoving) {
			return;
		}

		isMoving = false;
		delayMoving = 0.1f;

		nbRespawn ++;

		GameObject[] platforms = GameObject.FindGameObjectsWithTag ("Platform");
		//float cameraPositionX = Camera.main.transform.position.x - 5;
		GameObject respawnPlatform = platforms [0];
		float newDistance = Mathf.Abs(referencePosX - respawnPlatform.transform.position.x - (respawnPlatform.GetComponent<BoxCollider>().center.x * respawnPlatform.transform.localScale.x));

		foreach (GameObject p in platforms) {
			float positionX = p.transform.position.x + p.GetComponent<BoxCollider>().center.x * p.transform.localScale.x;
			float distance = Mathf.Abs (referencePosX - positionX);
			if(distance < newDistance){
				respawnPlatform = p;
				newDistance = distance;
			}
		}

		Vector3 scale = respawnPlatform.transform.localScale;
		Vector3 newPosition = respawnPlatform.GetComponent<BoxCollider> ().center;

		transform.position = new Vector3 (respawnPlatform.transform.position.x + (newPosition.x * scale.x), respawnPlatform.transform.position.y + (newPosition.y + respawnPlatform.GetComponent<BoxCollider> ().size.y + 5) * scale.y, transform.position.z);

		playerMalusText.text = "Malus : +" + (nbRespawn * 2) + "s";
		respawnIcon.SetActive (true);
		respawnIconDelay = 0.8f; 
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
