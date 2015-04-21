using UnityEngine;
using System.Collections;

public class ItemScript : MonoBehaviour {

	//private Transform wings;
	Animator animEnd;
	public float RespawnTimer;
	bool isActive;
	public float TempsEntreRespawn = 5;

	private SphereCollider collider;
	private SpriteRenderer sp;

	private AudioClip AudioUse;
	private AudioClip AudioLoot;
	// Use this for initialization
	void Start () {
		animEnd = GetComponent<Animator> ();
		RespawnTimer = 0;
		isActive = true;
		sp = GetComponent<SpriteRenderer> ();
		collider = GetComponent<SphereCollider> ();
		AudioUse = Resources.Load ("Sounds/LancerIt" )as AudioClip;
		AudioLoot =Resources.Load ("Sounds/LootItem" )as AudioClip;

	}
	
	// Update is called once per frame
	void Update () {
		RespawnTimer -= Time.deltaTime;
		if (!isActive && RespawnTimer < 0) {
			sp.enabled = true;
			collider.enabled=true;
		}

	}
	void  OnTriggerEnter (Collider other)
	{
		Debug.Log ("Collision Item");
		other.GetComponent<PlayerItemController> ().LootRandomItem ();
		//animEnd.CrossFade ("Destroy", 0.0f);
		isActive = false;
		RespawnTimer = TempsEntreRespawn;
		sp.enabled = false;
		collider.enabled=false;
		AudioSource.PlayClipAtPoint(AudioLoot,gameObject.transform.position,15f);
		//Destroy(gameObject) ;
		//gameObject.SetActive(false);

	}
	public void Activate(GameObject player)
	{
		if(gameObject.name =="Wings")
		player.GetComponent<PlayerItemController> ().BoostSpeed (3.0f);

		if(gameObject.name =="Chrome")
			player.GetComponent<PlayerItemController> ().ChromeThrow ();

		AudioSource.PlayClipAtPoint(AudioUse,gameObject.transform.position,15f);
	}
	public IEnumerator  wait()
	{
		yield return new WaitForSeconds (2f);
	}
}
