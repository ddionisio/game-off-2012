using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class ModalLevelSelect : UIController {
	public UIEventListener buttonTutorial;
	
	[System.Serializable]
	public class LevelButton {
		public GameObject lockObject;
		public GameObject completeObject;
		public UIButton button;
	}
	
	public LevelButton[] buttonLevels;
	
	public UIEventListener buttonBack;
	
	public override void OnShow(bool show) {
		if(show) {
			//determine button states
			for(int i = 0; i < buttonLevels.Length; i++) {
				LevelButton lb = buttonLevels[i];
				UIButton btn = lb.button;
				
				if(btn != null) {
					UserData.LevelState ls = Main.instance.userData.GetLevelState(i);
					switch(ls) {
					case UserData.LevelState.Unlocked:
						lb.lockObject.SetActiveRecursively(false);
						lb.completeObject.SetActiveRecursively(false);
						btn.isEnabled = true;
						break;
						
					case UserData.LevelState.Complete: //show star around
						lb.lockObject.SetActiveRecursively(false);
						btn.isEnabled = true;
						break;
						
					case UserData.LevelState.Locked:
						lb.completeObject.SetActiveRecursively(false);
						btn.isEnabled = false;
						break;
					}
				}
			}
		}
	}
	
	void OnButtonTutorial(GameObject go) {
		Main.instance.sceneManager.LoadScene(SceneManager.Scene.tutorial);
	}
	
	void OnButtonLevel(GameObject go) {
		//assumes name has a number: {0,1,2...N}
		string numStr = Regex.Match(go.name, @"\d+").Value;
		int level;
		if(int.TryParse(numStr, out level)) {
			Main.instance.sceneManager.LoadLevel(level);
		}
	}
			
	void Awake() {
		buttonTutorial.onClick += OnButtonTutorial;
		
		foreach(LevelButton lb in buttonLevels) {
			UIEventListener uie = lb.button.GetComponent<UIEventListener>();
			uie.onClick += OnButtonLevel;
		}
		
		buttonBack.onClick += UICommonCallbacks.OnButtonClose;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
