using UnityEngine;
using System.Collections;

public class PlateformPiege : MonoBehaviour {

	private float _nextShotInSecond = 0.5f;
	private bool e = false;
	private GameObject p;
	// Use this for initialization
	void Start () {
		p = gameObject.GetComponentInChildren<Piege> ().gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
		if ((_nextShotInSecond -= Time.deltaTime) > 0)
			return;
		

		if (e) {
			e=false;
			_nextShotInSecond = 4f;
		} else {
			e= true;
			_nextShotInSecond = 1.5f;
		}

		p.SetActive(e);

	}
}
