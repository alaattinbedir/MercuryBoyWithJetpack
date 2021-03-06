﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScriptUpDown : MonoBehaviour {

	//1
	public Sprite laserOnSprite;    
	public Sprite laserOffSprite;

	//2    
	public float interval = 0.5f;    
	public float rotationSpeed = 0.0f;

	//3
	private bool isLaserOn = true;    
	private float timeUntilNextToggle;
	public float hight = 2.2f;//max height of Box's movement
	public float yCenter = 0f;

	// Use this for initialization
	void Start () {
		timeUntilNextToggle = interval;
	}

	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate () {
		//1
		timeUntilNextToggle -= Time.fixedDeltaTime;

		//2
		if (timeUntilNextToggle <= 0) {

			//3
			isLaserOn = !isLaserOn;

			//4
			GetComponent<Collider2D>().enabled = isLaserOn;

			//5
			SpriteRenderer spriteRenderer = ((SpriteRenderer)this.GetComponent<Renderer>());
			if (isLaserOn)
				spriteRenderer.sprite = laserOnSprite;
			else
				spriteRenderer.sprite = laserOffSprite;

			//6
			timeUntilNextToggle = interval;
		}

		//7
		transform.position = new Vector3(transform.position.x, yCenter + Mathf.PingPong(Time.time * 2, hight) - hight/2f, transform.position.z);//move on y axis only

		//		transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time. fixedDeltaTime);


	}
}

