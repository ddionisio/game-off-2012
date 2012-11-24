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
		
		Main.instance.uiManager.hud.score.score = 0;
		
		Main.instance.uiManager.hud.playerStatus.SetStats(mPlayer.stats);
		Main.instance.uiManager.hud.bossStatus.gameObject.SetActiveRecursively(false); //...
	}
	
	public void SceneShutdown() {
		//
		Main.instance.uiManager.hud.score.Clear();
		Main.instance.uiManager.hud.wave.SetWave(0,0);
		
		Main.instance.uiManager.hud.playerStatus.SetStats(null);
		Main.instance.uiManager.hud.bossStatus.SetStats(null);
		
		Main.instance.uiManager.hud.gameObject.SetActiveRecursively(false);
	}
	
	//calls from entity manager
	public void OnEntitySpawn(Entity e) {
	}
	
	public void OnEntityRelease(Entity e) {
	}
	
	void Update() {
		if(Input.GetButtonDown("Menu")) {
			UIManager uimgr = Main.instance.uiManager;
			if(uimgr.ModalGetTop() == UIManager.Modal.GameOptions) {
				Main.instance.sceneManager.Resume();
				uimgr.ModalCloseTop();
			}
			else if(uimgr.ModalGetTop() == UIManager.Modal.NumModal) {
				Main.instance.sceneManager.Pause();
				uimgr.ModalOpen(UIManager.Modal.GameOptions);
			}
		}
	}
}
