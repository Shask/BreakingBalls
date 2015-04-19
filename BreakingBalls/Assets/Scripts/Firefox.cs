using UnityEngine;
using System.Collections;

public class Firefox : MonoBehaviour {

	private float speed = 0.15f;
	private Vector3 translateur;

	private float _nextShotInSecond = 0.1f;

	public FireBolt bolt;
	private FireBolt clone;
	private int nbBolt = 0;
	private int shoot = 0;

	private GameObject cible;

	private Vector3 maPositionDebut;
	private FireBolt projectile;
	private Transform destination;

	// Use this for initialization

	void Start () {
		maPositionDebut = transform.position;
		translateur.Set(0f,-3f,0f);
	}
	
	// Update is called once per frame
	void Update () {
		if (this.transform.position.y > 0.75f + maPositionDebut.y) {
			translateur.Set(0f,-10f,0f);
		}
		if (this.transform.position.y < maPositionDebut.y - 0.5f) {
			translateur.Set(0f,+10f,0f);
		}
		transform.Translate (translateur * speed * Time.deltaTime);

		if ((_nextShotInSecond -= Time.deltaTime) > 0)
			return;

		if (shoot>0) {
			//cible le 1er joueur rentré dans la boxCollider
			NewBolt(cible.transform.position);
			//horizontal
			//NewBolt ();

		}
		_nextShotInSecond = 2.25f;
	}

	void OnTriggerEnter(Collider other) {
		cible = other.gameObject;
		shoot ++;
		//Destroy (this.gameObject);
	}

	void OnTriggerExit(){
		shoot--;
	}


	void NewBolt(Vector3 target){
		//clone = new FireBolt();
		clone = Instantiate(bolt, transform.position, transform.rotation) as FireBolt;
		clone.gameObject.SetActive (true);
		clone.name = "FireBolt " + (++nbBolt);
		clone.setTargetPosition(target);
		clone.setSpeed (3f);
		clone.StartTranslation();
	}

	void NewBolt(){
		//clone = new FireBolt();
		clone = Instantiate(bolt, transform.position, transform.rotation) as FireBolt;
		clone.name = "FireBolt " + (++nbBolt);
		clone.setTargetPosition();
		clone.setSpeed (3f);
		clone.StartTranslation();
	}

}
