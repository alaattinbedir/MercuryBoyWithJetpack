using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;


public class GameData {

	public int totalCoins;
	public int bestScore;
	public int noAd;
	public string iTunesLink;
	public string googlePlayLink;
	public int isSoundON;
	public int isMusicON;
	public uint counter = 0;

	private static GameData _instance = null;
	
	private GameData() {
//		gameDataUpdate = new ParseObject ("GameData");
	}
	
	public static GameData Instance {
		get {
			if (_instance == null) {
				_instance = new GameData();
			}
			return _instance;
		}
	}


	public void Save(){

		PlayerPrefs.SetInt("totalCoins",totalCoins);
		PlayerPrefs.SetInt("bestScore",bestScore);
		PlayerPrefs.SetInt("noAd",noAd);
		PlayerPrefs.SetInt("isMusicON",isMusicON);
		PlayerPrefs.SetInt("isSoundON",isSoundON);
	}

	public void Get(){

		totalCoins = PlayerPrefs.HasKey("totalCoins") ? PlayerPrefs.GetInt("totalCoins"):0;
		bestScore = PlayerPrefs.HasKey("bestScore") ? PlayerPrefs.GetInt("bestScore"):0;
		noAd = PlayerPrefs.HasKey("noAd") ? PlayerPrefs.GetInt("noAd"):0;
		isMusicON = PlayerPrefs.HasKey("isMusicON") ? PlayerPrefs.GetInt("isMusicON"):1;
		isSoundON = PlayerPrefs.HasKey("isSoundON") ? PlayerPrefs.GetInt("isSoundON"):1;
	}

}
