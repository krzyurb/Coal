using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MovingObject {

	public int coalDamage = 1;
	public int damage = 1;
	public float restartLevelDelay = 1f;
	private Animator animator;
	public int coal;
	public int movePoints;

	private AudioSource audioSource;
	public AudioClip stepSound;
	public AudioClip hitSound;
	public AudioClip pointSound;
	public AudioClip piwkoSound;
	public AudioClip failSound;
	public AudioClip gameOverSound;

	private bool isPressed = false;

	public Text coalText;
	public Text moveText;

	protected override void Start () {
		audioSource = GetComponent<AudioSource>();
		animator = GetComponent<Animator> ();
		coal = 0;
	
		if (((int)(GameManager.instance.coalTotal / 5f)) > 2)
			movePoints = 10;
		else
			movePoints = 15;

	    base.Start ();
	}

	private void OnDisable () {
//		GameManager.instance.coalTotal += coal;
	}

	void Update () {
		string coalNum = (GameManager.instance.coalTotal + coal < 0) ? "0" : (GameManager.instance.coalTotal + coal).ToString ();
		GameObject.Find ("CoalText").GetComponent<Text> ().text = coalNum;
		GameObject.Find ("LifeText").GetComponent<Text> ().text = ((int)(GameManager.instance.coalTotal/5f)).ToString();
		GameObject.Find ("MoveText").GetComponent<Text> ().text = movePoints.ToString();

		int horizontal = 0;
		int vertical   = 0;

		horizontal = (int)Input.GetAxisRaw ("Horizontal");
		vertical =   (int)Input.GetAxisRaw ("Vertical");

		if (horizontal != 0)
			vertical = 0;

		if (!isPressed && (horizontal != 0 || vertical != 0)) {
			isPressed = true;
			AttemptMove<Wall> (horizontal, vertical);
		}

		if (horizontal == 0 && vertical == 0) {
			isPressed = false;
		}
	}


	protected override void AttemptMove<T> (int xDir, int yDir) {
		movePoints--;
		base.AttemptMove<T> (xDir, yDir);
		RaycastHit2D hit;
		CheckIfGameOver();
		GameManager.instance.playersTurn = false;
		audioSource.PlayOneShot (stepSound);
	}

	protected override void OnCantMove<T> (T component) {
		Coal hitCoal = component as Coal;
		hitCoal.DamageCoal (coalDamage, this);
		animator.SetTrigger ("playerChop");
		audioSource.PlayOneShot (hitSound);
	}

	public void LoseFood (int loss) {
		animator.SetTrigger ("playerHit");
		movePoints -= loss;
		CheckIfGameOver ();
	}

	private void OnTriggerEnter2D (Collider2D other) {
		if(other.tag == "Exit") {
			GameManager.instance.coalTotal += coal;
			coal = 0;
			GameManager.instance.coalTotal -= 5;
			Invoke("Restart", restartLevelDelay);
			enabled = false;
		} else if(other.tag == "Piwko") {
			audioSource.PlayOneShot (piwkoSound);
			movePoints += 11;
	        other.gameObject.SetActive (false);
	    } else if(other.tag == "Diament") {
			audioSource.PlayOneShot (pointSound);
			movePoints += 6;
			other.gameObject.SetActive (false);
	    }
	}

	private void CheckIfGameOver ()	{
		if (movePoints <= 0) {
			audioSource.PlayOneShot (failSound);
			GameManager.instance.coalTotal -= 5;
			Invoke ("Restart", restartLevelDelay);
			enabled = false;
		}

		if (GameManager.instance.coalTotal < 0) {
			audioSource.PlayOneShot (gameOverSound);
			Invoke ("GameOver", 5000f);
			GameManager.instance.GameOver ();
		}
	}

	protected override void EnemyAttack <T> (T component) {
		Enemy baddie = component as Enemy;
		animator.SetTrigger ("playerChop");
		movePoints -= 2;
		baddie.TakeDamage (damage);
	}

	protected override void EnemyHitWall <T> (T component) { }

	private void Restart () {
		GameManager.instance.InitGame ();
	}

	private void GameOver () {
		GameManager.instance.GameOver ();
	}
}
