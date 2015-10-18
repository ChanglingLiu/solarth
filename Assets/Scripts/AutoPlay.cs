using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AutoPlay : MonoBehaviour {

	public float degreePerSec;
	float lastUpdate;

	// Use this for initialization
	void Start () {
		lastUpdate = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		float now = Time.time;
		if (!this.GetComponent<Toggle> ().isOn) {
			lastUpdate = now;
			return;
		}

		Slider comp = GameObject.Find ("Slider").GetComponent<Slider> ();
		if (comp.value >= 1)
			comp.value -= 1;
		comp.value += degreePerSec * (now - lastUpdate);

		lastUpdate = now;
	}
}
