using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class UILevelSelect : UIController {
	public UIEventListener[] buttonLevels;
	
	public UIEventListener buttonBack;
	
	void OnButtonLevel(GameObject go) {
		//assumes name has a number: {0,1,2...N}
		string numStr = Regex.Match(go.name, @"\d+").Value;
		int level;
		if(int.TryParse(numStr, out level)) {
			Main.instance.sceneManager.LoadLevel(level);
		}
	}
	
	void Awake() {
		foreach(UIEventListener uie in buttonLevels) {
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
