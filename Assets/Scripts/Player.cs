﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovingObject {

	public int wallDamage = 1;
	public int pointsPerFood = 10;
	public int pointsPerSoda = 20;
	public float restartLevelDelay = 1f;
	private Animator animator;
	private int food;

	private bool isPressed = false;

	// Use this for initialization
	protected override void Start () {
		animator = GetComponent<Animator>();
//		food = GameManager.instance.playerFoodPoints;
		food = 10;
		base.Start ();
	}

	private void OnDisable()
	{
//		GameManager.instance.playerFoodPoints = food;
	}

	void Update () {

		int horizontal = 0;
		int vertical = 0;

		horizontal = (int)Input.GetAxisRaw("Horizontal");
		vertical =   (int)Input.GetAxisRaw("Vertical");

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


	protected override void AttemptMove<T>(int xDir, int yDir)
	{
		food--;
		base.AttemptMove<T>(xDir, yDir);
		RaycastHit2D hit;
		CheckIfGameOver();
        //CheckIfUserLevelUp(); // init game with new level, enlarge scene
//		GameManager.instance.playersTurn = false;
	}

	protected override void OnCantMove<T>(T component)
	{
		Wall hitWall = component as Wall;
//		hitWall.DamageWall(wallDamage);
//		animator.SetTrigger("playerChop");
	}

	private void Restart()
	{
		Application.LoadLevel(Application.loadedLevel);
	}

	public void LoseFood(int loss)
	{
		animator.SetTrigger("playerHit");
		food -= loss;
		CheckIfGameOver();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Exit")
		{
			Invoke("Restart", restartLevelDelay);
			enabled = false;
		}
		else if(other.tag == "Food")
		{
			food += pointsPerFood;
			other.gameObject.SetActive(false);
		} 
		else if(other.tag == "Soda")
		{
			food += pointsPerSoda;
			other.gameObject.SetActive(false);
		}
	}

	private void CheckIfGameOver()
	{
//		if (food <= 0)
//			GameManager.instance.GameOver();
	}
}
