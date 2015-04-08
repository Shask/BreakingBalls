using UnityEngine;
using System.Collections;

public class Firefox : MonoBehaviour {

	private float speed = 0.15f;
	private Vector3 translateur;
	private SphereCollider spCol;
	// Use this for initialization
	void Start () {
		translateur = new Vector3 (0, 5, 0);
		spCol = this.GetComponent<SphereCollider> ();
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
	}

	void OnTriggerEnter(Collider other) {
		print ("Touché");
		Destroy (this);
	}

}
