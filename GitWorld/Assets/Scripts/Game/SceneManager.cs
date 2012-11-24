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
	private string mCurLevel;
	private float mPrevTimeScale;
	
	public SceneController sceneController {
		get {
			//TODO: automagically create a controller?
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
		mCurLevel = levelString+level;
		LoadScene(mCurLevel);
	}
	
	public void ReloadLevel() {
		if(!string.IsNullOrEmpty(mCurLevel)) {
			LoadScene(mCurLevel);
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
			if(mSceneController != null) {
				mSceneController.BroadcastMessage("SceneStart", null, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	
	void OnLevelWasLoaded(int sceneInd) {
		InitScene();
	}
	
	void Awake() {
		mPrevTimeScale = Time.timeScale;
	}
}
