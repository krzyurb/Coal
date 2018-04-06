using UnityEngine;
using System.Collections;

public class CamMove : MonoBehaviour {

	private GameObject player;
	private Vector3 offset;

	void Start () {
		player = GameObject.FindWithTag ("Player");
		offset = transform.position - player.transform.position;
	}

	void Update () {
		player = GameObject.FindWithTag ("Player");
	}

	void LateUpdate () {
		transform.position = player.transform.position + offset;
	}
}