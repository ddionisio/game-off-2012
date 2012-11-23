using UnityEngine;
using System.Collections;

public class SceneStart : SceneController {

	// Use this for initialization
	protected override void Start () {
		base.Start();
		
		Main.instance.uiManager.ModalOpen(UIManager.Modal.Start);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
