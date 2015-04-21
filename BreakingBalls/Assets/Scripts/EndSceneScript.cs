using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class EndSceneScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject[] images = new GameObject[3];

		Text textEnd = GameObject.Find ("TextEnd").GetComponent<Text>();

		IOrderedEnumerable<KeyValuePair<int, float>> list = ApplicationModel.playerTimes.OrderBy (i => i.Value);

		for(int i = 0; i< 3; i++)
		{
			images[i] = GameObject.Find ("Image "+(i+1));
			int key =  list.ElementAt (i).Key;
			string nameAnimator = "animatorIddle" + key;
			if(ApplicationModel.playerChoice[i] == 0 || ApplicationModel.playerChoice[i] == 2)
				nameAnimator = nameAnimator + ApplicationModel.playerChoice[i];
			images[i].GetComponent<Animator> ().CrossFade (nameAnimator, 0.0f);
			Text textTime = GameObject.Find ("TextTime"+(i+1)).GetComponent<Text>();
			float time = list.ElementAt(i).Value;
			textTime.text = string.Format ("{0:#0}:{1:00}.{2:00}",
			                                                 Mathf.Floor (time / 60),
			                                                 Mathf.Floor (time) % 60,
			                                                 Mathf.Floor ((time * 100) % 100));
			if(i == 0){
				textEnd.text = "Congratulations player " + key + ", you win !";
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Cancel")){
			Application.LoadLevel ("SceneMenu");
		}
	}
}
