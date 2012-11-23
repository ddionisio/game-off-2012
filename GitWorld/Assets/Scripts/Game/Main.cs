using UnityEngine;
using System.Collections;

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
			
	public CameraController cameraController;
	
	public UserSettings userSettings;
	public UserData userData;
	
	public SceneManager sceneManager;
	public ReticleManager reticleManager;
	public UIManager uiManager;
	
	private static Main mInstance = null;
	
	public static Main instance {
		get {
			return mInstance;
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
}
