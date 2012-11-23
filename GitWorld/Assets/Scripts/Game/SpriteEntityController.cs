using UnityEngine;
using System.Collections;

//attach these to sprites you want to be affected by entity
public class SpriteEntityController : MonoBehaviour, Entity.IListener {
	public float invulBlinkDelay = 0.05f;
	
	private tk2dBaseSprite mSprite;
	private tk2dAnimatedSprite mSpriteAnim;
	private Color mPrevColor;
	private int[] mActionAnimIds;
	
	private bool mInvulDoBlink = false;
	private float mInvulCurTime = 0.0f;
	
	void Awake() {
		mSprite = GetComponent<tk2dBaseSprite>();
		mSpriteAnim = mSprite as tk2dAnimatedSprite;
	}
	
	void Start() {
		if(mSpriteAnim != null) {
			mActionAnimIds = new int[(int)Entity.Action.NumActions];
			for(int i = 0; i < mActionAnimIds.Length; i++) {
				mActionAnimIds[i] = mSpriteAnim.GetClipIdByName(((Entity.Action)i).ToString());
			}
		}
	}
	
	void Update() {
		if(mInvulDoBlink) {
			mInvulCurTime += Time.deltaTime;
			if(mInvulCurTime >= invulBlinkDelay) {
				mInvulCurTime = 0.0f;
				
				Color c = mSprite.color;
				c.a = c.a == 0.0f ? mPrevColor.a : 0.0f;
				mSprite.color = c;
			}
		}
	}

	public void OnEntityAct(Entity.Action act) {
		if(mSpriteAnim != null) {
			int id = mActionAnimIds[(int)act];
			if(id != -1) {
				mSpriteAnim.Play(id);
			}
		}
	}
	
	public void OnEntityInvulnerable(bool yes) {
		mInvulDoBlink = yes;
		if(yes) {
			mPrevColor = mSprite.color;
			mInvulCurTime = 0.0f;
		}
		else {
			mSprite.color = mPrevColor;
		}
	}
	
	public void OnEntityCollide(Entity other, bool youAreReceiver) {
	}
	
	public void OnEntitySpawnFinish() {
	}
}
