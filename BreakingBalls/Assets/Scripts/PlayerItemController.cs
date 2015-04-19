using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerItemController : MonoBehaviour {

	private bool hasItem = false;
	public List<GameObject> Item ;
	private int itemPicked;
	//private  MushroomItem;

	// Use this for initialization
	void Start () {
		//MushroomItem=ListItem.GetComponent("Mushroom");


	}
	
	// Update is called once per frame
	void Update () {
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

		Item[itemPicked].GetComponent<ItemScript>().Activate(gameObject);
		Item[itemPicked].SetActive(false);
		itemPicked = -1;
		hasItem = false;

	}

}
