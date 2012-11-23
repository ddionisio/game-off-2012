using UnityEngine;
using System.Collections;

public class EntityStats : MonoBehaviour {
	public int maxHP = 1;
	
	public int damage = 1;
	
	private int mCurHP;
	
	public int curHP {
		get {
			return mCurHP;
		}
	}
	
	public void ApplyDamage(EntityStats src) {
		if(src != null) {
			mCurHP -= src.damage;
			if(mCurHP < 0) {
				mCurHP = 0;
			}
		}
	}
	
	public void ResetStats() {
		mCurHP = maxHP;
	}
	
	void Awake() {
		ResetStats();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
