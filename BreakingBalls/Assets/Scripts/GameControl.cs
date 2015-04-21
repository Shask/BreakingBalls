using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {

	public GameObject[] playerChoices;
	public Sprite[] pIconChoices;

	private GameObject[] goPlayers;
	private Transform[] players;
	private PlayerController[] pControllers;
	public float cameraInitialSize = 5;
	private float hwRatio = 2; // Ratio between height and width screen size
	public float cameraMaxSize = 13;
	public float margin = 3;
	public int nbSecLostByRespawn = 2;
	public float cameraRightDelta = 5;

	public float counter = 0;
	private Text counterText;

	public bool onPause;
	private bool onBeginning;
	private float beginTimer;

	private GameObject pausePanel;
	private Button[] pauseButtons;
	private int buttonChoice;
	private Color baseColor;
	private bool onClick;

	// Use this for initialization
	void Start () {
		counterText = GameObject.Find("CounterText").GetComponent<Text>();
		onPause = false;
		onBeginning = true;
		beginTimer = Time.realtimeSinceStartup;
		Time.timeScale = 0f;

		pausePanel = GameObject.Find ("PausePanel");
		pauseButtons = new Button[3];
		pauseButtons[0] = GameObject.Find ("ButtonMenu").GetComponent<Button> ();
		pauseButtons[1] = GameObject.Find ("ButtonRestart").GetComponent<Button> ();
		pauseButtons[2] = GameObject.Find ("ButtonQuit").GetComponent<Button> ();
		baseColor = pauseButtons [0].image.color;
		pausePanel.SetActive (false);	
		onClick = false;

		players = new Transform[3];
		goPlayers = new GameObject[3];
		pControllers = new PlayerController[players.Length];
		Image[] playerIcons = new Image[3];
		for(int i = 0; i < players.Length; i++) {
			// Choix du préfab (couleur + fat/normal/rapide):
			goPlayers[i] = Instantiate(playerChoices[i+(ApplicationModel.playerChoice[i]*3)], new Vector3(-165,19,10), Quaternion.identity) as GameObject;
			goPlayers[i].name = "Player"+(i+1);
			playerIcons[i] = GameObject.Find ("P"+(i+1)+"Icon").GetComponent<Image>();
			playerIcons[i].sprite = pIconChoices[ApplicationModel.playerChoice[i]+(i*3)];

			players[i] = goPlayers[i].GetComponent<Transform>();
			pControllers[i] = players[i].GetComponent<PlayerController>();
		}
		//pControllers [1].isWin = true; // TEST
		//pControllers [2].isWin = true; // TEST
	}
	
	// Update is called once per frame
	void Update () {
		if (!onPause && !onBeginning) {
			ResizeCamera ();
			updateCounter ();
		} else if (onBeginning) {
			float timer = Time.realtimeSinceStartup;
			if(timer - beginTimer >= 3){
				onBeginning = false;
				Time.timeScale = 1.0f;
			}
		}else{
			if(Input.GetAxisRaw ("Vertical1") > 0 && !onClick)
			{
				pauseButtons[buttonChoice].image.color = Color.gray;
				buttonChoice = (buttonChoice + 2)%3;
				pauseButtons[buttonChoice].image.color = baseColor;
				onClick = true;
			}else if(Input.GetAxisRaw ("Vertical1") < 0 && !onClick)
			{
				pauseButtons[buttonChoice].image.color = Color.gray;
				buttonChoice = (buttonChoice + 1)%3;
				pauseButtons[buttonChoice].image.color = baseColor;
				onClick = true;
			}else if(Input.GetAxisRaw ("Vertical1") == 0)
			{
				onClick = false;
			}
			if(Input.GetButtonDown("Jump1"))
			{
				switch(buttonChoice)
				{
				case 0:
					ReturnToMenu();
					break;
				case 1:
					Restart ();
					break;
				case 2:
					Quit ();
					break;
				}
			}
		}

		if (Input.GetButtonDown ("Cancel")) {
			setPause ();
		}

		if (onePlayerAsWon ()) {
			cameraMaxSize = 25;
		}
		if (isGameFinished ()) {
			ApplicationModel.playerTimes = new Dictionary<int, float>();
			for(int i = 0; i < pControllers.Length; i++){
				ApplicationModel.playerTimes.Add (i+1, pControllers[i].winTime);
			}
			Application.LoadLevel ("SceneEnd");
		}
	}

	public bool onePlayerAsWon(){
		bool returnvalue = false;
		foreach (PlayerController pc in pControllers) {
			if (pc.isWin)
				returnvalue = true;
		}
		return returnvalue;
	}

	public bool isGameFinished()
	{
		bool returnvalue = true;
		foreach (PlayerController pc in pControllers) {
			if (!pc.isWin)
				returnvalue = false;
		}
		return returnvalue;
	}

	public void setPause()
	{
		onPause = !onPause;
		if (onPause) {
			pausePanel.SetActive (true);
			Time.timeScale = 0f;
			buttonChoice = 0;
			pauseButtons[0].image.color = baseColor;
			pauseButtons[1].image.color = Color.gray;
			pauseButtons[2].image.color = Color.gray;
		} else {
			pausePanel.SetActive (false);
			Time.timeScale = 1.0f;
		}
	}

	public void Restart(){
		setPause ();
		Application.LoadLevel(Application.loadedLevel);
	}
	
	public void Quit()
	{
		Application.Quit ();
	}

	public void ReturnToMenu()
	{
		setPause ();
		Application.LoadLevel ("SceneMenu");
	}
	
	void updateCounter()
	{
		counter += Time.deltaTime;
		counterText.text = string.Format ("{0:#0}:{1:00}.{2:00}",
		                                  Mathf.Floor (counter / 60),
		                                  Mathf.Floor (counter) % 60,
		                                  Mathf.Floor ((counter * 100) % 100));
	}

	void ResizeCamera(){
		float cameraSize = Camera.main.orthographicSize;
		float cameraLength = cameraSize * hwRatio;
		float cameraX = transform.position.x;
		float cameraY = transform.position.y;

		/* Centre la camera à la moyenne de la position des joueurs */
		float positionXToReach = 0;
		float positionYToReach = 0;

		foreach(Transform p in players){
			positionXToReach += p.position.x;
			positionYToReach += p.position.y;
		}
		positionXToReach /= players.Length;
		positionYToReach /= players.Length;

		float newPositionX = Mathf.Lerp (cameraX, positionXToReach + cameraRightDelta, Time.deltaTime * 10); // cameraDelta : écran un peu plus à droite que la moyenne pour une meilleure visibilité
		float newPositionY = Mathf.Lerp (cameraY, positionYToReach, Time.deltaTime * 10);

		transform.position = new Vector3 (newPositionX, newPositionY, transform.position.z);

		/* Agrandit la caméra si un joueur sort du cadre */
		float newSize = cameraSize;

		foreach (Transform p in players) {
			if(p.position.x < (cameraX - cameraLength + (margin*hwRatio)) || p.position.x > (cameraX + cameraLength - (margin*hwRatio)))
			{
				float size = (Mathf.Abs (cameraX - p.position.x) / hwRatio);
				if(size >= (cameraMaxSize-margin))
				{
					respawnLastPlayer();
				}
				else if(size + margin > newSize)
					newSize = size + margin;
			} else if(p.position.y < (cameraY - cameraSize + margin) || p.position.y > (cameraY + cameraSize - margin))
			{
				float size = Mathf.Abs (cameraY - p.position.y);
				if(size < (cameraMaxSize-margin) && size + margin > newSize)
					newSize = size + margin;
			}
		}
		if (cameraSize != newSize) {
			Camera.main.orthographicSize = Mathf.Lerp (cameraSize, newSize, Time.deltaTime * 2);
		}
		else{
			/* Réduit le cadre si possible */
			newSize = 0;
			
			foreach (Transform p in players) {
				float size = Mathf.Abs (p.position.x - cameraX) / hwRatio + margin;
				if(size > newSize)
					newSize = size;
				size = Mathf.Abs (p.position.y - cameraY) + margin;
				if(size > newSize)
					newSize = size;
			}
			if(newSize >= cameraSize){
				newSize = cameraSize;
			}
			if(newSize < cameraInitialSize)
				newSize = cameraInitialSize;
			Camera.main.orthographicSize = Mathf.Lerp (cameraSize, newSize, Time.deltaTime * 2);
		}
	}

	void respawnLastPlayer()
	{
		Transform lastPlayer = players [0];
		float distance = 0;
		foreach (Transform p in players) {
			float distance1 = (-p.position.x + transform.position.x) / hwRatio;
			//float distance2 = (-p.position.y + transform.position.y);
			if(distance1 > distance)
			{
				distance = distance1;
				lastPlayer = p;
			}
			/*if(distance2 > distance)
			{
				distance = distance2;
				lastPlayer = p;
			}*/
		}
		lastPlayer.GetComponent<PlayerController> ().Respawn(this.transform.position.x - 5);
	}

	void GameOver()
	{
		Application.LoadLevel(Application.loadedLevel);
	}
}
