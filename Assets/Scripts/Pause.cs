using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Pause : MonoBehaviour {
	public float stopAfterTime;
	bool autoStop = true;
	// Use this for initialization
	void Start () {
		autoStop = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (autoStop && stopAfterTime != 0) {
			GameObject.Find("Atmosphere").GetComponent<Toggle> ().isOn = true;
		}

		if (autoStop && stopAfterTime != 0 && Time.time > stopAfterTime){
			this.GetComponent<Toggle> ().isOn = true;
			autoStop = false;
		}

		if (this.GetComponent<Toggle> ().isOn) {
			Time.timeScale = 0.0f;
		} else {
			Time.timeScale = 1.0f;
		}
	}
}
