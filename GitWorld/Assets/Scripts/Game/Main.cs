using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {
	//only use these after awake
	public static int layerPlayer;
	public static int layerEnemy;
	public static int layerProjectile;
	public static int layerItem;
	public static int layerPlayerProjectile;
	
	public static int layerMaskPlayer;
	public static int layerMaskEnemy;
	public static int layerMaskProjectile;
	public static int layerMaskItem;
	public static int layerMaskPlayerProjectile;
	
	public TextAsset stringAsset;
	
	[System.NonSerialized] public CameraController cameraController;
	[System.NonSerialized] public UserSettings userSettings;
	[System.NonSerialized] public UserData userData;
	[System.NonSerialized] public SceneManager sceneManager;
	[System.NonSerialized] public ReticleManager reticleManager;
	[System.NonSerialized] public UIManager uiManager;
	
	private static Main mInstance = null;
	
	private Dictionary<string, object> mStringTable = null;
	
	public static Main instance {
		get {
			return mInstance;
		}
	}
	
	public Dictionary<string, object> strings {
		get {
			if(mStringTable == null) {
				InitStrings();
			}
			
			return mStringTable;
		}
	}
	
	public SceneController sceneController {
		get {
			return sceneManager.sceneController;
		}
	}
	
	void OnApplicationQuit() {
		mInstance = null;
	}
			
	void Awake() {
		mInstance = this;
		
		layerPlayer = LayerMask.NameToLayer("Player");
		layerEnemy = LayerMask.NameToLayer("Enemy");
		layerProjectile = LayerMask.NameToLayer("Projectile");
		layerItem = LayerMask.NameToLayer("Item");
		layerPlayerProjectile = LayerMask.NameToLayer("PlayerProjectile");
		
		layerMaskPlayer = 1<<layerPlayer;
		layerMaskEnemy = 1<<layerEnemy;
		layerMaskProjectile = 1<<layerProjectile;
		layerMaskItem = 1<<layerItem;
		layerMaskPlayerProjectile = 1<<layerPlayerProjectile;
		
		DontDestroyOnLoad(gameObject);
		
		userData = GetComponentInChildren<UserData>();
		userSettings = GetComponentInChildren<UserSettings>();
		
		cameraController = GetComponentInChildren<CameraController>();
		
		sceneManager = GetComponentInChildren<SceneManager>();
		reticleManager = GetComponentInChildren<ReticleManager>();
		uiManager = GetComponentInChildren<UIManager>();
		
		InitStrings();
	}
	
	void Start() {
		//TODO: maybe do other things before starting the game
		//go to start if we are in main scene
		SceneManager.Scene mainScene = SceneManager.Scene.main;
		if(Application.loadedLevelName == mainScene.ToString()) {
			sceneManager.LoadScene(SceneManager.Scene.start);
		}
		else {
			sceneManager.InitScene();
		}
	}
	
	void InitStrings() {
		if(mStringTable == null) {
			if(stringAsset != null) {
				mStringTable = fastJSON.JSON.Instance.Parse(stringAsset.text) as Dictionary<string, object>;
			}
		}
	}
}
