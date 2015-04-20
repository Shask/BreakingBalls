using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ApplicationModel
{
	public static int[] playerChoice;
	public static Dictionary<int, float> playerTimes;
}

public class MenuController : MonoBehaviour {

	Image[] imagePlayers;
	bool[] onClick;
	string[] horizontalInputs = {"Horizontal1", "Horizontal2", "Horizontal3"};
	//Button buttonStart;

	// Use this for initialization
	void Start () {
		ApplicationModel.playerChoice = new int[] {0, 1 ,2};
		onClick = new bool[] {false, false, false};

		imagePlayers = new Image[3];
		for (int i = 0; i < 3; i ++) {
			imagePlayers [i] = GameObject.Find ("Image"+(i+1)).GetComponent<Image> ();
			changeAnimationPlayer(i);
		}
		//buttonStart = GameObject.Find ("ButtonStart").GetComponent<Button> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton ("Jump1")) {
			Application.LoadLevel ("SceneGwen");
		} else if (Input.GetButton ("Cancel")) {
			Application.Quit ();
		}

		for (int i = 0; i < 3; i++) {
			if (Input.GetAxisRaw (horizontalInputs[i]) > 0 && !onClick [i]) {
				ApplicationModel.playerChoice [i] += 1;
				ApplicationModel.playerChoice [i] %= 3;
				changeAnimationPlayer (i);
				onClick [i] = true;
			} else if (Input.GetAxisRaw (horizontalInputs[i]) < 0 && !onClick [i]) {
				ApplicationModel.playerChoice [i] += 2;
				ApplicationModel.playerChoice [i] %= 3;
				changeAnimationPlayer (i);
				onClick [i] = true;
			} else if (Input.GetAxisRaw (horizontalInputs[i]) == 0) {
				onClick [i] = false;
			}
		}
	}

	private void changeAnimationPlayer(int no)
	{
		Animator anim = imagePlayers [no].GetComponent<Animator> ();

		switch (ApplicationModel.playerChoice [no]) {
		case 0:
			anim.CrossFade ("animationImage"+(no+1)+"0", 0.0f);
			break;
		case 1:
			anim.CrossFade ("animationImage"+(no+1), 0.0f);
			break;
		case 2:
			anim.CrossFade ("animationImage"+(no+1)+"2", 0.0f); // temp
			break;
		}
	}
}
