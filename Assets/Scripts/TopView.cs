using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TopView : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Transform camera = GameObject.Find ("Camera").transform;
		CameraSlider cs = GameObject.Find ("Slider Camera").GetComponent<CameraSlider> ();
		Vector3 earthPos = GameObject.Find ("Earth").transform.position;

		if (this.GetComponent<Toggle> ().isOn) {
			cs.enabled = false;
			camera.position = new Vector3(0f, 5f, 0f) + earthPos;
			camera.LookAt(earthPos);
			cs.GetComponent<Slider>().interactable = false;
		} else {
			cs.enabled = true;
			cs.GetComponent<Slider>().interactable = true;
		}
	}
}
