using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Threading;


//  This script will be updated in Part 2 of this 2 part series.
public class DialogueManager : MonoBehaviour {
	public Sprite icon;
	private ModalPanel modalPanel;
	public AudioClip menuClip;

	private static DialogueManager dialogueManager;
	
	public static DialogueManager Instance () {
		if (!dialogueManager) {
			dialogueManager = FindObjectOfType(typeof (DialogueManager)) as DialogueManager;
			if (!dialogueManager)
				Debug.LogError ("There needs to be one active DialogueManager script on a GameObject in your scene.");
		}
		
		return dialogueManager;
	}
	


	void Awake () {
		modalPanel = ModalPanel.Instance ();
	}


	void Start(){

	}

	public void showDialog(string title, string message,int tag){
		modalPanel = ModalPanel.Instance ();
		modalPanel.Choice (title, message, () => {PostOkFunction (tag);});
	}

	public void showConfirmDialog(string title, string message,int tag,bool multi){
		modalPanel = ModalPanel.Instance ();
		if (multi) {
			modalPanel.Choice (title,message, () => {
				PostYesFunction (tag);}, () => {
				PostNoFunction (tag);});
		} else {
			modalPanel.Choice2 (title,message, () => {
				PostYesFunction (tag);}, () => {
				PostNoFunction (tag);});
		}

	}

	void PostYesFunction(int tag){
		if (GameData.Instance.isSoundON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		switch (tag) {
		case 0:
			
			break;
		case 10:{
			
		}
			break;
		case 33:{
			
		}
			break;
		case 77:
			break;
		case 88:
			{
				
			}
			break;
		case 90:{
				Application.Quit ();
			}
			break;
		default:{
			
		}
			break;
		}
		
	}

	void PostNoFunction(int tag){
		if (GameData.Instance.isSoundON != 0)
			SoundManager.instance.PlaySingle(menuClip);


		switch (tag) {
		case 0:
			break;
		case 10:{
		}
			break;
		case 33:{
		}
			break;
		case 77:{
		}
			break;
		case 88:{
		}
			break;
		case 90:
			break;
		default:
			//gameLogic.BackButtonClicked();
			break;
		}
	}

	void PostOkFunction(int tag){
		if (GameData.Instance.isSoundON != 0)
			SoundManager.instance.PlaySingle(menuClip);

		switch (tag) {
		case 50:{

			#if UNITY_ANDROID
			Application.OpenURL(GameData.Instance.googlePlayLink);
			#elif UNITY_IPHONE
			Application.OpenURL(GameData.Instance.iTunesLink);
			#endif

			GameObject.Find("MainCamera").SetActive(false);

			Thread.Sleep(200);
			Application.Quit(); 
		}
			break;

		case 100:{
			
		}
			break;
		case 500:{
			
		}
			break;
		case 501:{
			
		}
			break;
		case 600:{
			
		}
			break;
		case 700:{
			
		}
			break;
		
		default:
			break;
		}
	}
}
