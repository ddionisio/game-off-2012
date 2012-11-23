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
		LoadScene(levelString+level);
	}
	
	public void InitScene() {
		if(mSceneController == null) {
			mSceneController = (SceneController)Object.FindObjectOfType(typeof(SceneController));
		}
	}
	
	void OnLevelWasLoaded(int sceneInd) {
		InitScene();
	}
	
	void Awake() {
	}
}
