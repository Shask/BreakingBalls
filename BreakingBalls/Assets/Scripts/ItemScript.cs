using UnityEngine;
using System.Collections;

public class ItemScript : MonoBehaviour {

	private Transform wings;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void  OnTriggerEnter (Collider other)
	{
		other.GetComponent<PlayerItemController> ().LootRandomItem ();
		gameObject.SetActive (false);
	}
	public void Activate(GameObject player)
	{
		player.GetComponent<PlayerItemController> ().BoostSpeed (3.0f);
		//GameObject ItemFeedBack= (GameObject) Instantiate (wings);

	}
}
