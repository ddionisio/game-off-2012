using UnityEngine;
using System.Collections;

public class Ease {
	public static float In(float t, float tMax, float start, float delta) {
		return start + delta*_in(t/tMax);
	}
	
	private static float _in(float r) {
		return r*r*r;
	}

	public static float Out(float t, float tMax, float start, float delta) {		
		return start + delta*_out(t/tMax);
	}
	
	private static float _out(float r) {
		float ir = r - 1.0f;
		return ir*ir*ir + 1.0f;
	}
}
