using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameControl : MonoBehaviour {

	public Transform[] players;
	public float cameraInitialSize = 5;
	private float hwRatio = 2; // Ratio between height and width screen size
	public float cameraMaxSize = 13;
	public float margin = 3;

	private float counter = 0;
	private Text counterText;


	// Use this for initialization
	void Start () {
		counterText = GameObject.Find("CounterText").GetComponent<Text>();
	}

	
	// Update is called once per frame
	void Update () {
		ResizeCamera ();
		counter += Time.deltaTime;
		counterText.text = string.Format("{0:#0}:{1:00}.{2:00}",
		                                 Mathf.Floor(counter / 60),
		                                 Mathf.Floor(counter) % 60,
		                                 Mathf.Floor((counter*100) % 100));
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

		float newPositionX = Mathf.Lerp (cameraX, positionXToReach, Time.deltaTime * 10);
		float newPositionY = Mathf.Lerp (cameraY, positionYToReach, Time.deltaTime * 10);

		transform.position = new Vector3 (newPositionX, newPositionY, transform.position.z);

		/* Agrandit la caméra si un joueur sort du cadre */
		float newSize = cameraSize;

		foreach (Transform p in players) {
			if(p.position.x < (cameraX - cameraLength + (margin*hwRatio)) || p.position.x > (cameraX + cameraLength - (margin*hwRatio)))
			{
				float size = (Mathf.Abs (cameraX - p.position.x) / hwRatio);
				if(size > cameraMaxSize)
				{
					respawnLastPlayer();
				}
				else if(size + margin > newSize)
					newSize = size + margin;
			} else if(p.position.y < (cameraY - cameraSize + margin) || p.position.y > (cameraY + cameraSize - margin))
			{
				float size = Mathf.Abs (cameraY - p.position.y);
				if(size > cameraMaxSize)
				{
					respawnLastPlayer();
				}
				else if(size + margin > newSize)
					newSize = size + margin;
			}
		}
		if(cameraSize != newSize)
			Camera.main.orthographicSize = Mathf.Lerp (cameraSize, newSize, Time.deltaTime * 2);
		else{
			/* Réduit le cadre si possible */
			newSize = 0;
			
			foreach (Transform p in players) {
				float size = Mathf.Abs (p.position.x - cameraX) / hwRatio + margin + 1;
				if(size > newSize)
					newSize = size;
				size = Mathf.Abs (p.position.y - cameraY) / hwRatio + margin + 1;
				if(size > newSize)
					newSize = size;
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
			float distance2 = (-p.position.y + transform.position.y);
			if(distance1 > distance)
			{
				distance = distance1;
				lastPlayer = p;
			}
			if(distance2 > distance)
			{
				distance = distance2;
				lastPlayer = p;
			}
		}
		Debug.Log (lastPlayer.name);
		PlayerController pc = lastPlayer.GetComponent<PlayerController> ();
		pc.Respawn();
	}

	void GameOver()
	{
		Application.LoadLevel(Application.loadedLevel);
	}
}
