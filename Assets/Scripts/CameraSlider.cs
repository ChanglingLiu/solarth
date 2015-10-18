using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraSlider : MonoBehaviour {

	public float cameraDistance;
	
	// Update is called once per frame
	void Update () {
		Transform earthCameraTransform = GameObject.Find ("Camera").transform;
		Transform earth = GameObject.Find ("Earth").transform;
		float angle = GameObject.Find ("Slider Camera").GetComponent<Slider> ().value * 2.0f * Mathf.PI;
		GameObject.Find ("Slider Camera").GetComponent<Slider> ().interactable = GameObject.Find ("Toggle").GetComponent<Toggle> ().isOn;
		earthCameraTransform.position = earth.position + new Vector3 (cameraDistance * Mathf.Sin(angle), 0f, 
		                                                              cameraDistance * Mathf.Cos(angle));
		earthCameraTransform.LookAt (earth);

	}
}
