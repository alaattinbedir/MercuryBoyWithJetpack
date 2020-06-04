using UnityEngine;
using System.Collections;

/**
 * Used just to pass information from one scene to the next
 */

public class GamePlay {

	public bool paused;

	private static GamePlay _instance = null;

	private GamePlay() {
		// Anything to init would go here
	}

	public static GamePlay Instance {
		get {
			if (_instance == null) {
				_instance = new GamePlay();
			}
			return _instance;
		}
	}
}

