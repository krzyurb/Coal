﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

	[Serializable]
	public class Count {
		public int maximum;
		public int minimum;

		public Count (int min, int max) {
			minimum = min;
			maximum = max;
		}
	}

	public int columns = 8;
	public int rows    = 8;

    public WallObject[] wallTiles;
	public GameObject[] enemyTiles;

	public GameObject   leftGateTile;
	public GameObject   centerGateTile;
	public GameObject   rightGateTile;

	public GameObject   coal;
	public GameObject   trackTile;

	public GameObject   railCarUp;
	public GameObject   railCarDown;

	public GameObject   player;

    public GameObject wallObstacle;

	private Transform boardHolder;
	private List <Vector3> gridPositions = new List<Vector3>();
    private List<int> coalPositions = new List<int>();
	private int levelID;

    void InitialiseList() {
		gridPositions.Clear ();

		for (int x = 1; x < columns - 1; x++) {
			for (int y = 1; y < rows - 1; y++) {
				if (x == columns - 2 && ((rows - y) > 0 && (rows - y) < 7)) { }
				else
					gridPositions.Add (new Vector3 (x, y, 0f));
			}
		}
	}

	void BoardSetup (int level) {
		boardHolder = new GameObject ("Board").transform;
		levelID = Random.Range (0, 2);

        for (int x = -1; x < columns + 1; x++) {
			for (int y = -1; y < rows + 1; y++) {
				WallObject wallTilesByLevel = wallTiles[levelID];
                GameObject toInstantiate = wallTilesByLevel.floorTile;

                if (x == -1 || x == columns || y == -1 || y == rows) {
                    GameObject[] wall = wallTilesByLevel.wallTiles;
                    toInstantiate = wall[Random.Range(0, wall.Length)];
                }

                if (y == rows) { // dodawanie bramy
					if (x == columns - 3) toInstantiate = leftGateTile; // brama lewa
					if (x == columns - 2) toInstantiate = centerGateTile; // brama środek
					if (x == columns - 1) toInstantiate = rightGateTile; // brama prawa
				}

				GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity);
				instance.transform.SetParent(boardHolder);
			}
		}
	}

	Vector3 RandomPositions () {
		int randomIndex = Random.Range (0, gridPositions.Count);
		Vector3 randomPosition = gridPositions [randomIndex];
		gridPositions.RemoveAt (randomIndex);
		return randomPosition;
	}

	void TracksAtTop () {
		for (int y = 0; y < rows + 1; y++) {
			if ((rows - y) > 0 && (rows - y) < 5) {
				GameObject instance = Instantiate (trackTile, new Vector3 (columns - 2, y, 0f), Quaternion.identity);
				instance.transform.SetParent(boardHolder);
			}
		}
	}

	void CarAtTop () {
		GameObject instance1 = Instantiate (railCarUp,  new Vector3 (columns - 2, (rows - 2), 0f), Quaternion.identity);
		instance1.transform.SetParent(boardHolder);
		GameObject instance2 = Instantiate (railCarDown, new Vector3 (columns - 2, (rows - 3), 0f), Quaternion.identity);
		instance2.transform.SetParent(boardHolder);
	}

	void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum) {
		int objectCount = Random.Range (minimum, maximum + 1);

		for(int i = 0; i < objectCount; i++) {
			Vector3 randomPosition = RandomPositions();
			GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];
			GameObject instance = Instantiate (tileChoice, randomPosition, Quaternion.identity);
			instance.transform.SetParent(boardHolder);
		}
	}

	public void SetupScene(int level) {
		Destroy (GameObject.Find ("Board"));

		if (level <= 6)
			columns = rows = 6;
		else
			columns = rows = 5 + (int)(0.5 * level);

		BoardSetup (level);
		InitialiseList ();

	
		ObjectAtRandom (coal, 1, 10);
		LayoutObjectAtRandom (enemyTiles, 1, 3);

		TracksAtTop ();
		CarAtTop ();

		GameObject instance = Instantiate (player, new Vector3 (columns - 3, rows - 1, 0f), Quaternion.identity);
		instance.transform.SetParent(boardHolder);
	}

    private void CoalAtRandom(int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);
        for (int i = 0; i < objectCount; i++)
        {
            int randomIndex = Random.Range(0, gridPositions.Count);
            Vector3 randomPosition = gridPositions[randomIndex];
            gridPositions.RemoveAt(randomIndex);

            coalPositions.Add(randomIndex);
			GameObject instance = Instantiate(coal, randomPosition, Quaternion.identity);
			instance.transform.SetParent(boardHolder);
        }
    }

    List<Vector3> ObjectAtRandom(GameObject tile, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);
        List<Vector3> objectReservedPositions = new List<Vector3>();
        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPositions();
            objectReservedPositions.Add(randomPosition);
			GameObject instance = Instantiate(tile, randomPosition, Quaternion.identity);
			instance.transform.SetParent(boardHolder);
        }
        return objectReservedPositions;
    }

    [Serializable]
    public class WallObject
    {
        public GameObject[] wallTiles;
        public GameObject floorTile;
    }
}
