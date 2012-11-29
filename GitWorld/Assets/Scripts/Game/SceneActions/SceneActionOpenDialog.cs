using UnityEngine;
using System.Collections;

public class SceneActionOpenDialog : SequencerAction {
	public string name;
	public string portrait;
	public string[] texts;

	public override void Start(MonoBehaviour behaviour, Sequencer.StateInstance state) {	
		ModalDialog.Open(portrait, name, texts);
	}
	
	public override bool Update(MonoBehaviour behaviour, Sequencer.StateInstance state) {
		return !Main.instance.uiManager.ModalIsInStack(UIManager.Modal.Dialog);
	}
}