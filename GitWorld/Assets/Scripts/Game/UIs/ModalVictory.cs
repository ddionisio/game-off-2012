using UnityEngine;
using System.Collections;

public class ModalVictory : UIController {
	public UIEventListener buttonReturn;
	
	void OnButtonReturn(GameObject go) {
		Main.instance.sceneManager.LoadScene(SceneManager.Scene.start);
	}

	void Awake() {
		buttonReturn.onClick += OnButtonReturn;
	}
}
