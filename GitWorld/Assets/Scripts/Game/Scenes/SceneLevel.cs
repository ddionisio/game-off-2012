using UnityEngine;
using System.Collections;

public class SceneLevel : SceneController {
	public const string WaveStringFormat = "CONFLICT {0}-{1}";
	
	[SerializeField] int numWave;
	[SerializeField] Transform enemiesHolder;
		
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
	
	public int enemyCount {
		get {
			return enemiesHolder.childCount;
		}
	}
			
	private Player mPlayer;
	private Planet mPlanet;
	private int mCurWave = 0;
	
	public void IncWave() {
		if(mCurWave < numWave) {
			mCurWave++;
		}
		
		Main.instance.uiManager.hud.wave.SetWave(mCurWave,numWave);
	}
	
	public string GetWaveString() {
		return string.Format(WaveStringFormat, mCurWave, numWave);
	}
	
	void SceneStart() {
		mPlayer = GetComponentInChildren<Player>();
		mPlanet = GetComponentInChildren<Planet>();
		
		Main.instance.uiManager.hud.gameObject.SetActiveRecursively(true);
		
		mCurWave = 0;
		
		Main.instance.uiManager.hud.wave.SetWave(0,numWave);
		
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
