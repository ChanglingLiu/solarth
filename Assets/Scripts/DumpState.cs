using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DumpState : MonoBehaviour {

	// Use this for initialization
	void Update (){
		this.GetComponent<Button> ().interactable = GameObject.Find ("Atmosphere").GetComponent<Toggle> ().isOn;
	}

	public void Click(){
		GameObject.Find ("Path").GetComponent<Text> ().text = "File dumped to " + System.Environment.CurrentDirectory + "\\dump.txt";
		//print (System.Environment.CurrentDirectory);
		CalculateAtmosphereUnit.dumpGridDensity ();
	}
}
