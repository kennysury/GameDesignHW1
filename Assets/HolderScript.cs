using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolderScript : MonoBehaviour {

	public LineRenderer frontElastic, backElastic;
	public Vector3[] Points;
	public Vector3 frontjoint;
	// Use this for initialization
	void Start () {
		frontElastic.sortingLayerName = "CatapultFront";
	}
	
	// Update is called once per frame
	void Update () {
		Points [0] = frontjoint;
		Points [1] = transform.position;
		frontElastic.SetPositions (Points);
	}
}
