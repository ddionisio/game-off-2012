using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUDEntityStatus : MonoBehaviour {
	public Transform hitpointTemplate;
	
	public Transform container; //this is where we put the hearts
	public Transform cacheContainer; //this is where we put cache
	
	public NGUILayoutAnchor containerFrameLayout; //the frame around the hearts
	public NGUILayoutFlow containerLayout; //the one that holds the hearts
	
	public UILabel nameWidget;
	public UISprite portraitWidget;
	
	private List<HUDHitPoint> mHPs = new List<HUDHitPoint>();
	private EntityStats mStats;
	private int mCurHP;
	
	public void SetStats(EntityStats stats) {
		mStats = stats;
		
		if(stats == null) {
			Clear();
		}
		else if(gameObject.active) { //refresh later
			RefreshStats(true);
		}
	}
	
	public void RefreshStats(bool refreshHP) {
		if(refreshHP) {
			if(mStats.maxHP != mHPs.Count) {
				Clear();
				
				for(int i = 0; i < mStats.maxHP; i++) {
					Transform hpTrans;
					if(cacheContainer.GetChildCount() > 0) {
						hpTrans = cacheContainer.GetChild(0);
					}
					else {
						hpTrans = (Transform)Object.Instantiate(hitpointTemplate);
					}
					
					hpTrans.parent = container;
					hpTrans.localPosition = Vector3.zero;
					hpTrans.localRotation = Quaternion.identity;
					hpTrans.localScale = Vector3.one;
					hpTrans.gameObject.SetActiveRecursively(true);
					
					mHPs.Add(hpTrans.GetComponent<HUDHitPoint>());
				}
				
				if(containerLayout != null) {
					containerLayout.Reposition();
				}
				
				if(containerFrameLayout != null) {
					containerFrameLayout.Reposition();
				}
			}
			
			int curHPInd = 0;
			for(; curHPInd < mStats.curHP; curHPInd++) {
				mHPs[curHPInd].SetOn(true);
			}
			
			for(; curHPInd < mStats.maxHP; curHPInd++) {
				mHPs[curHPInd].SetOn(false);
			}
			
			mCurHP = mStats.curHP;
		}
		
		if(nameWidget != null) {
			nameWidget.text = mStats.displayName;
		}
		
		if(portraitWidget != null) {
			UIAtlas.Sprite spr = portraitWidget.atlas.GetSprite(mStats.portrait);
			if(spr != null) {
				portraitWidget.sprite = spr;
			}
		}
	}
	
	void OnEnable() {
		if(mStats != null) {
			RefreshStats(true);
		}
		else {
			cacheContainer.gameObject.SetActiveRecursively(false);
			Clear();
		}
	}
	
	void OnDisable() {
	}
	
	void Awake() {
		cacheContainer.gameObject.SetActiveRecursively(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(mStats != null && mStats.curHP != mCurHP) {
			if(mCurHP > mStats.curHP) { //decrease
				for(int i = mCurHP-1; i >= mStats.curHP; i--) {
					mHPs[i].SetOn(false);
				}
			}
			else { //increase
				for(int i = mCurHP; i < mStats.curHP; i++) {
					mHPs[i].SetOn(true);
				}
			}
			
			mCurHP = mStats.curHP;
		}
	}
	
	void Clear() {
		foreach(HUDHitPoint hp in mHPs) {
			hp.transform.parent = cacheContainer;
			hp.gameObject.SetActiveRecursively(false);
		}
		
		mHPs.Clear();
	}
}
