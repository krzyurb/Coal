using System.Collections;
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


    public GameObject wallObstacle;

	private Transform boardHolder;
	private List <Vector3> gridPositions = new List<Vector3>();
    private List<int> coalPositions = new List<int>();

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
        for (int x = -1; x < columns + 1; x++) {
			for (int y = -1; y < rows + 1; y++) {
                WallObject wallTilesByLevel = wallTiles[level];
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
				Instantiate (trackTile, new Vector3 (columns - 2, y, 0f), Quaternion.identity);
			}
		}
	}

	void CarAtTop () {
		Instantiate (railCarUp,  new Vector3 (columns - 2, (rows - 2), 0f), Quaternion.identity);
		Instantiate (railCarDown, new Vector3 (columns - 2, (rows - 3), 0f), Quaternion.identity);
	}

	void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum) {
		int objectCount = Random.Range (minimum, maximum + 1);

		for(int i = 0; i < objectCount; i++) {
			Vector3 randomPosition = RandomPositions();
			GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];
			Instantiate (tileChoice, randomPosition, Quaternion.identity);
		}
	}

	public void SetupScene(int level) {
		BoardSetup (level);
		InitialiseList ();

		TracksAtTop ();
		CarAtTop ();

		ObjectAtRandom (coal, 1, 10);
		LayoutObjectAtRandom (enemyTiles, 1, 3);
		// CoalAtRandom (10, 22);
        // ObjectAtRandom(wallObstacle, 1, 2);
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
            Instantiate(coal, randomPosition, Quaternion.identity);
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
            Instantiate(tile, randomPosition, Quaternion.identity);
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
