using UnityEngine;
using System.Collections;

public class SceneActionOpenModal : SequencerAction {
	public UIManager.Modal modal;
	public bool waitForClose = false;

	public override void Start(MonoBehaviour behaviour) {	
		Main.instance.uiManager.ModalOpen(modal);
	}
	
	public override bool Update(MonoBehaviour behaviour) {
		return !(waitForClose && Main.instance.uiManager.ModalIsInStack(modal));
	}
}
