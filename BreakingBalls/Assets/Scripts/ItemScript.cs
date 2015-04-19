using UnityEngine;
using System.Collections;

public class ItemScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void  OnTriggerEnter (Collider other)
	{
		other.GetComponent<PlayerItemController> ().LootRandomItem ();
	}
	public void Activate(GameObject player)
	{
		player.GetComponent<PlayerController> ().BoostSpeed (10.0f);
	}
}
