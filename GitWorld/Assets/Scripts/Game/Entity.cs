using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {
	public enum Action {
		idle,
		spawning, //once finish, calls OnEntitySpawnFinish to listeners
		reviving,
		revived,
		hurt,
		die,
		move,
		attack,
		jump,
		grabbed,
		stunned,
		
		NumActions
	}
	
	[System.Flags]
	public enum Flag : int {
		None = 0x0,
		Targetted = 0x1,
		Invulnerable = 0x2
	}
	
	public interface IListener {
		void OnEntityAct(Action act);
		void OnEntityInvulnerable(bool yes);
		void OnEntityCollide(Entity other, bool youAreReceiver);
		void OnEntitySpawnFinish();
	}
	
	public float spawnDelay = 1.0f;
	
	protected Reticle.Type mReticle = Reticle.Type.NumType;
	protected int mCollideLayerMask = 0; //0 is none, only initialize on start
	
	private Flag mFlags = Flag.None;
	
	private Action mCurAct = Action.NumActions;
	private Action mPrevAct = Action.NumActions;
	
	private EntityStats mStats;
	private PlanetAttach mPlanetAttach;
	
	private float mEntCurTime = 0;
	
	private float mInvulDelay = 0;
	
	private IListener[] mListeners;
	
	public Reticle.Type reticle {
		get {
			return mReticle;
		}
	}
	
	public EntityStats stats {
		get {
			return mStats;
		}
	}
	
	public PlanetAttach planetAttach {
		get {
			return mPlanetAttach;
		}
	}
	
	public Action prevAction {
		get {
			return mPrevAct;
		}
	}
	
	public Action action {
		get {
			return mCurAct;
		}
		set {
			if(mCurAct != value) {
				mPrevAct = mCurAct;
				mCurAct = value;
				if(mCurAct != Action.NumActions) {
					foreach(IListener l in mListeners) {
						l.OnEntityAct(value);
					}
				}
			}
		}
	}
	
	public void YieldSetAction(Action act) {
		StartCoroutine(OnYieldAction(act));
	}
	
	IEnumerator OnYieldAction(Action act) {
		yield return new WaitForFixedUpdate();
		
		action = act;
		
		yield break;
	}
	
	public void FlagsAdd(Flag flag) {
		mFlags |= flag;
	}
	
	public void FlagsRemove(Flag flag) {
		mFlags ^= flag;
	}
	
	public bool FlagsCheck(Flag flag) {
		return (mFlags & flag) != Flag.None;
	}
	
	public void InvulnerableOff() {
		mEntCurTime = 0;
		
		FlagsRemove(Flag.Invulnerable);
		
		foreach(IListener l in mListeners) {
			l.OnEntityInvulnerable(false);
		}
	}
	
	public void Invulnerable(float delay) {
		mEntCurTime = 0;
		mInvulDelay = delay;
		FlagsAdd(Flag.Invulnerable);
		
		foreach(IListener l in mListeners) {
			l.OnEntityInvulnerable(true);
		}
	}
	
	/// <summary>
	/// Spawn this entity, resets stats, set action to spawning, then later calls OnEntitySpawnFinish.
	/// NOTE: calls after an update to ensure Awake and Start is called.
	/// </summary>
	public void Spawn() {
		//ensure start is called before spawning if we are freshly allocated from entity manager
		StartCoroutine(DoSpawn());
	}
	
	//////////internal methods
	
	
			
	/////////////implements
	
	//we are being targetted or cleared out of target
	//called by reticle manager when reticle is set to us
	public virtual void OnTargetted(bool yes) {
	}
	
	protected virtual void Awake() {
		mStats = GetComponent<EntityStats>();
		mPlanetAttach = GetComponent<PlanetAttach>();
		
		Component[] cs = GetComponentsInChildren(typeof(IListener), true);
		mListeners = new IListener[cs.Length];
		for(int i = 0; i < cs.Length; i++) {
			mListeners[i] = cs[i] as IListener;
		}
	}
	
	protected virtual void OnEnable() {
	}
	
	protected virtual void Start() {
		action = Action.idle;
	}
	
	protected virtual void Update() {
		switch(mCurAct) {
		case Action.NumActions:
			//why are we here?
			break;
			
		case Action.spawning:
			mEntCurTime += Time.deltaTime;
			if(mEntCurTime >= spawnDelay) {
				mCurAct = Action.NumActions; //no act until set later
				
				foreach(IListener l in mListeners) {
					l.OnEntitySpawnFinish();
				}
			}
			break;
			
		default:
			//check collision
			//planet attach necessary?
			if(mListeners.Length > 0 && mCollideLayerMask > 0 && mPlanetAttach != null) {
				float radius = mPlanetAttach.radius;
				RaycastHit hit;
				if(Physics.SphereCast(transform.position, radius, new Vector3(0,0,1.0f), out hit, 1.0f, mCollideLayerMask)) {
					Entity e = hit.transform.GetComponent<Entity>();
					if(e != null) {
						foreach(IListener l in mListeners) {
							l.OnEntityCollide(e, true);
						}
						
						//tell the other receiving end
						foreach(IListener lOther in e.mListeners) {
							lOther.OnEntityCollide(this, false);
						}
					}
				}
			}
			
			if(FlagsCheck(Flag.Invulnerable)) {
				mEntCurTime += Time.deltaTime;
				if(mEntCurTime >= mInvulDelay) {
					InvulnerableOff();
				}
			}
			break;
		}
	}
	
	//////////internal
	void OnDestroy() {
		mListeners = null;
	}
	
	IEnumerator DoSpawn() {
		yield return new WaitForFixedUpdate();
		
		if(stats != null) {
			stats.ResetStats();
		}
		
		mEntCurTime = 0.0f;
		
		action = Action.spawning;
		
		yield break;
	}
}
