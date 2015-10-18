using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class applyUnitChange : MonoBehaviour {

	bool inited = false;
	public Transform prefab;
	public float densityOfAtmosphere;
	public float heightOfAtomosphere;
	public float gravityOfEarth;
	public float gravityOfSun;
	public float fixRate;
	public float squaredAngularVelocity; 
	public int xDivision;
	public int yDivision;

	void Start () {
		inited = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Toggle atm = GameObject.Find("Atmosphere").GetComponent<Toggle> ();

		if (! atm.isOn)
			return;
		else {
			atm.interactable = false;
			GameObject.Find("Slider").GetComponent<Slider> ().interactable = false;
			Toggle ap = GameObject.Find("Auto").GetComponent<Toggle> ();
			ap.isOn = true;
			ap.interactable = false;
		}

		if (!inited) {
			inited = true;
			CalculateAtmosphereUnit.initGrid (prefab, densityOfAtmosphere, heightOfAtomosphere, 
			                                  gravityOfEarth, gravityOfSun, fixRate, squaredAngularVelocity,
			                                  xDivision, yDivision);
		}else
			CalculateAtmosphereUnit.updateGrid ();

	}
}
