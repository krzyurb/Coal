using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public BoardManager boardScript;
	public float turnDelay = 0.00f;
	public int playerFoodPoints = 100;
	public int level = 1;
    [HideInInspector] public bool playersTurn = true;

	public int coalTotal  = 10;

	private List<Enemy> enemies;
	private bool enemiesMoving;
    public float levelStartDelay = 2f;
    private bool doingSetup = true;
    private GameObject levelImage;

	void Awake () {
		if (instance == null)
		  instance = this;
		else if (instance != this)
		  Destroy (gameObject);

		DontDestroyOnLoad(gameObject);
		enemies = new List<Enemy>();
		SceneManager.activeSceneChanged += OnSceneLoaded;

		boardScript = GetComponent<BoardManager> ();
		InitGame ();
	}

	void OnLevelWasLoaded(int index) {
		level++;
//	    if(level == 3) {
//	        level = 0;
//	    }
		InitGame();
	}

	void OnSceneLoaded (Scene previousScene, Scene newScene) {
	    level++;

//	    if (level == 3) {
//	        level = 0;
//	    }
	}

	public void InitGame () {
	    doingSetup = true;
	    Invoke("HideLevelImage", levelStartDelay);
		enemies.Clear ();
		boardScript.SetupScene (level++);
		Debug.Log (level);
	}

  void HideLevelImage() {
    doingSetup = false;
  }

  public void GameOver() {
		Application.LoadLevel("StartMenu");

  }

  public void AddEnemyToList (Enemy script) {
    enemies.Add (script);
  }

  void Update () {
    if (playersTurn || enemiesMoving || doingSetup) {
      return;
    }
    StartCoroutine (MoveEnemies ());
  }

  IEnumerator MoveEnemies() {
    enemiesMoving = true;
    Debug.Log("enemies.Count  "+ enemies.Count);
    yield return new WaitForSeconds (turnDelay);
    if (enemies.Count == 0) {
      yield return new WaitForSeconds (turnDelay);
    }

    for (int i = 0; i < enemies.Count; i++) {
      Debug.Log("ENEMIES: "+enemies[i]);
      if(enemies[i] != null) {
        enemies [i].MoveEnemy ();
      }
      yield return new WaitForSeconds (0.5f);
    }

    playersTurn = true;
    enemiesMoving = false;
  }
}
