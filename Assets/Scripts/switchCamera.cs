using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class switchCamera : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		this.GetComponent<Camera> ().enabled = GameObject.Find("Toggle").GetComponent<Toggle> ().isOn;
	}
}
