using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public enum Mode {
		Free,
		Attach
	}
	
	private Mode mCurMode = Mode.Free;
	
	private Transform mAttach;
	
	private Transform mOrigin;
	
	private float mOriginMinDistance;
	
	//prev pos, prev up
	//curTime
	
	//public Anchor.Type anchor = Anchor.Type.BottomLeft;
	
	//private tk2dCamera mTKCamera;
	
	public Transform origin {
		get {
			return mOrigin;
		}
		set {
			mOrigin = value;
		}
	}
	
	public Transform attach {
		get {
			return mAttach;
		}
		set {
			mAttach = value;
		}
	}
	
	public Mode mode {
		get {
			return mCurMode;
		}
		set {
			mCurMode = value;
			
			switch(mCurMode) {
			case Mode.Free:
				break;
			case Mode.Attach:
				break;
			}
			
			//TODO: set interpolation
		}
	}
	
	public float originMinDistance {
		get {
			return mOriginMinDistance;
		}
		set {
			mOriginMinDistance = value;
		}
	}
	
	void SceneShutdown() {
		mAttach = null;
		mOrigin = null;
	}
	
	void Awake() {
		//mTKCamera = mCamera.GetComponent<tk2dCamera>();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		switch(mCurMode) {
		case Mode.Attach:
			if(mAttach != null) {
				Vector3 attachPos = mAttach.position;
				
				Transform camTrans = transform;
				Vector3 camPos = camTrans.position;
				
				camTrans.rotation = mAttach.rotation;
				
				Vector3 newPos = new Vector3(attachPos.x, attachPos.y, camPos.z);//new Vector3(attachPos.x + tX, attachPos.y + tY, camPos.z);
				
				//limit the position's distance to origin
				if(mOrigin != null) {
					Vector3 origPos = mOrigin.position;
					Vector3 dirToCam = newPos - origPos;
					float dirToCamDist = dirToCam.magnitude;
					if(dirToCamDist < mOriginMinDistance) {
						dirToCam /= dirToCamDist;
						newPos = origPos + dirToCam*mOriginMinDistance;
						newPos.z = camPos.z;
					}
				}
				
				camTrans.position = newPos;
			}
			break;
				
		case Mode.Free:
			break;
		}
	}
}
