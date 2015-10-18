using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public static class CalculateAtmosphereUnit {
	public static Transform prefab;

	public static int gridX;
	public static int gridY;
	static Vector3[,] gridPosition;
	static Transform[,] gridItem;
	public static Vector3[,] gridDensity;

	public static float density;
	public static float height;
	public static float gravity;
	public static float sunGravity;
	public static float fixForce;
	public static float squaredOmega;

	static float lastUpdate;

	static int neLen = 4;
	static int[,] unitNeighbor =  { { 0, 1},
		{ 0,-1},
		{ 1, 0},
		{-1, 0} }; 
	

	
	public static void initGrid(Transform instance, 
	                            float densityOfAtmosphere, 
	                            float heightOfAtomosphere, 
	                            float gravityOfEarth,
	                            float gravityOfSun,
	                            float fixRate,
	                            float squaredAngularVelocity,
	                            int xDivision = 36, int yDivision = 36){
		prefab = instance;
		density = densityOfAtmosphere;
		height = heightOfAtomosphere;
		gridX = xDivision;
		gridY = yDivision;
		gravity = gravityOfEarth;
		sunGravity = gravityOfSun;
		fixForce = fixRate;
		lastUpdate = Time.time;
		squaredOmega = squaredAngularVelocity;

		Transform earth = GameObject.Find ("Earth").transform;

		gridPosition = new Vector3[gridX, gridY];
		gridItem = new Transform[gridX, gridY];
		gridDensity = new Vector3[gridX, gridY];

		for (int x = 0; x < gridX; ++x) {
			for (int y = 1; y < gridY; ++y){
				//if (y == gridY / 2) continue;
				float alpha = 1.0f * x / gridX * Mathf.PI * 2.0f;
				float beta =  1.0f * y / gridY * Mathf.PI;

				gridPosition[x, y] = new Vector3(height * Mathf.Sin(beta) * Mathf.Cos(alpha),
												 height * Mathf.Cos(beta),
				                                 height * Mathf.Sin(beta) * Mathf.Sin(alpha));

				gridItem[x, y] = Transform.Instantiate(prefab, gridPosition[x, y] + earth.position, Quaternion.identity) as Transform;

			}
		}
	}

	static Vector3 applyForce (Rigidbody item, Transform obj, float distExpect, float density, float timeMultiplier){
		float k = (distExpect - Vector3.Distance (item.transform.position, obj.position)) * density * timeMultiplier;
		if (k < 0)
			item.AddForce (k * (obj.position - item.transform.position));
		return k * (obj.position - item.transform.position);
	}

	public static void dumpGridDensity(){
		Slider comp = GameObject.Find ("Slider").GetComponent<Slider> ();
		StreamWriter sw = new StreamWriter (System.Environment.CurrentDirectory + "\\dump.txt");
		lock (gridDensity) {
			sw.WriteLine(gridX);
			sw.WriteLine(gridY - 1);
			sw.WriteLine(comp.value * 365.2422f);
			for (int x = 0; x < gridX; ++x) {
				for (int y = 1; y < gridY; ++y) {
					sw.Write ((gridDensity [x, y].magnitude * 1e4f).ToString ());
					sw.Write ("\t");
				}
				sw.WriteLine("");
			}
		}
		sw.Close ();
		//System.IO.File.WriteAllText (System.Environment.CurrentDirectory + "\\dump.txt", mag.ToString());
	}

	public static void updateGrid(){
		// plz use FixedUpdate
		float now = Time.time;
		Transform earth = GameObject.Find ("Earth").transform;
		Transform sun = GameObject.Find ("Sun").transform;
		Rigidbody currentRigidbody;

		float max = 0f, min = 1e20f, ave = 0f;
		for (int x = 0; x < gridX; ++x) {
			for (int y = 1; y < gridY; ++y) {
				//if (y == gridY / 2) continue;
				float alpha = 1.0f * x / gridX * Mathf.PI * 2.0f;
				
				Vector3 direction = earth.position + gridPosition[x, y] - gridItem[x, y].transform.position;

				currentRigidbody = gridItem[x, y].GetComponent<Rigidbody> ();

				gridDensity[x, y] = new Vector3(0f,0f,0f);
				for (int i = 0; i < neLen; ++i){

					if (x + unitNeighbor[i,0] < 0 || x + unitNeighbor[i,0] >= gridX) continue;
					if (y + unitNeighbor[i,1] < 1 || y + unitNeighbor[i,1] >= gridY) continue;

					gridDensity[x, y] += applyForce(currentRigidbody, 
					           						gridItem[x + unitNeighbor[i, 0], y + unitNeighbor[i, 1]],
					         		  				Vector3.Distance(gridPosition[x + unitNeighbor[i, 0], y + unitNeighbor[i ,1]], 
					                 								 gridPosition[x, y]),
					           						density, now - lastUpdate);
				}
				max = Mathf.Max(gridDensity[x, y].magnitude, max);
				min = Mathf.Min(gridDensity[x, y].magnitude, min);
				ave += gridDensity[x, y].magnitude;

				//Debug.Log(unitNeighbor.Length);
				currentRigidbody.AddForceAtPosition(direction.normalized * (now - lastUpdate) * fixForce, 
				                                    earth.position + gridPosition[x, y]);

				float earthDist = Vector3.Distance(earth.position, gridItem[x, y].position);
				if (earthDist < earth.localScale.x / 2.0f) 
					currentRigidbody.transform.position = (currentRigidbody.position - earth.position).normalized * earth.localScale.x / 2.0f + earth.position;
				currentRigidbody.AddForce( (earth.position - gridItem[x, y].position).normalized * (now - lastUpdate) * gravity 
				                          / earthDist / earthDist * (1 - squaredOmega * Mathf.Sin(alpha) ));

				float sunDist = Vector3.Distance(sun.position, gridItem[x, y].position);
				currentRigidbody.AddForce( (  sun.position - gridItem[x, y].position).normalized * (now - lastUpdate) * sunGravity
				                            / sunDist / sunDist );

			}
		}

		GameObject.Find ("MaxLabel").GetComponent<Text> ().text = string.Format("Max: {0:F4}", (max * 1e4f));
		GameObject.Find ("MinLabel").GetComponent<Text> ().text = string.Format("Min: {0:F4}", (min * 1e4f));
		GameObject.Find ("Average").GetComponent<Text> ().text = string.Format("Average: {0:F4}", (ave * 1e4f / gridX / (gridY - 1)));

		for (int x = 0; x < gridX; ++x) {
			for (int y = 1; y < gridY; ++y) {
				float qen = (gridDensity[x, y].magnitude - min)/(max - min);
				gridItem[x, y].GetComponent<MeshRenderer> ().material.color = qen * Color.red + (1f - qen) * Color.green;
			}
		}

		lastUpdate = now;


	}

}
