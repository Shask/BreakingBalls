using UnityEngine;
using System.Collections;

public class FireBolt : MonoBehaviour {

	private Vector3 target;//= new Vector3(-10f,0f,0f);
	private float speed = 2f;

	private float _nextShotInSecond;

	private bool move=false;
	private Vector3 maposition;

	// Use this for initialization
	void Start () {
		//r.enabled = false;
		maposition = transform.position;
		_nextShotInSecond = 7f;
	}
	
	// Update is called once per frame
	void Update () {
		if (move) {
			//transform.Translate (new Vector3(-1f,0f,-0f) * speed * Time.deltaTime);
			transform.Translate (target * speed * Time.deltaTime);
			//if ((maposition.x - 25f ) > transform.position.x)
			if ((_nextShotInSecond -= Time.deltaTime) > 0)
				return;
				
			Destroy (this.gameObject);
			//transform.Rotate(new Vector3(0f,0f,30f));
		}

	}

	void OnTriggerEnter(Collider other) {
		other.GetComponent<PlayerController> ().Respawn (other.transform.position.x - 10);
		Destroy (this.gameObject);
	}

	public void setTargetPosition(Vector3 pos){
		target = (pos - transform.position)/5;
	}

	public void setTargetPosition(){
		target = new Vector3 (-1f, 0f, -0f);
	}

	public void setSpeed(float v){
		speed = v;
	}

	public void StartTranslation(){
		move = true;

	}


}
