using UnityEngine;
using System.Collections;

public class FireBolt : MonoBehaviour {

	private Vector3 target;
	private float speed = 0.15f;

	//public Transform _destination;
	public float _speed;
	private Vector3 maposition;
	
	public void Initialize(Transform destination, float vitesse, Vector3 depart)
	{

		target = destination.position;
		speed = vitesse;
		maposition = depart;
		transform.position = maposition;
	}

	// Use this for initialization
	void Start () {
		target = new Vector3 (2, 2, 0);
		transform.position = new Vector3 (0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		//transform.Rotate (new Vector3 (0, 0, 2));
		transform.Translate (target * speed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other) {
		Destroy (this);
	}

	public void setTargetPosition(Vector3 pos){
		target = pos;
	}

}
