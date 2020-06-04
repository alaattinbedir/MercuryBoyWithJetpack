using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatingParticleSortingLayerFix2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = "Objects";
		GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingOrder = -1;
	}

	// Update is called once per frame
	void Update () {

	}
}
