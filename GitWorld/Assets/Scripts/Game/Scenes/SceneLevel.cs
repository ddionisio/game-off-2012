using UnityEngine;
using System.Collections;

public class SceneLevel : SceneController {
	[System.NonSerialized]
	public int enemyCount;
	
	
	/// <summary>
	/// Only use this for components that are in-game related. Only available after awake/level loaded.
	/// </summary>
	public static SceneLevel instance {
		get {
			return Main.instance != null && Main.instance.sceneController != null ? (SceneLevel)Main.instance.sceneController : null;
		}
	}
	
	public Planet planet {
		get {
			return mPlanet;
		}
	}
	
	public Player player {
		get {
			return mPlayer;
		}
	}
			
	private Player mPlayer;
	private Planet mPlanet;
	
	void SceneStart() {
		mPlayer = GetComponentInChildren<Player>();
		mPlanet = GetComponentInChildren<Planet>();
		
		Main.instance.uiManager.hud.gameObject.SetActiveRecursively(true);
		Main.instance.uiManager.hud.playerStatus.SetStats(mPlayer.stats);
	}
	
	public void SceneShutdown() {
		Main.instance.uiManager.hud.playerStatus.SetStats(null);
		Main.instance.uiManager.hud.gameObject.SetActiveRecursively(false);
	}
	
	//calls from entity manager
	public void OnEntitySpawn(Entity e) {
	}
	
	public void OnEntityRelease(Entity e) {
	}
}
