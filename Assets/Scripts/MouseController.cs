﻿using System;
using System.Threading;
using GoogleMobileAds.Api;
using GooglePlayGames;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

#if UNITY_IOS
using UnityEngine.SocialPlatforms.GameCenter;
#endif


public class MouseController : MonoBehaviour {

	private InterstitialAd interstitial;
	private RewardedAd rewardedAd;
	private bool GOD_MODE = false;
	public float jetpackForce;
	public float forwardMovementSpeed;
	public Rigidbody2D rb;
	public Transform groundCheckTransform;
	private DialogueManager dialogueManager;
	private bool grounded;
	public LayerMask groundCheckLayerMask;

	Animator animator;
	public ParticleSystem jetpack;
	private bool dead = false;
	private bool loggedIn = false;
	private bool gameOver = false;
	private bool paused = false;
	private uint coins = 0;
	private uint speed = 50;
	private uint lifeCount = 1;


	public Texture2D coinIconTexture;

	public AudioClip coinCollectSound;

	public AudioClip explosionSound;

	public AudioSource jetpackAudio;

	public AudioSource footstepsAudio;

	public ParallaxScroll parallax;

	public Text textCoinsCount;
	public Text textDistance;
    public Text textDistanceM;
    public Text textHighDistance;
    public Text textHighDistanceM;
    public Text textSpeed;
    public Text textSpeedKMH;
    public Text textLife;
	public Text textCoinsCountGameOver;
	public Text textDistanceGameOver;
	public Text textBestDistanceGameOver;

	public Text textStart;
	public Text textContinue;

	public Image imgCoinsCount;
	public Image imgtDistance;
	public Image imgHighDistance;
	public Image imgSpeed;
	public Image imgLife;

	public float distance;
	public bool level0;
	public bool level1;
	public bool level2;
	public bool level3;
	public bool level4;
	public bool level5;
	public bool level6;
	public bool level7;
	public bool level8;
	public bool level9;
	public bool level10;
	public bool level11;
	public bool level12;
	public bool level13;
	public bool level14;
	public bool level15;
	public bool level16;
	public bool level17;
	public bool level18;
	public bool level19;
	public bool level20;
	private bool life1;
	private bool life2;
	private bool life3;
	private bool life4;
	private bool life5;
	public GameObject pauseButton;
	public GameObject pausePanel;
	public GameObject gameOverPanel;
	public GameObject menuPanel;
	public GameObject mercuryBoy;
	public GameObject smoke;
	public Button music;
	public Button musicBan;
	public Button sound;
	public Button soundBan;

	public Button musicPause;
	public Button musicBanPause;
	public Button soundPause;
	public Button soundBanPause;

	float timeLeft = 2.0f;

	public AudioClip menuSelect;
	public AudioClip backgroudMusic;
	

	void Awake(){

        MobileAds.Initialize(initStatus => { });

#if UNITY_IPHONE

		if (!Social.localUser.authenticated) {
		Social.localUser.Authenticate (ProcessAuthentication);
		}

		GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);

#endif
#if UNITY_ANDROID
        // recommended for debugging:
  //      PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
		//// enables saving game progress.
		//.EnableSavedGames()
		//.RequestServerAuthCode(false)
		//// requests an ID token be generated.  This OAuth token can be used to
		////  identify the player to other services such as Firebase.
		//.RequestIdToken()
		//.Build();

		//PlayGamesPlatform.InitializeInstance(config);

		PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();

        LogIn();

#endif

	}

	
	


