using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntityManager : MonoBehaviour {
	[System.Serializable]
	public class FactoryData {
		public Transform template;
		public float z;
		
		public int startCapacity;
		public int maxCapacity;
						
		[System.NonSerialized]
		private List<Transform> available;
		
		private int allocateCounter = 0;
		
		private Transform poolHolder;
		
		public void Init(Transform poolHolder) {
			this.poolHolder = poolHolder;
			
			available = new List<Transform>(maxCapacity);
			Expand(startCapacity);
		}
		
		public void Expand(int num) {
			for(int i = 0; i < num; i++) {
				//PoolDataController
				Transform t = (Transform)Object.Instantiate(template);
				t.parent = poolHolder;
				
				PoolDataController pdc = t.GetComponent<PoolDataController>();
				if(pdc == null) {
					pdc = t.gameObject.AddComponent<PoolDataController>();
				}
				
				pdc.factoryKey = template.name;
				
				t.gameObject.SetActiveRecursively(false);
				
				available.Add(t);
			}
		}
		
		public void Release(Transform t) {
			t.parent = poolHolder;
			t.gameObject.SetActiveRecursively(false);
			
			available.Add(t);
			allocateCounter--;
		}
		
		public Transform Allocate(string name, Transform parent) {
			if(available.Count == 0) {
				if(allocateCounter+1 > maxCapacity) {
					Debug.LogWarning(template.name+" is expanding beyond max capacity: "+maxCapacity);
					
					Expand(maxCapacity);
				}
				
				Expand(1);
			}
			
			Transform t = available[available.Count-1];
			available.RemoveAt(available.Count-1);
			
			t.name = string.IsNullOrEmpty(name) ? template.name + allocateCounter : name;
			t.parent = parent;
			t.localPosition = new Vector3(0.0f, 0.0f, z);
			t.localRotation = Quaternion.identity;
			t.localScale = Vector3.one;
			
			allocateCounter++;
			return t;
		}
		
		public void DeInit() {
			available.Clear();
			
			poolHolder = null;
					
			allocateCounter = 0;
		}
	}
	
	[SerializeField]
	FactoryData[] factory;
	
	[SerializeField]
	Transform poolHolder;
	
	private static EntityManager mInstance = null;
	
	private Dictionary<string, FactoryData> mFactory;
	
	public static EntityManager instance {
		get {
			return mInstance;
		}
	}
	
	//if toParent is null, then set parent to us.
	public Transform Spawn(string type, string name, Transform toParent, string waypoint) {
		Transform ret = null;
		
		FactoryData dat;
		if(mFactory.TryGetValue(type, out dat)) {
			ret = dat.Allocate(name, toParent == null ? transform : toParent);
			
			if(ret != null) {				
				if(!string.IsNullOrEmpty(waypoint)) {
					Transform wp = WaypointManager.instance.GetWaypoint(waypoint);
					if(wp != null) {
						ret.position = wp.position;
					}
				}
				
				ret.gameObject.SetActiveRecursively(true);
				
				Entity entity = ret.GetComponentInChildren<Entity>();
				if(entity != null) {
					entity.Spawn();
				}
			}
		}
		else {
			Debug.LogWarning("No such type: "+type+" attempt to allocate: "+name);
		}
		
		return ret;
	}
	
	public void Release(Transform t) {
		PoolDataController pdc = t.GetComponent<PoolDataController>();
		if(pdc != null) {
			FactoryData dat;
			if(mFactory.TryGetValue(pdc.factoryKey, out dat)) {
				dat.Release(t);
			}
		}
		else { //not in the pool, just kill it
			Object.Destroy(t.gameObject);
		}
	}
	
	void OnDestroy() {
		mInstance = null;
		
		foreach(FactoryData dat in mFactory.Values) {
			dat.DeInit();
		}
	}
			
	void Awake() {
		mInstance = this;
		
		//generate cache and such
		mFactory = new Dictionary<string, FactoryData>(factory.Length);
		foreach(FactoryData factoryData in factory) {
			factoryData.Init(poolHolder);
			
			mFactory.Add(factoryData.template.name, factoryData);
		}
		
		poolHolder.gameObject.SetActiveRecursively(false);
	}
}
