using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject {

	public int hp=4;
	public int playerDmg;
	public GameObject enemySpawned;
	public int playerDamage;
	public int wallDamage;

	private GameManager gameManage;
	private Animator animator;
	private Transform target;
	private int skipMove;

	void Awake () {
		gameManage = GameObject.Find ("GameManager").GetComponent<GameManager> ();
	}

	protected override void Start () {
		hp = 2;
		playerDamage = 10;

		GameManager.instance.AddEnemyToList (this);
		animator = GetComponent <Animator> ();
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		base.Start ();
	}

//	void SpawnEnemy() {
//		if ((int)Random.Range (0, 10) == 1) {
//			Instantiate(enemySpawned, new Vector2(transform.position.x + Random.Range(-1,1), transform.position.y + Random.Range (-1, 1)), Quaternion.identity);
//		}
//	}

	protected override void OnCantMove <T> (T component) {

	}



}
