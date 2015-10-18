using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class rotate_around_sun : MonoBehaviour {

	public float a = 14.96f;
	public float b = 14.958f;
	public float earth_sun_angle = 23.5f / 360.0f * 2.0f * Mathf.PI;

	// Update is called once per frame
	void Update () {
		Transform earth = GameObject.Find ("Earth").transform;
		float alpha = this.GetComponent<Slider> ().value * 2 * Mathf.PI;


		float theta = (365.2564f * alpha - Mathf.Floor (365.2564f * alpha)) * 360.0f;
		earth.rotation = Quaternion.Euler (new Vector3 (0.0f, theta));
		//float theta = (365.2564f * alpha - Mathf.Floor (365.2564f * alpha)) * 2 * Mathf.PI;
		//earth.rotation = new Quaternion (Mathf.Sin (23.5f / 180.0f * Mathf.PI) * Mathf.Sin (theta / 2.0f), 
		//                                 Mathf.Sin (23.5f / 180.0f * Mathf.PI) * Mathf.Cos (theta / 2.0f), 0, 
		//                                 Mathf.Cos (theta / 2.0f));

		Vector3 planePosition = new Vector3( Mathf.Cos (alpha) * a * Mathf.Cos(earth_sun_angle), 
		                                     Mathf.Cos (alpha) * a * Mathf.Sin(earth_sun_angle), 
		                                     Mathf.Sin (alpha) * b);
		earth.position = planePosition;
	}
}
