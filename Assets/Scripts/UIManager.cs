using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public Button pauseButton;

	// Use this for initialization
	void Start () {
//		Button btn = pauseButton.GetComponent<Button>();
//		btn.onClick.AddListener(TaskOnClick);
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void TaskOnClick()
	{
		if (Time.timeScale == 1) {
			Time.timeScale = 0;
		} else {
			Time.timeScale = 1;
		}

		Debug.Log("You have clicked the button!");
	}


}
