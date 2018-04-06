using UnityEngine;
using System.Collections;

public class Enemy : MovingObject {
	public int playerDamage;
	private Animator animator;
	private Transform target;
	private int skipMove;
	private GameManager gameManage;
	public int hp = 4;

	void Awake () {
		gameManage = GameObject.Find ("GameManager").GetComponent<GameManager> ();
	}

	protected override void Start () {
		hp = 4;
		playerDamage = 20;

		GameManager.instance.AddEnemyToList (this);
		animator = GetComponent<Animator> ();
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		base.Start ();
	}

	protected override void AttemptMove <T> (int xDir, int yDir) {
	  //Check if skipMove is true, if so set it to false and skip this turn.
		if (skipMove == 1) {
			skipMove = 0;
			return;
		}

		base.AttemptMove <T> (xDir, yDir);
		skipMove = Random.Range (0, 2);
	}

	public void MoveEnemy () {
		int xDir = 0;
		int yDir = 0;

		if(Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon)
			yDir = target.position.y > transform.position.y ? 1 : -1;
	    else
	        xDir = target.position.x > transform.position.x ? 1 : -1;

		AttemptMove <Player> (xDir, yDir);
	}

	protected override void OnCantMove <T> (T component) {
//	    Player hitplayer = component as Player;
	    animator.SetTrigger ("enemyAttack");
//	    hitplayer.LoseFood (playerDamage);
	    Player targetScript = target.gameObject.GetComponent <Player> ();
	}

	protected override void EnemyHitWall <T> (T component) { }

	public void TakeDamage (int damage) {
		hp -= damage;

		if (hp <= 0) {
			DestroyObject (gameObject);
			//gameObject.SetActive (false);
		}
	}

	protected override void EnemyAttack <T> (T component) { }
}
