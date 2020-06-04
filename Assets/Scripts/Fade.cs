using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DentedPixel;

public class Fade : MonoBehaviour {
	
	CanvasGroup canvasGroup;

	void Start()
	{
		canvasGroup = this.GetComponent<CanvasGroup>();

		if(canvasGroup == null)
		{
			Debug.Log("Must have canvas group attached!");
			this.enabled = false;
		}


	}



}
