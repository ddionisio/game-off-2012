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
		
		UIManager.instance.hud.wave.SetWave(mCurWave,numWave);
	}
	
	public string GetWaveString() {
		return string.Format(WaveStringFormat, mCurWave, numWave);
	}
	
	protected override void SequenceChangeState (string state) {
		base.SequenceChangeState (state);
	}
	
	protected override void Awake() {
		mPlayer = GetComponentInChildren<Player>();
		mPlanet = GetComponentInChildren<Planet>();
		
		base.Awake();
	}
	
	protected override void Start() {
		UIManager.instance.hud.gameObject.SetActiveRecursively(true);
		
		mCurWave = 0;
		
		UIManager.instance.hud.combo.gameObject.SetActiveRecursively(false);
		
		UIManager.instance.hud.wave.SetWave(0,numWave);
		
		UIManager.instance.hud.score.score = 0;
		
		UIManager.instance.hud.playerStatus.SetStats(mPlayer.stats);
		UIManager.instance.hud.bossStatus.gameObject.SetActiveRecursively(false); //...
		
		UIManager.instance.hud.pointer.SetPOI(null);
		
		base.Start();
	}
	
	public void SceneShutdown() {
		//
		PlayerPrefs.Save();
	}
	
	//calls from entity manager
	public void OnEntitySpawn(Entity e) {
	}
	
	public void OnEntityRelease(Entity e) {
	}
	
	void Update() {
		if(Input.GetButtonDown("Menu")) {
			UIManager uimgr = UIManager.instance;
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
