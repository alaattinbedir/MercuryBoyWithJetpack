using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Threading;
using DG.Tweening;

//  This script will be updated in Part 2 of this 2 part series.
public class ModalPanel : MonoBehaviour {
	
	public Text message;
	public Text title;
	public Image iconImage;
	public Button yesButton;
	public Button noButton;
	public Button okButton;
	public GameObject modalPanelObject;
	

	private static ModalPanel modalPanel;
	
	public static ModalPanel Instance () {
		if (!modalPanel) {
			modalPanel = FindObjectOfType(typeof (ModalPanel)) as ModalPanel;
			if (!modalPanel)
				Debug.LogError ("There needs to be one active ModalPanel script on a GameObject in your scene.");
		}
		
		return modalPanel;
	}


	void Start(){
		DOTween.Init(false, false, LogBehaviour.Default);
	}

	// Yes/No/Cancel: A string, a Yes event, a No event and Cancel event
	public void Choice (string message, UnityAction yesEvent, UnityAction noEvent, UnityAction okEvent) {
		modalPanelObject.SetActive (true);
		modalPanelObject.transform.DOPunchScale (new Vector3 (0.3f, 0.3f, 0f), 0.4f, 5, 0.3f);
//		modalPanelObject.transform.DOPunchScale (new Vector3 (0.2f, 0.2f, 0f), 0.2f, 5, 0.2f);
		
		yesButton.onClick.RemoveAllListeners();
		yesButton.onClick.AddListener (yesEvent);
		yesButton.onClick.AddListener (ClosePanel);
		
		noButton.onClick.RemoveAllListeners();
		noButton.onClick.AddListener (noEvent);
		noButton.onClick.AddListener (ClosePanel);
		
		okButton.onClick.RemoveAllListeners();
		okButton.onClick.AddListener (okEvent);
		okButton.onClick.AddListener (ClosePanel);
		
		this.message.text = message;
		
		this.iconImage.gameObject.SetActive (false);
		yesButton.gameObject.SetActive (true);
		noButton.gameObject.SetActive (true);
		okButton.gameObject.SetActive (true);
	}

	// An annucenment: A string and Cancel event
	public void Choice (string title, string message, UnityAction okEvent) {
		modalPanelObject.SetActive (true);
		modalPanelObject.transform.DOPunchScale (new Vector3 (0.3f, 0.3f, 0f), 0.4f, 5, 0.3f);
//		modalPanelObject.transform.DOPunchScale (new Vector3 (0.3f, 0.3f, 0f), 0.8f, 5, 0.3f);

//		modalPanelObject.transform.DOShakeScale (1, 1, 10, 90);

		okButton.onClick.RemoveAllListeners();
		okButton.onClick.AddListener (okEvent);
		okButton.onClick.AddListener (ClosePanel);
		
		this.title.text = title;
		this.message.text = message;

		
		this.iconImage.gameObject.SetActive (true);
		yesButton.gameObject.SetActive (false);
		noButton.gameObject.SetActive (false);
		okButton.gameObject.SetActive (true);
	}

	//  Yes/No: A string, a Yes event, a No event (No Cancel Button);
	public void Choice (string title,string message, UnityAction yesEvent, UnityAction noEvent) {
		modalPanelObject.SetActive (true);
		modalPanelObject.transform.DOPunchScale (new Vector3 (0.3f, 0.3f, 0f), 0.4f, 5, 0.3f);

		yesButton.onClick.RemoveAllListeners();
		yesButton.onClick.AddListener (yesEvent);
		yesButton.onClick.AddListener (ClosePanel);
		
		noButton.onClick.RemoveAllListeners();
		noButton.onClick.AddListener (noEvent);
		noButton.onClick.AddListener (ClosePanel);

		this.title.text = title;
		this.message.text = message;
		
		this.iconImage.gameObject.SetActive(false);
		yesButton.gameObject.SetActive(true);
		noButton.gameObject.SetActive(true);
		okButton.gameObject.SetActive(false);

	}

	//  Yes/No: A string, a Yes event, a No event (No Cancel Button);
	public void Choice2 (string title,string message, UnityAction yesEvent, UnityAction noEvent) {
		modalPanelObject.SetActive (true);
		modalPanelObject.transform.DOPunchScale (new Vector3 (0.3f, 0.3f, 0f), 0.4f, 5, 0.3f);

		yesButton.onClick.RemoveAllListeners();
		yesButton.onClick.AddListener (yesEvent);
		yesButton.onClick.AddListener (ClosePanel);
		
		noButton.onClick.RemoveAllListeners();
		noButton.onClick.AddListener (noEvent);
		noButton.onClick.AddListener (ClosePanel);

		this.title.text = title;
		this.message.text = message;
		
		this.iconImage.gameObject.SetActive(false);
		yesButton.gameObject.SetActive(true);
		noButton.gameObject.SetActive(true);
		okButton.gameObject.SetActive(false);
	
		StartCoroutine(ExecuteAfterTime(2));
	}


	IEnumerator ExecuteAfterTime(float time)
	{
		yield return new WaitForSeconds(time);
		
		// Code to execute after the delay
		// not used anymore because of add policy
		//if(GameData.Instance.noAd != 99)
			//showHZAdVideos ();
	}

	public void showHZAdVideos(){

		//if (HZIncentivizedAd.IsAvailable ()) {
		//	HZIncentivizedAd.Shoglew ();
		//}

		//HZIncentivizedAd.Fetch ();

		//		// Later, such as after a level is completed
		//		if (HZVideoAd.IsAvailable()) {
		//			HZVideoAd.Show();
		//			HZVideoAd.Fetch();
		//		}

	}

	// Yes/No/Cancelwith a Yes event, a No event 
	public void Choice (string message, Sprite iconImage, UnityAction yesEvent, UnityAction noEvent) {
		modalPanelObject.SetActive (true);
		modalPanelObject.transform.DOPunchScale (new Vector3 (0.3f, 0.3f, 0f), 0.4f, 5, 0.3f);

		yesButton.onClick.RemoveAllListeners();
		yesButton.onClick.AddListener (yesEvent);
		yesButton.onClick.AddListener (ClosePanel);
		
		noButton.onClick.RemoveAllListeners();
		noButton.onClick.AddListener (noEvent);
		noButton.onClick.AddListener (ClosePanel);
		

		this.message.text = message;
		this.iconImage.sprite = iconImage;
		
		this.iconImage.gameObject.SetActive (false);
		yesButton.gameObject.SetActive (true);
		noButton.gameObject.SetActive (true);
		okButton.gameObject.SetActive (false);
	}

	// Yes/No/Cancelwith Image: A string, a sprite ,a Yes event, a No event and Cancel event
	public void Choice (string message, Sprite iconImage, UnityAction okEvent) {
		modalPanelObject.SetActive (true);
		modalPanelObject.transform.DOPunchScale (new Vector3 (0.3f, 0.3f, 0f), 0.4f, 5, 0.3f);

		okButton.onClick.RemoveAllListeners();
		okButton.onClick.AddListener (okEvent);
		okButton.onClick.AddListener (ClosePanel);
		
		this.message.text = message;
		this.iconImage.sprite = iconImage;
		
		this.iconImage.gameObject.SetActive (true);
		yesButton.gameObject.SetActive (false);
		noButton.gameObject.SetActive (false);
		okButton.gameObject.SetActive (true);
	}

	void ClosePanel () {
		modalPanelObject.SetActive (false);
	}

}