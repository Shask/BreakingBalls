using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerItemController : MonoBehaviour {
	
	private bool hasItem = false;
	public List<GameObject> Item ;
	private int itemPicked;
	
	private PlayerController playerController;
	private string inputFire;
	private Animator playerAnim;
	
	GameObject itemFeedBack;

	
	
	
	float timerItem=0;
	bool onItem=false;
	
	
	float oldMaxSpeed ;
	float oldAcceleration ;
	float oldJump;
	bool isInvincible ;
	
	// Use this for initialization
	void Start () {
		//MushroomItem=ListItem.GetComponent("Mushroom");
	

		playerController = GetComponent<PlayerController> (); 
		ItemInit ();
		inputFire = "Fire" + playerController.playerNo;
		playerAnim = GetComponent<Animator> ();
		Debug.Log ("ItemP" + playerController.playerNo + "Menu/Wings");
		string s = "/Canvas/Player" + playerController.playerNo + "Panel/ItemP" + playerController.playerNo + "Menu/Wings";
		string s2 = "/Canvas/Player" + playerController.playerNo + "Panel/ItemP" + playerController.playerNo + "Menu/Chrome";
		GameObject Wings = GameObject.Find (s);
		GameObject Chrome = GameObject.Find (s2);
		Item[1]= Chrome;
		Item [0] = Wings;


	}
	
	// Update is called once per frame
	void Update () {
		if (onItem && timerItem <= Time.time) {
			ItemEnd ();
		}
		if (hasItem && itemPicked!=-1) {
			if (Input.GetButtonDown (inputFire)) {
				UseItem ();
			}
		}
		
	}
	public void LootRandomItem()
	{
		if (!hasItem) {
			hasItem = true;
			int item = Random.Range (0, Item.Count);
			Item [item].SetActive (true);
			itemPicked = item;
		}
	}
	void UseItem()
	{
		if (!onItem) {
			Item [itemPicked].GetComponent<ItemScript> ().Activate (gameObject);
			Item [itemPicked].SetActive (false);
			
			GameObject go = (GameObject)Resources.Load (Item [itemPicked].name);

			Vector3 itemPosition = gameObject.transform.localPosition;
			if( Item [itemPicked].name=="Chrome")
			{
				playerAnim.SetTrigger ("Attack");
				if(transform.localRotation.y!=0 ) itemPosition+=new Vector3(-1,0,0);
				else itemPosition+=new Vector3(1,0,0);
			}
			itemFeedBack = Instantiate (go, itemPosition, Quaternion.identity) as GameObject; 
			if(Item [itemPicked].name=="Wings")itemFeedBack.transform.parent = transform ;
			itemFeedBack.transform.rotation=transform.rotation;
			itemFeedBack.SetActive (true);
			
			itemPicked = -1;
			hasItem = false;
		}
		
	}
	void ItemEnd()
	{

		playerController.speed=oldMaxSpeed;
		playerController.acceleration = oldAcceleration;
		playerController.jumpHeight = oldJump;
		isInvincible = false;
		onItem = false;
		itemFeedBack.SetActive (false);
		Destroy (itemFeedBack);
		
	}
	void ItemInit()
	{
		oldJump = playerController.jumpHeight;
		oldMaxSpeed = playerController.speed;
		oldAcceleration = playerController.acceleration;
		isInvincible = false;
		onItem = false;
	}
	public void BoostSpeed(float timer)
	{

		playerController.jumpHeight += 2;
		playerController.speed += 3;
		playerController.acceleration += 100;
		timerItem = Time.time + timer;
		onItem = true;
		
	}
	public void ChromeThrow()
	{

	}
	
	
}