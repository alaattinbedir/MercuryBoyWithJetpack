using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour {

	float desiredMPS = 0.06f;


	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate () {

		transform.position = new Vector3(transform.position.x - desiredMPS, transform.position.y, transform.position.z);//move on y axis only

	}
}


