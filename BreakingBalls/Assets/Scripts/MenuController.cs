using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ApplicationModel
{
	public static int[] playerChoice;
}

public class MenuController : MonoBehaviour {

	Image[] imagePlayers;
	bool[] onClick;
	public string[] horizontalInputs = {"Horizontal", "Horizontal2", "Horizontal3"};
	//Button buttonStart;

	// Use this for initialization
	void Start () {
		ApplicationModel.playerChoice = new int[] {1, 1 ,1};
		onClick = new bool[] {false, false, false};

		imagePlayers = new Image[3];
		imagePlayers [0] = GameObject.Find ("Image1").GetComponent<Image>();
		imagePlayers [1] = GameObject.Find ("Image2").GetComponent<Image>();
		imagePlayers [2] = GameObject.Find ("Image3").GetComponent<Image>();
		//buttonStart = GameObject.Find ("ButtonStart").GetComponent<Button> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton ("Jump")) {
			Application.LoadLevel ("SceneGwen");
		}

		for (int i = 0; i < 3; i++) {
			if (Input.GetAxisRaw (horizontalInputs[i]) > 0 & !onClick [i]) {
				ApplicationModel.playerChoice [i] += 1;
				ApplicationModel.playerChoice [i] %= 3;
				changeAnimationPlayer (i);
				onClick [i] = true;
			} else if (Input.GetAxisRaw (horizontalInputs[i]) < 0 & !onClick [i]) {
				ApplicationModel.playerChoice [i] += 2;
				ApplicationModel.playerChoice [i] %= 3;
				changeAnimationPlayer (i);
				onClick [i] = true;
			} else if (Input.GetAxisRaw (horizontalInputs[i]) == i) {
				onClick [i] = false;
			}
		}
	}

	private void changeAnimationPlayer(int no)
	{
		Animator anim = imagePlayers [no].GetComponent<Animator> ();

		if (no != 0) // temp
			return;

		switch (ApplicationModel.playerChoice [no]) {
		case 0:
			anim.CrossFade ("animationImage"+(no+1)+"0", 0.0f);
			break;
		case 1:
			anim.CrossFade ("animationImage"+(no+1), 0.0f);
			break;
		case 2:
			anim.CrossFade ("animationImage"+(no+1)+"0", 0.0f); // temp
			break;
		}
	}
}
