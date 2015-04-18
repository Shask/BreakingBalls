using UnityEngine;
using System.Collections;

public class Firefox : MonoBehaviour {

	private float speed = 0.15f;
	private Vector3 translateur;
	private SphereCollider spCol;

	private float _nextShotInSecond;
	private FireBolt projectile;
	private Transform destination;
	// Use this for initialization
	void Start () {
		translateur = new Vector3 (0, 5, 0);
		spCol = this.GetComponent<SphereCollider> ();

		destination.position = new Vector3 (0, 0, 0);
		//destination.transform.position = new Vector3 (5, 5, 0);
		_nextShotInSecond = 0.1f;
	}
	
	// Update is called once per frame
	void Update () {
		if (this.transform.position.y >= 0.5) {
			translateur = new Vector3 (0, -3, 0);
		}
		if (this.transform.position.y <= 0) {
			translateur = new Vector3 (0, 3, 0);
		}
		transform.Translate (translateur * speed * Time.deltaTime);
		//spCol.transform.Translate (new Vector3 (0, -3, 0) * speed*4 * Time.deltaTime);

		if ((_nextShotInSecond -= Time.deltaTime) > 0)
			return;

		_nextShotInSecond = 50f;
	//	bolt = (FireBolt)Instantiate(projectile,transform.position,transform.rotation);
	//	bolt.Initialize (destination, 0.25f, transform.position);
	}

	void OnTriggerEnter(Collider other) {
		print ("Touché");
		Destroy (this);
	}

}
