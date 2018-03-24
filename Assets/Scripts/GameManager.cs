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
		InitGame();
	}

	void OnSceneLoaded (Scene previousScene, Scene newScene) {
		level++;
	}

	void InitGame () {
    doingSetup = true;
    Invoke("HideLevelImage", levelStartDelay);
		enemies.Clear ();
		boardScript.SetupScene (level);
	}

  void HideLevelImage() {
    doingSetup = false;
  }

  public void GameOver() {
    enabled = false;
  }

  public void AddEnemyToList (Enemy script) {
    enemies.Add (script);
  }


  void Update () {
    // Debug.Log("playersTurn || enemiesMoving "+ (playersTurn || enemiesMoving));
    if (playersTurn || enemiesMoving || doingSetup) {
      return;
    }
    StartCoroutine (MoveEnemies ());
  }

  IEnumerator MoveEnemies() {
    Debug.Log("MOVE ENEMIES");
    enemiesMoving = true;
    yield return new WaitForSeconds (turnDelay);
    Debug.Log(enemies.Count);
    if (enemies.Count == 0) {
      yield return new WaitForSeconds (turnDelay);
    }

    for (int i = 0; i < enemies.Count; i++) {
      enemies [i].MoveEnemy ();
      yield return new WaitForSeconds (enemies[i].moveTime);
    }

    playersTurn = true;
    enemiesMoving = false;
  }
}
