using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {
	
	public enum Scene {
		main,
		start,
		test
	}
	
	public const string levelString = "level";
	
	private SceneController mSceneController;
	private string mCurLevelStr;
	private int mCurLevel;
	private float mPrevTimeScale;
	
	public int curLevel {
		get {
			return mCurLevel;
		}
	}
	
	public SceneController sceneController {
		get {
			return mSceneController;
		}
	}
	
	//TODO: transitions
		
	public void LoadScene(Scene scene) {
		LoadScene(scene.ToString());
	}
	
	public void LoadScene(string scene) {
		Main.instance.BroadcastMessage("SceneShutdown", null, SendMessageOptions.DontRequireReceiver);
		
		if(mSceneController != null) {
			mSceneController.BroadcastMessage("SceneShutdown", null, SendMessageOptions.DontRequireReceiver);
			mSceneController = null;
		}
		
		Application.LoadLevel(scene);
	}
	
	public void LoadLevel(int level) {
		mCurLevel = level;
		mCurLevelStr = levelString+level;
		LoadScene(mCurLevelStr);
	}
	
	public void ReloadLevel() {
		if(!string.IsNullOrEmpty(mCurLevelStr)) {
			LoadScene(mCurLevelStr);
		}
	}
	
	public void Pause() {
		if(Time.timeScale != 0.0f) {
			mPrevTimeScale = Time.timeScale;
			Time.timeScale = 0.0f;
		}
		
		BroadcastMessage("OnScenePause", null, SendMessageOptions.DontRequireReceiver);
	}
	
	public void Resume() {
		Time.timeScale = mPrevTimeScale;
		
		BroadcastMessage("OnSceneResume", null, SendMessageOptions.DontRequireReceiver);
	}
	
	/// <summary>
	/// Internal use only. Called at OnLevelWasLoaded in SceneManager or in Main (for debug/dev)
	/// </summary>
	public void InitScene() {
		if(mSceneController == null) {
			mSceneController = (SceneController)Object.FindObjectOfType(typeof(SceneController));
		}
	}
	
	void OnLevelWasLoaded(int sceneInd) {
		InitScene();
	}
	
	void Awake() {
		mPrevTimeScale = Time.timeScale;
	}
}
