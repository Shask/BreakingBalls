using UnityEngine;
using System.Collections;

public class PlatformControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		resizeCollider ();
	}

	void resizeCollider()
	{
		BoxCollider boxCollider;
		Transform image;
		SpriteRenderer srImage;

		boxCollider = transform.GetComponent<BoxCollider>();
		image = transform.GetChild (0);
		srImage = image.GetComponent<SpriteRenderer>();
		//transform.position = image.position;

		boxCollider.size = new Vector3(srImage.bounds.size.x / transform.localScale.x, srImage.bounds.size.y / transform.localScale.y, boxCollider.size.z);
		boxCollider.center = new Vector3((image.position.x - transform.position.x) / transform.localScale.x, (image.position.y - transform.position.y) / transform.localScale.y, boxCollider.center.z);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
