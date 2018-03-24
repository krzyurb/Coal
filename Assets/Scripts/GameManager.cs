﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public static GameManager instance = null;

	public BoardManager boardScript;
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true;

	private int level = 1;

	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad(gameObject);
		
		boardScript = GetComponent<BoardManager> ();
		InitGame ();
	}

	void InitGame () {
		boardScript.SetupScene (level);
	}

    public void GameOver()
    {
        enabled = false;
        level = 1;
    }
}
