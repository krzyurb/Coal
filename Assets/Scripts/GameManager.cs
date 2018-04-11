using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public BoardManager boardScript;
	public float turnDelay = 0.00f;
	public float levelStartDelay = 2f;
	public int level = 1;
	public int coalTotal  = 10;
	[HideInInspector] public bool playersTurn = true;
	private List<Enemy> enemies;
	private bool enemiesMoving;
	private bool doingSetup = true;
	private GameObject levelImage;
	
	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		enemies = new List<Enemy> ();
		SceneManager.activeSceneChanged += OnSceneLoaded;

		boardScript = GetComponent<BoardManager> ();
		InitGame ();
	}


	void OnLevelWasLoaded (int index) {
//		level++;
		InitGame ();
	}

	void OnSceneLoaded (Scene previousScene, Scene newScene) {
//	    level++;
	}

	public void InitGame () {
	    doingSetup = true;
	    Invoke ("HideLevelImage", levelStartDelay);
		enemies.Clear ();
		boardScript.SetupScene (level++);
		Debug.Log (level);
	}

	void HideLevelImage () {
		doingSetup = false;
	}

	public void GameOver () {
		SceneManager.LoadScene ("StartMenu");
	}

	public void AddEnemyToList (Enemy script) {
		enemies.Add (script);
	}

	void Update () {
		GameObject.Find ("DayText").GetComponent<Text> ().text = (GameManager.instance.level - 2).ToString ();

		if (playersTurn || enemiesMoving || doingSetup)
			return;
		
		StartCoroutine (MoveEnemies ());
	}

	IEnumerator MoveEnemies() {
	    enemiesMoving = true;
	    yield return new WaitForSeconds (turnDelay);

		if (enemies.Count == 0)
			yield return new WaitForSeconds (turnDelay);
	
		for (int i = 0; i < enemies.Count; i++) {
			if (enemies[i] != null)
				enemies[i].MoveEnemy ();
			yield return new WaitForSeconds (0.5f);
	    }

	    playersTurn   = true;
		enemiesMoving = false;
	}
}