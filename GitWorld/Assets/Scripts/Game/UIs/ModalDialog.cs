using UnityEngine;
using System.Collections;

public class ModalDialog : UIController {
	[SerializeField] UISprite portraitWidget; //around 60x60
	[SerializeField] UILabel nameWidget;
	[SerializeField] UILabel content;
	
	[SerializeField] UIEventListener clickArea;
	
	private string[] mPages;
	private int mCurPage=0;
	
	public static void Open(string portraitRef, string name, string[] pages) {
		if(Main.instance.uiManager.ModalGetTop() != UIManager.Modal.Dialog) {
			Main.instance.uiManager.ModalOpen(UIManager.Modal.Dialog);
		}
		
		//...
		ModalDialog us = (ModalDialog)Main.instance.uiManager.uis[(int)UIManager.Modal.Dialog].ui;
		
		us.mPages = pages;
		us.mCurPage = 0;
		
		UIAtlas.Sprite spr = us.portraitWidget.atlas.GetSprite(portraitRef);
		if(spr != null) {
			us.portraitWidget.sprite = spr;
			us.portraitWidget.MakePixelPerfect();
		}
		
		us.nameWidget.text = name;
		
		us.content.text = pages[0];
	}
	
	void OnPageClick(GameObject go) {
		mCurPage++;
		if(mPages == null || mCurPage == mPages.Length) {
			Main.instance.uiManager.ModalCloseTop();
		}
		else {
			content.text = mPages[mCurPage];
		}
	}
		
	void Awake() {
		clickArea.onClick += OnPageClick;
	}
	
	//TODO: fancy open
	
	public override void OnClose() {
		mPages = null;
	}
}
