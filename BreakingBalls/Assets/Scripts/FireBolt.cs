using UnityEngine;
using System.Collections;

public class FireBolt : MonoBehaviour {

	private Vector3 target;//= new Vector3(-10f,0f,0f);
	private float speed = 2f;

	//public Transform _destination;
	private bool move=false;
	private Vector3 maposition;

	//public Renderer r;

	// Use this for initialization
	void Start () {
		//r.enabled = false;
		maposition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (move) {
			//transform.Translate (new Vector3(-1f,0f,-0f) * speed * Time.deltaTime);
			transform.Translate (target * speed * Time.deltaTime);
			if ((maposition.x - 25f ) > transform.position.x)
				Destroy (this.gameObject);
		}

	}

	void OnTriggerEnter(Collider other) {
		other.GetComponent<PlayerController> ().Respawn ();
		Destroy (this.gameObject);
	}

	public void setTargetPosition(Vector3 pos){
		target = pos;
	}

	public void setSpeed(float v){
		speed = v;
	}

	public void StartTranslation(){
		move = true;

	}


}
