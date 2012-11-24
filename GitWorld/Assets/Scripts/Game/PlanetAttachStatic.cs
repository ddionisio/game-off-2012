using UnityEngine;
using System.Collections;

public class PlanetAttachStatic : MonoBehaviour {
	
	[System.NonSerialized]
	public PlanetBody planet = null;
	
	public float radius;
	
	[SerializeField]
	tk2dBaseSprite orientSprite = null;
	
	protected Transform mTrans;
	
	private Vector2 mPlanetPos; //relative to planet's surface
	
	private Vector2 mPlanetDir; //facing direction, relative to planet's surface
	
	public Vector2 planetPos {
		get {
			return mPlanetPos;
		}
		set {
			if(value != mPlanetPos) {
				mPlanetDir = value - mPlanetPos;
				mPlanetDir.Normalize();
				
				//default facing left
				if(orientSprite != null) {
					Vector3 s = orientSprite.scale;
					s.x = mPlanetDir.x > 0.0f ? 1 : -1;
					orientSprite.scale = s;
				}
				
				mPlanetPos = value;
				
				//wrap position
				mPlanetPos.x %= planet.surfaceLength;
				if(mPlanetPos.x < 0) {
					mPlanetPos.x += planet.surfaceLength;
				}
				
				//adjust to ground
				if(planetPos.y < radius) {
					mPlanetPos.y = radius;
					OnAdjustToGround();
				}
			}
		}
	}
	
	public Vector2 planetDir {
		get {
			return mPlanetDir;
		}
	}
		
	public Vector2 ConvertToPlanetDir(Vector3 dir) {
		//ew
		Vector2 _dpos = transform.position + dir;
		Vector2 _plpos = planet.ConvertToPlanetPos(_dpos);
		Vector2 _dir = _plpos-mPlanetPos;
		_dir.Normalize();
		
		return _dir;
	}
	
	public void RefreshPos() {
		if(planet != null && mTrans != null) {
			mPlanetPos = planet.ConvertToPlanetPos(mTrans.position);
		}
	}
	
	public Util.Side CheckSide(PlanetAttachStatic against) {
		float x=mPlanetPos.x, xAgainst = against.mPlanetPos.x;
		float len1 = x - xAgainst;
		float len2 = x - (planet.surfaceLength-xAgainst);
		float d;
		if(Mathf.Abs(len1) < Mathf.Abs(len2)) {
			d = len1;
		}
		else {
			d = len2;
		}
		
		return d == 0.0f ? Util.Side.None : d < 0.0f ? Util.Side.Right : Util.Side.Left;
	}
	
	public Vector2 GetDirTo(PlanetAttachStatic target) {
		float x;
		switch(CheckSide(target)) {
		case Util.Side.Left:
			x = -1;
			break;
		case Util.Side.Right:
			x = 1;
			break;
		default:
			x = 0;
			break;
		}
		
		return (new Vector2(x, target.mPlanetPos.y - mPlanetPos.y)).normalized;
	}
	
	protected virtual void OnAdjustToGround() {
	}
	
	protected virtual void OnEnable() {
		Start();
	}
	
	protected virtual void Awake() {
		SphereCollider sc = GetComponentInChildren<SphereCollider>();
		if(sc != null) {
			radius = sc.radius;
		}
				
		mTrans = transform;
	}
	
	protected virtual void Start() {
		if(planet == null && SceneLevel.instance != null && SceneLevel.instance.planet != null)
			planet = SceneLevel.instance.planet.body;
		
		RefreshPos();
	}
	
	void SceneStart() {
		Start();
	}
}
