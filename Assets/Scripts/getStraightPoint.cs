using UnityEngine;
using System.Collections;

public class getStraightPoint : MonoBehaviour {

	public Transform prefab;

	bool inited;
	Transform redPoint;

	void Start() {
		inited = false;
	}


	// Update is called once per frame
	void Update () {
		Transform earth = GameObject.Find ("Earth").transform;
		Transform sun = GameObject.Find ("Sun").transform;
		Vector3 redPos = (sun.position - earth.position).normalized;

		if (! inited) {
			redPoint = Instantiate (prefab, earth.position + earth.localScale.x / 2.0f * redPos, Quaternion.identity) as Transform;
			redPoint.parent = earth;
			inited = true;
		} else {
			redPoint.position = earth.position + earth.localScale.x / 2.0f * redPos;
		}

	}
}
