using UnityEngine;
using System.Collections;

public class chromeBolt : MonoBehaviour {
	
	public Vector3 target = new Vector3 (10, 0, 0);//= new Vector3(-10f,0f,0f);
	public float speed = 0.1f;

	public float notActiveFor = 0.5f;

	
	private bool move=true;
	private Vector3 maposition;
	private BoxCollider bxcollider;

	public float lifeTime = 20;
	// Use this for initialization
	void Start () {
		//r.enabled = false;
		maposition = transform.position;
		lifeTime += Time.time ;
		notActiveFor += Time.time;
		bxcollider = GetComponent<BoxCollider> ();
	}
	
	// Update is called once per frame
	void Update () {

			//transform.Translate (new Vector3(-1f,0f,-0f) * speed * Time.deltaTime);
			transform.Translate (target * speed * Time.deltaTime);
		
			if(Time.time > lifeTime)
			Destroy (this.gameObject);
		if (Time.time > notActiveFor)
			bxcollider.enabled = true;
			//transform.Rotate(new Vector3(0f,0f,30f));

		
	}
	
	void OnTriggerEnter(Collider other) {
		other.GetComponent<PlayerPhysics> ().Move (new Vector2(-0.5f, 0));
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
