using UnityEngine;
using System.Collections;

public class SceneMainMenu : SceneController {

	// Use this for initialization
	void SceneStart() {
		Main.instance.uiManager.hud.gameObject.SetActiveRecursively(false);
		Main.instance.uiManager.ModalOpen(UIManager.Modal.Start);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
