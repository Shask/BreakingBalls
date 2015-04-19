using UnityEngine;
using System.Collections;

public class Firefox : MonoBehaviour {

	private float speed = 0.15f;
	private Vector3 translateur;

<<<<<<< HEAD
	private float _nextShotInSecond = 0.1f;

	public FireBolt bolt;
	private FireBolt clone;
	private int nbBolt = 0;
	private int shoot = 0;
	private Vector3 cible = new Vector3(-1f,0f,-0f);

	private Vector3 maPositionDebut;
=======
	private float _nextShotInSecond;
	private FireBolt projectile;
	private Transform destination;

>>>>>>> 9763786b310a17eab3eded18d9eec071ce76e853
	// Use this for initialization

	void Start () {
		maPositionDebut = transform.position;
		translateur.Set(0f,-3f,0f);

	}
	
	// Update is called once per frame
	void Update () {
		if (this.transform.position.y > 0.5f + maPositionDebut.y) {
			translateur.Set(0f,-10f,0f);
		}
		if (this.transform.position.y < maPositionDebut.y - 0.5f) {
			translateur.Set(0f,+10f,0f);
		}
		transform.Translate (translateur * speed * Time.deltaTime);

		if ((_nextShotInSecond -= Time.deltaTime) > 0)
			return;

		if (shoot>0) {
			NewBolt (cible);
		}
		_nextShotInSecond = 2.25f;



	
	}

	void OnTriggerEnter(Collider other) {
		print (other.name.ToString());
		cible = other.gameObject.transform.position;
		shoot ++;
		/*
		if(other.GetType().ToString().CompareTo("BoxCollider")==0){
			print ("CARRE");
		}
		if(other.GetType().ToString().CompareTo("SphereCollider")==0){
			print ("SPHERE");
			print ("Firefox touché -> détruit");
			Destroy (this.gameObject);
		}*/
	}

	void OnTriggerExit(){
		shoot--;
	}


	void NewBolt(Vector3 target){

		clone = new FireBolt();
		clone = Instantiate(bolt, transform.position, transform.rotation) as FireBolt;
		clone.name = "FireBolt " + (++nbBolt);
		clone.setTargetPosition(new Vector3(-1f,0f,-0f));
		clone.setSpeed (3f);
		clone.StartTranslation();




	}

}
