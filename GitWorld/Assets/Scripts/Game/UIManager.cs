using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {
	public enum Modal {
		Start,
		GameOptions,
		LevelSelect,
		HowToPlay,
		Victory,
		
		NumModal
	}
	
	[System.Serializable]
	public class UIData {
		public string name;
		public UIController ui;
		public bool exclusive = true; //hide modals behind
	}
	
	public UIData[] uis;
			
	private Stack<UIData> mModalStack = new Stack<UIData>((int)Modal.NumModal);
	
	//closes all modal and open this
	public void ModalReplace(Modal modal) {
		ModalClearStack(false);
		ModalPushToStack(modal, false);
		
	}
	
	public void ModalOpen(Modal modal) {
		ModalPushToStack(modal, true);
	}
			
	public void ModalCloseTop() {
		if(mModalStack.Count > 0) {
			UIData uid = mModalStack.Pop();
			UIController ui = uid.ui;
			ui.OnClose();
			ui.gameObject.SetActiveRecursively(false);
			
			if(mModalStack.Count == 0) {
				ModalInactive();
			}
			else {
				//re-show top
				UIData prevUID = mModalStack.Peek();
				UIController prevUI = prevUID.ui;
				if(!prevUI.gameObject.active) {
					prevUI.gameObject.SetActiveRecursively(true);
					prevUI.OnShow(true);
				}
			}
		}
	}
	
	public void ModalCloseAll() {
		ModalClearStack(true);
	}
	
	void ModalPushToStack(Modal modal, bool evokeActive) {
		if(evokeActive && mModalStack.Count == 0) {
			SceneController sc = Main.instance.sceneController;
			if(sc != null) {
				sc.BroadcastMessage("OnUIModalActive", null, SendMessageOptions.DontRequireReceiver);
			}
		}
		
		
		
		UIData uid = uis[(int)modal];
		
		if(uid.exclusive && mModalStack.Count > 0) {
			//hide below
			UIData prevUID = mModalStack.Peek();
			UIController prevUI = prevUID.ui;
			prevUI.OnShow(false);
			prevUI.gameObject.SetActiveRecursively(false);
		}
		
		UIController ui = uid.ui;
		ui.gameObject.SetActiveRecursively(true);
		ui.OnOpen();
		ui.OnShow(true);
		
		mModalStack.Push(uid);
	}
	
	void ModalClearStack(bool evokeInactive) {
		if(mModalStack.Count > 0) {
			foreach(UIData uid in mModalStack) {
				UIController ui = uid.ui;
				ui.OnClose();
				ui.gameObject.SetActiveRecursively(false);
			}
			
			mModalStack.Clear();
			
			if(evokeInactive) {
				ModalInactive();
			}
		}
	}
	
	void ModalInactive() {
		SceneController sc = Main.instance.sceneController;
		if(sc != null) {
			sc.BroadcastMessage("OnUIModalInactive", null, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	void SceneShutdown() {
		ModalCloseAll();
	}
	
	void Awake() {
		//deactivate all uis
		foreach(UIData uid in uis) {
			UIController ui = uid.ui;
			if(ui != null) {
				ui.gameObject.SetActiveRecursively(false);
			}
		}
	}
}
