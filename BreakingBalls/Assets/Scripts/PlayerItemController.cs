using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerItemController : MonoBehaviour {
	
	private bool hasItem = false;
	public List<GameObject> Item ;
	private int itemPicked;
	
	private PlayerController playerController;
	
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
	}
	
	// Update is called once per frame
	void Update () {
		if (onItem && timerItem <= Time.time) {
			ItemEnd ();
		}
		if (hasItem && itemPicked!=-1) {
			if (Input.GetButtonDown ("Fire1")) {
				UseItem ();
			}
		}
		
	}
	public void LootRandomItem()
	{
		hasItem = true;
		int item=Random.Range (0, Item.Count-1);
		Item[item].SetActive(true);
		itemPicked = item;
	}
	void UseItem()
	{
		if (!onItem) {
			Item [itemPicked].GetComponent<ItemScript> ().Activate (gameObject);
			Item [itemPicked].SetActive (false);
			
			GameObject go = (GameObject)Resources.Load (Item [itemPicked].name);
			
			itemFeedBack = Instantiate (go, gameObject.transform.localPosition, Quaternion.identity) as GameObject; 
			itemFeedBack.transform.parent = transform;
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
	
	
}