public void LogIn ()
	{

		if (loggedIn == true)
        {
			return;
        }
        //// authenticate user:
  //      PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, (result) =>
  //      {
  //          //	// handle results


  //          Firebase.Analytics.FirebaseAnalytics
		//			  .LogEvent(
		//				Firebase.Analytics.FirebaseAnalytics.EventJoinGroup,
		//				Firebase.Analytics.FirebaseAnalytics.ParameterGroupId,
		//				"android_login"
		//			  );
		//});

        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    Debug.Log("Login Sucess");

					loggedIn = true;

				}
                else
                {
                    Debug.Log("Login failed");
					loggedIn = false;
				}
            });
        }
    }

	public void OnNoAdvertisement(){
		if (GameData.Instance.isSoundON != 0)
			SoundManager.instance.PlaySingle(menuSelect);


//#if UNITY_ANDROID
//        Application.OpenURL("https://play.google.com/store/apps/details?id=com.magiclampgames.NumberGame2");
//#elif UNITY_IPHONE
//        Application.OpenURL("https://itunes.apple.com/us/app/mastermind-numbers-the-best-iq-puzzle-game/id1000344141");
//#endif

        string message = "No Ad functionality is not available for this version";
        dialogueManager = DialogueManager.Instance();
        dialogueManager.showDialog("Info", message, 100);

    }

    public void OnShowAchivements(){
		if (GameData.Instance.isSoundON != 0)
			SoundManager.instance.PlaySingle(menuSelect);

#if UNITY_IPHONE

		Social.ShowAchievementsUI();

#endif
#if UNITY_ANDROID

        ((PlayGamesPlatform)Social.Active).ShowAchievementsUI();
#endif

    }

	public void OnShowLeaderBoard ()
	{
		if (GameData.Instance.isSoundON != 0)
			SoundManager.instance.PlaySingle(menuSelect);

#if UNITY_IPHONE

		Social.ShowLeaderboardUI();

#endif
#if UNITY_ANDROID

		Social.ShowLeaderboardUI (); 
#endif


	}

	void CallbackIncentivizedAd(){
		// The user has watched the entire video and should be given a reward.
		Firebase.Analytics.FirebaseAnalytics
		  .LogEvent(
			Firebase.Analytics.FirebaseAnalytics.EventJoinGroup,
			Firebase.Analytics.FirebaseAnalytics.ParameterGroupId,
			"rewarded_video"
		  );
		
		textContinue.gameObject.SetActive (true);


		Thread.Sleep(1000);
		OnRiseAfterAdVideo();
	}

	// Use this for initialization
	void Start () {
		gameOverPanel.SetActive (false);
		gameOver = false;
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		distance = 0;
		textHighDistance.text = PlayerPrefs.GetInt("highscore", 0).ToString();

		Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
		Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;

		RequestInterstitial();
		CreateAndLoadRewardedAd();
		SetSoundAndMusic ();
		OnMenu ();
	}

	public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
	{
		UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
	}

	public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
	{
		UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
	}

	public void CreateAndLoadRewardedAd()
	{
		//// Test
		//#if UNITY_ANDROID
		//            string adUnitId = "ca-app-pub-3940256099942544/5224354917";
		//#elif UNITY_IPHONE
		//		string adUnitId = "ca-app-pub-3940256099942544/1712485313";
		//#else
		//            string adUnitId = "unexpected_platform";
		//#endif

#if UNITY_ANDROID
            string adUnitId = "ca-app-pub-7610769761173728/9356844697";
#elif UNITY_IPHONE
		string adUnitId = "ca-app-pub-7610769761173728/1973178698";
#else
            string adUnitId = "unexpected_platform";
#endif

		this.rewardedAd = new RewardedAd(adUnitId);

		this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
		this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
		this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the rewarded ad with the request.
		this.rewardedAd.LoadAd(request);
	}

	public void HandleRewardedAdLoaded(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardedAdLoaded event received");
	}

	public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
	{
		MonoBehaviour.print(
			"HandleRewardedAdFailedToLoad event received with message: "
							 + args.Message);
	}

	public void HandleRewardedAdOpening(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardedAdOpening event received");
	}

	public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
	{
		MonoBehaviour.print(
			"HandleRewardedAdFailedToShow event received with message: "
							 + args.Message);
	}

	public void HandleRewardedAdClosed(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardedAdClosed event received");
		this.CreateAndLoadRewardedAd();
	}

	public void HandleUserEarnedReward(object sender, Reward args)
	{
		string type = args.Type;
		double amount = args.Amount;
		MonoBehaviour.print(
			"HandleRewardedAdRewarded event received for "
						+ amount.ToString() + " " + type);

        CallbackIncentivizedAd();
	}

	private void RequestInterstitial()
	{
		if (this.interstitial != null)
        {
			this.interstitial.Destroy();
        }

        //// Test ortami
        //#if UNITY_ANDROID
        //        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
        //#elif UNITY_IPHONE
        //		string adUnitId = "ca-app-pub-3940256099942544/4411468910";
        //#else
        //        string adUnitId = "unexpected_platform";
        //#endif

        //PROD ortami
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-7610769761173728/6403378295";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-7610769761173728/2112779498";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);

		// Called when an ad request has successfully loaded.
		this.interstitial.OnAdLoaded += HandleOnAdLoaded;
		// Called when an ad request failed to load.
		this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
		// Called when an ad is shown.
		this.interstitial.OnAdOpening += HandleOnAdOpened;
		// Called when the ad is closed.
		this.interstitial.OnAdClosed += HandleOnAdClosed;
		// Called when the ad click caused the user to leave the application.
		this.interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;

		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the interstitial with the request.
		this.interstitial.LoadAd(request);
	}

	// This function gets called when Authenticate completes
	// Note that if the operation is successful, Social.localUser will contain data from the server. 
	void ProcessAuthentication (bool success) {
		if (success) {
			Debug.Log ("Authenticated, checking achievements");

			Firebase.Analytics.FirebaseAnalytics
			  .LogEvent(
				Firebase.Analytics.FirebaseAnalytics.EventJoinGroup,
				Firebase.Analytics.FirebaseAnalytics.ParameterGroupId,
				"ios_login"
			  );

			// Request loaded achievements, and register a callback for processing them
			Social.LoadAchievements (ProcessLoadedAchievements);
		} else {
			Debug.Log ("Failed to authenticate");
		}
	}

	// This function gets called when the LoadAchievement call completes
	void ProcessLoadedAchievements (IAchievement[] achievements) {
		if (achievements.Length == 0)
			Debug.Log ("Error: no achievements found");
		else
			Debug.Log ("Got " + achievements.Length + " achievements");
	}

	public void HandleOnAdLoaded(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdLoaded event received");
	}

	public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
							+ args.Message);
	}

	public void HandleOnAdOpened(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdOpened event received");
	}

	public void HandleOnAdClosed(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdClosed event received");
        RequestInterstitial();
	}

	public void HandleOnAdLeavingApplication(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdLeavingApplication event received");
	}

	public void BackButtonClicked()
	{
		Firebase.Analytics.FirebaseAnalytics
			  .LogEvent(
				Firebase.Analytics.FirebaseAnalytics.EventJoinGroup,
				Firebase.Analytics.FirebaseAnalytics.ParameterGroupId,
				"android_exit"
			  );

		Application.Quit();
	}



	// Update is called once per frame
	void Update () {
		if (paused || gameOver)
			return;




		timeLeft -= Time.deltaTime;
		if(timeLeft < 0)
		{
			GOD_MODE = false;
		}

#if UNITY_ANDROID
		if (Input.GetKeyDown(KeyCode.Escape)) {
			//if (HeyzapAds.OnBackPressed())
			//	return;
			//else
				BackButtonClicked ();
		}
#endif

		DisplayRestartButton ();
		DisplayCoinsCount ();
	}

	void ReportAchivement(string achivementID){
		// You can also call into the functions like this

		Social.ReportProgress(achivementID, 100.0f, (bool success) => {
			// handle success or failure
			if (success)
				Debug.Log("Successfully reported achievement progress");
			else
				Debug.Log("Failed to report achievement");
		});
	}

	void FixedUpdate () 
	{
		if (paused || gameOver)
			return;


		bool jetpackActive = Input.GetButton ("Fire1");

		jetpackActive = jetpackActive && !dead;
		if (jetpackActive) {
			rb.AddForce (new Vector2 (0, jetpackForce));
		}

		if (!dead) {
			Vector2 newVelocity = rb.velocity; 
			newVelocity.x = forwardMovementSpeed;
			rb.velocity = newVelocity;
		}

		UpdateGroundedStatus ();

		AdjustJetpack (jetpackActive);

		AdjustFootstepsAndJetpackSound (jetpackActive);

		parallax.offset = transform.position.x;

		distance += Time.deltaTime * forwardMovementSpeed;

		if (!level0 && (int)distance == 500) {
			forwardMovementSpeed += 0.2f;
			level0 = true;
			speed += 5;

#if UNITY_IPHONE
			ReportAchivement ("HunderedID");
#endif
#if UNITY_ANDROID
			ReportAchivement ("CgkI2YLQ_YUNEAIQHA");
#endif


		} else if (!level1 && (int)distance == 1000) {
			forwardMovementSpeed += 0.2f;
			level0 = false;
			level1 = true;
			speed += 5;

#if UNITY_IPHONE
			ReportAchivement ("FiveHunderedID");
#endif
#if UNITY_ANDROID
			ReportAchivement ("CgkI2YLQ_YUNEAIQBA");
#endif

		} else if (!level2 && (int)distance == 1500) {
			forwardMovementSpeed += 0.2f;
			level1 = false;
			level2 = true;
			speed += 5;

#if UNITY_IPHONE
			ReportAchivement ("BinMetreID");
#endif
#if UNITY_ANDROID
			ReportAchivement ("CgkI2YLQ_YUNEAIQBQ");
#endif

		} else if (!level3 && (int)distance == 2000) {
			forwardMovementSpeed += 0.2f;
			speed += 5;
			level2 = false;
			level3 = true;

#if UNITY_IPHONE
			ReportAchivement ("BinBesyuzID");
#endif
#if UNITY_ANDROID
			ReportAchivement ("CgkI2YLQ_YUNEAIQBg");
#endif

		} else if (!level4 && (int)distance == 2500) {
			forwardMovementSpeed += 0.2f;
			speed += 5;
			level3 = false;
			level4 = true;

#if UNITY_IPHONE
			ReportAchivement ("ikibinID");
#endif
#if UNITY_ANDROID
			ReportAchivement ("CgkI2YLQ_YUNEAIQBw");
#endif

		} else if (!level5 && (int)distance == 3000) {
			forwardMovementSpeed += 0.2f;
			speed += 5;
			level4 = false;
			level5 = true;

#if UNITY_IPHONE
			ReportAchivement ("ikibinbesyuzID");
#endif
#if UNITY_ANDROID
			ReportAchivement ("CgkI2YLQ_YUNEAIQCA");
#endif

		} else if (!level6 && (int)distance == 3500) {
			forwardMovementSpeed += 0.2f;
			speed += 5;
			level5 = false;
			level6 = true;

#if UNITY_IPHONE
			ReportAchivement ("ucbinID");
#endif
#if UNITY_ANDROID
			ReportAchivement ("CgkI2YLQ_YUNEAIQCQ");
#endif

		} else if (!level7 && (int)distance == 4000) {
			forwardMovementSpeed += 0.2f;
			level6 = false;
			level7 = true;
			speed += 5;

#if UNITY_IPHONE
			ReportAchivement ("ucbinbesyuzID");
#endif
#if UNITY_ANDROID
			ReportAchivement ("CgkI2YLQ_YUNEAIQEg");
#endif

		} else if (!level8 && (int)distance == 4500) {
			forwardMovementSpeed += 0.2f;
			level7 = false;
			level8 = true;
			speed += 5;

#if UNITY_IPHONE
			ReportAchivement ("dortbinID");
#endif
#if UNITY_ANDROID
			ReportAchivement ("CgkI2YLQ_YUNEAIQEQ");
#endif

		} else if (!level9 && (int)distance == 5000) {
			forwardMovementSpeed += 0.2f;
			level8 = false;
			level9 = true;
			speed += 5;

#if UNITY_IPHONE
			ReportAchivement ("dortbinbesyuzID");
#endif
#if UNITY_ANDROID
			ReportAchivement ("CgkI2YLQ_YUNEAIQFA");
#endif

		} else if (!level10 && (int)distance == 5500) {
			forwardMovementSpeed += 0.2f;
			level9 = false;
			level10 = true;
			speed += 5;

#if UNITY_IPHONE
			ReportAchivement ("besbinID");
#endif
#if UNITY_ANDROID
			ReportAchivement ("CgkI2YLQ_YUNEAIQFQ");
#endif

		} else if (!level11 && (int)distance == 6000) {
			forwardMovementSpeed += 0.2f;
			level10 = false;
			level11 = true;
			speed += 5;

#if UNITY_IPHONE
			ReportAchivement ("besbinbesyuzID");
#endif
#if UNITY_ANDROID
			ReportAchivement ("CgkI2YLQ_YUNEAIQFg");
#endif

		} else if (!level12 && (int)distance == 6500) {
			forwardMovementSpeed += 0.2f;
			level11 = false;
			level12 = true;
			speed += 5;

#if UNITY_IPHONE
			ReportAchivement ("altibinID");
#endif
#if UNITY_ANDROID
			ReportAchivement ("CgkI2YLQ_YUNEAIQFw");
#endif

		} else if (!level13 && (int)distance == 7000) {
			forwardMovementSpeed += 0.2f;
			level12 = false;
			level13 = true;
			speed += 5;

#if UNITY_IPHONE
			ReportAchivement ("altibinbesyuzID");
#endif
#if UNITY_ANDROID
			ReportAchivement ("CgkI2YLQ_YUNEAIQHg");
#endif

		} else if (!level14 && (int)distance == 7500) {
			forwardMovementSpeed += 0.2f;
			level13 = false;
			level14 = true;
			speed += 5;

#if UNITY_IPHONE
			ReportAchivement ("yedibinID");
#endif
#if UNITY_ANDROID
			ReportAchivement ("CgkI2YLQ_YUNEAIQGA");
#endif

		} else if (!level15 && (int)distance == 8000) {
			forwardMovementSpeed += 0.2f;
			level14 = false;
			level15 = true;
			speed += 5;

#if UNITY_IPHONE
			ReportAchivement ("yedibinbesyuzID");
#endif
#if UNITY_ANDROID
			ReportAchivement ("CgkI2YLQ_YUNEAIQHw");
#endif

		} else if (!level16 && (int)distance == 8500) {
			forwardMovementSpeed += 0.2f;
			level15 = false;
			level16 = true;
			speed += 5;

#if UNITY_IPHONE
			ReportAchivement ("sekizbinID");
#endif
#if UNITY_ANDROID
			ReportAchivement ("CgkI2YLQ_YUNEAIQGQ");
#endif

		} else if (!level17 && (int)distance == 9000) {
			forwardMovementSpeed += 0.2f;
			level16 = false;
			level17 = true;
			speed += 5;

#if UNITY_IPHONE
			ReportAchivement ("sekizbinbesyuzID");
#endif
#if UNITY_ANDROID
			ReportAchivement ("CgkI2YLQ_YUNEAIQIQ");
#endif

		} else if (!level18 && (int)distance == 9500) {
			forwardMovementSpeed += 0.2f;
			level17 = false;
			level18 = true;
			speed += 5;

#if UNITY_IPHONE
			ReportAchivement ("dokuzbinID");
#endif
#if UNITY_ANDROID
			ReportAchivement ("CgkI2YLQ_YUNEAIQGg");
#endif

		} else if (!level19 && (int)distance == 10000) {
			forwardMovementSpeed += 0.2f;
			level18 = false;
			level19 = true;
			speed += 5;

#if UNITY_IPHONE
			ReportAchivement ("dokuzbinesyuzID");
#endif
#if UNITY_ANDROID
			ReportAchivement ("CgkI2YLQ_YUNEAIQIg");
#endif

		} else if(!level20 && (int)distance == 49000){
			timeLeft = 1000.0f;
			forwardMovementSpeed += 0.2f;
			speed += 5;
			GOD_MODE = true;
			level19 = false;
			level20 = true;

#if UNITY_IPHONE
			ReportAchivement ("GodModID");
#endif
#if UNITY_ANDROID
			ReportAchivement ("CgkI2YLQ_YUNEAIQGw");
#endif


			SendScore ();

		}



		if (!dead) {
			if(!GOD_MODE)
				textDistance.text = ((int)distance).ToString ();

			textSpeed.text = speed.ToString ();
		}


	}



	public void OnPause(){

		if (GameData.Instance.isSoundON != 0)
			SoundManager.instance.PlaySingle(menuSelect);
		
		paused = true;

		SetSoundAndMusicPause ();

		EnableScoreTable (true);

		menuPanel.SetActive (false);
		jetpackAudio.enabled = false;
		footstepsAudio.enabled = false;
		jetpackAudio.volume = 0.0f;
		footstepsAudio.volume = 0.0f;
		pausePanel.SetActive (true);
		pauseButton.SetActive (false);
		Time.timeScale = 0;
	}

	public void StartNewGame(){
		Firebase.Analytics.FirebaseAnalytics
  .LogEvent(
	Firebase.Analytics.FirebaseAnalytics.EventJoinGroup,
	Firebase.Analytics.FirebaseAnalytics.ParameterGroupId,
	"start_new_game"
  );
		lifeCount = 3;
		speed = 50;
		forwardMovementSpeed = 4.0f;
		jetpackForce = 35;


		OnUnPause ();
	}

	public void ContinueGame(){

		Firebase.Analytics.FirebaseAnalytics
  .LogEvent(
	Firebase.Analytics.FirebaseAnalytics.EventJoinGroup,
	Firebase.Analytics.FirebaseAnalytics.ParameterGroupId,
	"continue"
  );
		
	}


	public void OnMenu(){
		paused = true;
		jetpackAudio.enabled = false;
		footstepsAudio.enabled = false;
		jetpackAudio.volume = 0.0f;
		footstepsAudio.volume = 0.0f;
		menuPanel.SetActive (true);
		pausePanel.SetActive (false);
		pauseButton.SetActive (false);

		EnableScoreTable (false);

		Time.timeScale = 0;

		LeanTween.alphaCanvas(textStart.gameObject.GetComponent<CanvasGroup>(), 0f, 0.7f) .setEase(LeanTweenType.easeInExpo).setOnComplete(FadeComplete).setIgnoreTimeScale(true);

	}

	void EnableScoreTable(bool enable){
		imgCoinsCount.gameObject.SetActive (enable);
		imgHighDistance.gameObject.SetActive (enable);
		imgtDistance.gameObject.SetActive (enable);
		imgSpeed.gameObject.SetActive (enable);
		imgLife.gameObject.SetActive (enable);
		textCoinsCount.gameObject.SetActive (enable);
		textHighDistance.gameObject.SetActive (enable);
        textHighDistanceM.gameObject.SetActive(enable);
        textDistance.gameObject.SetActive (enable);
        textDistanceM.gameObject.SetActive(enable);
        textSpeed.gameObject.SetActive (enable);
        textSpeedKMH.gameObject.SetActive(enable);
        textLife.gameObject.SetActive (enable);

	}

	public void FadeComplete(){
		LeanTween.alphaCanvas(textStart.gameObject.GetComponent<CanvasGroup>(), 1f, 0.7f) .setEase(LeanTweenType.easeInExpo).setOnComplete(FadeIn).setIgnoreTimeScale(true);
	}

	public void FadeIn(){
		LeanTween.alphaCanvas(textStart.gameObject.GetComponent<CanvasGroup>(), 0f, 0.7f) .setEase(LeanTweenType.easeInExpo).setOnComplete(FadeComplete).setIgnoreTimeScale(true);
	}

	public void PrepareToStart() {
		textContinue.gameObject.SetActive (false);
		gameOverPanel.SetActive (false);
		menuPanel.SetActive (false);
		EnableScoreTable (true);
		paused = false;
		jetpackAudio.enabled = true;
		footstepsAudio.enabled = true;
		jetpackAudio.volume = 1.0f;
		footstepsAudio.volume = 1.0f;
		pausePanel.SetActive (false);
		pauseButton.SetActive (true);
		textLife.text = lifeCount.ToString ();


		Time.timeScale = 1;
	}

	public void OnUnPause(){

		if (GameData.Instance.isSoundON != 0)
			SoundManager.instance.PlaySingle(menuSelect);

		PrepareToStart ();			
	}

	public void OnClickedRestartButton(){
		if (GameData.Instance.isSoundON != 0)
			SoundManager.instance.PlaySingle(menuSelect);

		if (lifeCount == 0)
		{
			if (this.interstitial.IsLoaded())
			{
				SoundManager.instance.StopMusic();
				this.interstitial.Show();
						Firebase.Analytics.FirebaseAnalytics
						.LogEvent(
						Firebase.Analytics.FirebaseAnalytics.EventJoinGroup,
						Firebase.Analytics.FirebaseAnalytics.ParameterGroupId,
						"intersititial_video"
			  );
			}
		}

		Firebase.Analytics.FirebaseAnalytics
		  .LogEvent(
			Firebase.Analytics.FirebaseAnalytics.EventJoinGroup,
			Firebase.Analytics.FirebaseAnalytics.ParameterGroupId,
			"game_restart"
		  );
		
		//		SceneManager.LoadScene(Application.loadedLevelName, LoadSceneMode.Single);
		Application.LoadLevel (Application.loadedLevelName);
	}


	void StoreHighscore(long newHighscore)
	{
		int oldHighscore = PlayerPrefs.GetInt("highscore", 0);    
		if (newHighscore > oldHighscore) {
			PlayerPrefs.SetInt ("highscore", (int)newHighscore);
			GameData.Instance.bestScore = (int)newHighscore;
			GameData.Instance.Save ();
		}

	}

	void DisplayCoinsCount()
	{
		if (coins == 500) {
#if UNITY_IPHONE
			ReportAchivement ("besyuzParaID");
#endif
#if UNITY_ANDROID
			ReportAchivement ("CgkI2YLQ_YUNEAIQCg");
#endif
		} else if (coins == 1500) {
#if UNITY_IPHONE
			ReportAchivement ("binParaID");
#endif
#if UNITY_ANDROID
			ReportAchivement ("CgkI2YLQ_YUNEAIQDg");
#endif
		} else if (coins == 1000 && !life1) {
			lifeCount = lifeCount + 1;
			life1 = true;
#if UNITY_IPHONE
			ReportAchivement ("life2000ID");
#endif
#if UNITY_ANDROID
			ReportAchivement ("CgkI2YLQ_YUNEAIQIw");
#endif
		} else if(coins == 2500) {
			
#if UNITY_IPHONE
			ReportAchivement ("binbesyuzParaID");
#endif
#if UNITY_ANDROID
			ReportAchivement ("CgkI2YLQ_YUNEAIQCw");
#endif
		} else if(coins == 3500) {
#if UNITY_IPHONE
			ReportAchivement ("ikibinparaID");
#endif
#if UNITY_ANDROID
			ReportAchivement ("CgkI2YLQ_YUNEAIQDA");
#endif

		} else if (coins == 2000 && !life2) {
			lifeCount = lifeCount + 1;
			life2 = true;
#if UNITY_IPHONE
			ReportAchivement ("life4000ID");
#endif
#if UNITY_ANDROID
			ReportAchivement ("CgkI2YLQ_YUNEAIQJA");
#endif

		} else if(coins == 4500) {
			
#if UNITY_IPHONE
			ReportAchivement ("ikibinbesyuzParaID");
#endif
#if UNITY_ANDROID
			ReportAchivement ("CgkI2YLQ_YUNEAIQDQ");
#endif

		} else if (coins == 3000 && !life3) {
			lifeCount = lifeCount + 1;
			life3 = true;
#if UNITY_IPHONE
			ReportAchivement ("lie6000ID");
#endif
#if UNITY_ANDROID
			ReportAchivement ("CgkI2YLQ_YUNEAIQJQ");
#endif

		} else if(coins == 5500) {
#if UNITY_IPHONE
			ReportAchivement ("ucbinparaID");
#endif
#if UNITY_ANDROID
			ReportAchivement ("CgkI2YLQ_YUNEAIQDw");
#endif
		} else if(coins == 6500) {
#if UNITY_IPHONE
			ReportAchivement ("ucbinbesyuzParaID");
#endif
#if UNITY_ANDROID
			ReportAchivement ("CgkI2YLQ_YUNEAIQEA");
#endif
		} else if (coins == 4000 && !life4) {
			lifeCount = lifeCount + 1;
			life4 = true;
#if UNITY_IPHONE
			ReportAchivement ("life8000ID");
#endif
#if UNITY_ANDROID
			ReportAchivement ("CgkI2YLQ_YUNEAIQJg");
#endif

		} else if(coins == 7500) {
#if UNITY_IPHONE
			ReportAchivement ("dortbinParaID");
#endif
#if UNITY_ANDROID
			ReportAchivement ("CgkI2YLQ_YUNEAIQEQ");
#endif
		} else if(coins == 5000 && !life5) {
			lifeCount = lifeCount + 1;
			life5 = true;
#if UNITY_IPHONE
			ReportAchivement ("life10000ID");
#endif
#if UNITY_ANDROID
			ReportAchivement ("CgkI2YLQ_YUNEAIQJw");
#endif
		}

		textCoinsCount.text = coins.ToString (); 
		textLife.text = lifeCount.ToString (); 
	}

	void ReportScore(long score, string leaderboardID)
	{
		Debug.Log("Reporting score " + score + " on leaderboard " + leaderboardID);
		if (Social.localUser.authenticated) {
			Social.ReportScore(score, leaderboardID, success => {
				Debug.Log(success ? "Reported score successfully" : "Failed to report score");
			});
		}
	}
		
	void DisplayGameOverResults()
	{
		textCoinsCountGameOver.text = coins.ToString (); 
		textLife.text = lifeCount.ToString ();
		CheckHighScoreGameOver ();
	}

	void AdjustJetpack (bool jetpackActive)
	{
		jetpack.enableEmission = !grounded;
		jetpack.emissionRate = jetpackActive ? 300.0f : 75.0f; 
	}

	void AdjustFootstepsAndJetpackSound(bool jetpackActive)    
	{
		footstepsAudio.enabled = !dead && grounded;

		jetpackAudio.enabled =  !dead && !grounded;
		jetpackAudio.volume = jetpackActive ? 1.0f : 0.5f;        
	}

	void CollectCoin(Collider2D coinCollider)
	{
		coins++;

		Destroy(coinCollider.gameObject);

		AudioSource.PlayClipAtPoint(coinCollectSound, transform.position);
	}

	void CollectSpeedUp(Collider2D coinCollider)
	{
		if (forwardMovementSpeed < 8.0f) {
			forwardMovementSpeed += 0.2f;
			speed += 5;
		}


		Destroy(coinCollider.gameObject);

		AudioSource.PlayClipAtPoint(coinCollectSound, transform.position);
	}

	void CollectSpeedDown(Collider2D coinCollider)
	{
		if (forwardMovementSpeed > 4.0f) {
			forwardMovementSpeed -= 0.2f;
			speed -= 5;
		}

		Destroy(coinCollider.gameObject);

		AudioSource.PlayClipAtPoint(coinCollectSound, transform.position);
	}

	void UpdateGroundedStatus()
	{
		//1
		grounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);

		//2
		animator.SetBool("grounded", grounded);
	}

	void DisplayRestartButton()
	{
		if (dead && grounded)
		{
			OnGameOver ();
		}
	}

	void SendScore(){
		long highScore = long.Parse(textDistance.text);
		long collectedCoins = long.Parse(textCoinsCount.text);
		GameData.Instance.totalCoins += (int)collectedCoins;
		GameData.Instance.Save ();

#if UNITY_IPHONE

		ReportScore(highScore, "HighestDistanceID");
		ReportScore(collectedCoins, "CollectedCoinsID");
		ReportScore(GameData.Instance.totalCoins, "TotalCollectedCoinsID");

#endif
#if UNITY_ANDROID

		ReportScore(highScore, "CgkI2YLQ_YUNEAIQAg");
		ReportScore(1000000*collectedCoins, "CgkI2YLQ_YUNEAIQAw");
		ReportScore(1000000*GameData.Instance.totalCoins, "CgkI2YLQ_YUNEAIQHQ");

#endif


		StoreHighscore (highScore);
		CheckHighScore ();
	}

	void OnGameOver() {

		footstepsAudio.enabled = !dead && grounded;

		GameData.Instance.Get ();

		animator.SetBool("rise", false);
		gameOver = true;

		if(lifeCount != 0)
			lifeCount = lifeCount - 1;
		
		SendScore ();

		pauseButton.gameObject.SetActive (false);
		DisplayGameOverResults ();
		EnableScoreTable (false);
		gameOverPanel.SetActive (true);
	}


	void CheckHighScore(){
		int highScore = int.Parse(textDistance.text);
		int oldHighscore = int.Parse(textHighDistance.text);

		if (highScore > oldHighscore) {
			textHighDistance.text = highScore.ToString ();
		}
	}

	void CheckHighScoreGameOver(){
		int highScore = int.Parse(textDistance.text);
		int oldHighscore = int.Parse(textHighDistance.text);

		if (highScore > oldHighscore) {
			textBestDistanceGameOver.text = highScore.ToString ();
		} else {
			textBestDistanceGameOver.text = oldHighscore.ToString ();
		}

		textDistanceGameOver.text = textDistance.text;
	}

	public void OnWatchAdVideo(){
		if (lifeCount > 0) {

			if (GameData.Instance.isSoundON != 0)
				SoundManager.instance.PlaySingle (menuSelect);


			if (this.rewardedAd.IsLoaded())
			{
				SoundManager.instance.StopMusic();
				this.rewardedAd.Show();
            }
            else
            {
				string message = "Rewarded video is not available at this time";
				dialogueManager = DialogueManager.Instance();
				dialogueManager.showDialog("Info", message, 100);
			}

			//if (HZIncentivizedAd.IsAvailable ()) {
			//	SoundManager.instance.StopMusic ();

			//	HZIncentivizedAd.Show ();
			//	HZIncentivizedAd.Fetch ();
			//} else {

			//string message = "Rewarded video is not available at this time";
			//	dialogueManager = DialogueManager.Instance ();
			//	dialogueManager.showDialog ("Info", message, 100);

			//}
		} else {
			string message = "Rewarded video cannot be used to continue.";
			dialogueManager = DialogueManager.Instance ();
			dialogueManager.showDialog ("Info", message, 100);
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.CompareTag ("coin")) {
			CollectCoin (collider);
		}
		else if (collider.gameObject.CompareTag ("speedup")) {
			CollectSpeedUp (collider);
		}
		else if (collider.gameObject.CompareTag ("speeddown")) {
			CollectSpeedDown (collider);
		}
		else {
			HitByLaser (collider);
		}
	}
		
	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag ("rocket")){
			HitByRocket (collision);
		}
	}

	void HitByRocket(Collision2D rocketCollision)
	{
		if (!GOD_MODE) {
			if (!dead)
				AudioSource.PlayClipAtPoint(explosionSound, transform.position);

			Instantiate (smoke, gameObject.transform.position, Quaternion.identity);
			Destroy (rocketCollision.gameObject);
//			rocketCollision.gameObject.SetActive (false);
			dead = true;
			animator.SetBool ("dead", true);
		}
	}

	void HitByLaser(Collider2D laserCollider)
	{
		if (!GOD_MODE) {
			if (!dead)
				laserCollider.gameObject.GetComponent<AudioSource> ().Play ();
		
			dead = true;
			animator.SetBool ("dead", true);
		}
	}

	void OnRiseAfterAdVideo(){
		RiseAgain ();
		PrepareToStart ();
		ContinueGame ();
	}

	void RiseAgain()
	{
		dead = false;
		paused = false;
		gameOver = false;
		animator.SetBool("dead", false);
		animator.SetBool("grounded", true);
		animator.SetBool("rise", true);
		animator.SetTrigger("dieOnceTrigger");
		GOD_MODE = true;
		timeLeft = 2.0f;
		if (GameData.Instance.isMusicON != 0) {
			SoundManager.instance.PlayMusic ();
		}

	}

	public void SetSoundAndMusic(){
		GameData.Instance.Get();

		if (GameData.Instance.isSoundON != 0) {
			sound.gameObject.SetActive (true);
			soundBan.gameObject.SetActive (false);
		} else {
			sound.gameObject.SetActive (false);
			soundBan.gameObject.SetActive (true);
		}

		if (GameData.Instance.isMusicON != 0) {
			music.gameObject.SetActive (true);
			musicBan.gameObject.SetActive (false);
			SoundManager.instance.PlayMusic();
		} else {
			music.gameObject.SetActive (false);
			musicBan.gameObject.SetActive (true);
			SoundManager.instance.StopMusic();
		}

	}

	public void SetSoundAndMusicPause(){
		GameData.Instance.Get();

		if (GameData.Instance.isSoundON != 0) {
			soundPause.gameObject.SetActive (true);
			soundBanPause.gameObject.SetActive (false);
		} else {
			soundPause.gameObject.SetActive (false);
			soundBanPause.gameObject.SetActive (true);
		}

		if (GameData.Instance.isMusicON != 0) {
			musicPause.gameObject.SetActive (true);
			musicBanPause.gameObject.SetActive (false);
		} else {
			musicPause.gameObject.SetActive (false);
			musicBanPause.gameObject.SetActive (true);
			SoundManager.instance.StopMusic();
		}

	}

	public void OnSoundClicked(){
		if (GameData.Instance.isSoundON != 0)
			SoundManager.instance.PlaySingle(menuSelect);

		GameData.Instance.isSoundON = 0;
		GameData.Instance.Save ();

		sound.gameObject.SetActive (false);
		soundBan.gameObject.SetActive (true);

	}

	public void OnSoundBanClicked(){
		if (GameData.Instance.isSoundON != 0)
			SoundManager.instance.PlaySingle(menuSelect);

		GameData.Instance.isSoundON = 1;
		GameData.Instance.Save ();

		sound.gameObject.SetActive (true);
		soundBan.gameObject.SetActive (false);

	}

	public void OnMusicClicked(){
		if (GameData.Instance.isSoundON != 0)
			SoundManager.instance.PlaySingle(menuSelect);

		GameData.Instance.isMusicON = 0;
		GameData.Instance.Save ();

		SoundManager.instance.StopMusic();

		music.gameObject.SetActive (false);
		musicBan.gameObject.SetActive (true);
	}

	public void OnMusicBanClicked(){
		if (GameData.Instance.isSoundON != 0)
			SoundManager.instance.PlaySingle(menuSelect);

		GameData.Instance.isMusicON = 1;
		GameData.Instance.Save ();

		SoundManager.instance.PlayMusic();

		music.gameObject.SetActive (true);
		musicBan.gameObject.SetActive (false);

	}

	public void OnSoundClickedPause(){
		if (GameData.Instance.isSoundON != 0)
			SoundManager.instance.PlaySingle(menuSelect);

		GameData.Instance.isSoundON = 0;
		GameData.Instance.Save ();

		soundPause.gameObject.SetActive (false);
		soundBanPause.gameObject.SetActive (true);

	}

	public void OnSoundBanClickedPause(){
		if (GameData.Instance.isSoundON != 0)
			SoundManager.instance.PlaySingle(menuSelect);

		GameData.Instance.isSoundON = 1;
		GameData.Instance.Save ();

		soundPause.gameObject.SetActive (true);
		soundBanPause.gameObject.SetActive (false);

	}

	public void OnMusicClickedPause(){
		if (GameData.Instance.isSoundON != 0)
			SoundManager.instance.PlaySingle(menuSelect);

		GameData.Instance.isMusicON = 0;
		GameData.Instance.Save ();

		SoundManager.instance.StopMusic();

		musicPause.gameObject.SetActive (false);
		musicBanPause.gameObject.SetActive (true);
	}

	public void OnMusicBanClickedPause(){
		if (GameData.Instance.isSoundON != 0)
			SoundManager.instance.PlaySingle(menuSelect);

		GameData.Instance.isMusicON = 1;
		GameData.Instance.Save ();

		SoundManager.instance.PlayMusic();

		musicPause.gameObject.SetActive (true);
		musicBanPause.gameObject.SetActive (false);

	}

}
