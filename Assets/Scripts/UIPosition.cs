using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPosition : MonoBehaviour {

	private Vector3 offset;
	private GameObject camera;

	void Start () 
	{
		offset = transform.position - GameObject.FindWithTag("MainCamera").transform.position;
		camera = GameObject.FindWithTag ("MainCamera");
	}

	void Update()
	{
		Vector3 moveCamera = new Vector3 (camera.transform.position.x, camera.transform.position.y, 10f);
		transform.position = Vector3.Lerp (transform.position, moveCamera + offset, 20f * Time.deltaTime);
	}

}
