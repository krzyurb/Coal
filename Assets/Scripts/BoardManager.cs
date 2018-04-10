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

	[Serializable]
	public class WallObject {
		public GameObject[] wallTiles;
		public GameObject   floorTile;
	}

	private int columns;
	private int rows;

	public GameObject player;
    public WallObject[] wallTiles;
	public GameObject[] enemyTiles;
	public GameObject  leftGateTile;
	public GameObject  centerGateTile;
	public GameObject  rightGateTile;
	public GameObject  trackTile;
	public GameObject  railCarUp;
	public GameObject  railCarDown;
	public GameObject  coal;
	public GameObject  piwko;
	public GameObject  diament;

	private Transform boardHolder;
	private List <Vector3> gridPositions = new List<Vector3> ();

	public void SetupScene (int level) {
		BoardSetup (level);
		InitialiseGridPositionsList ();

		if (level <= 6) {
			PlaceCoalAtRandomPositions (2, 5);
			PlaceObjectsAtRandomPositions (enemyTiles, 0, 2);
		} else {
			PlaceCoalAtRandomPositions ((int)(rows * rows * 0.1), (int)((rows - 1) * (rows - 1) * 0.35) - 4);
			PlaceObjectAtRandomPositions (piwko, 0, (int)(rows / 3));
			PlaceObjectAtRandomPositions (diament, 1, rows);
			PlaceObjectsAtRandomPositions (enemyTiles, 1, (int)(rows / 3));
		}

		GameObject instance = Instantiate (player, new Vector3 (columns - 3, rows - 1, 0f), Quaternion.identity);
		instance.transform.SetParent (boardHolder);
	}
		
	private void BoardSetup (int level) {
		Destroy (GameObject.Find ("Board"));
		SetBoardSize (level);

		boardHolder = new GameObject ("Board").transform;
		int levelType = Random.Range (0, 2);

		for (int x = -1; x < columns + 1; x++) {
			for (int y = -1; y < rows + 1; y++) {
				WallObject wallTilesByLevel = wallTiles[levelType];
				GameObject toInstantiate 	= wallTilesByLevel.floorTile;

				if (x == -1 || x == columns || y == -1 || y == rows) {
					GameObject[] wall = wallTilesByLevel.wallTiles;
					toInstantiate     = wall[Random.Range(0, wall.Length)];
				}

				if (y == rows) {
					if (x == columns - 3) toInstantiate = leftGateTile;
					if (x == columns - 2) toInstantiate = centerGateTile;
					if (x == columns - 1) toInstantiate = rightGateTile;
				}

				GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity);
				instance.transform.SetParent(boardHolder);
			}
		}

		CreateTracksTiles ();
		CreateCarTiles ();
	}

	private void SetBoardSize (int level) {
		if (level <= 6)
			columns = rows = 6;
		else
			columns = rows = 5 + (int)(0.5 * level);
	}

	void CreateTracksTiles () {
		for (int y = 0; y < rows + 1; y++)
			if ((rows - y) > 0 && (rows - y) < 5)
				Instantiate (trackTile, new Vector3 (columns - 2, y, 0f), Quaternion.identity).transform.SetParent(boardHolder);
	}

	void CreateCarTiles () {
		Instantiate (railCarUp,   new Vector3 (columns - 2, (rows - 2), 0f), Quaternion.identity).transform.SetParent(boardHolder);
		Instantiate (railCarDown, new Vector3 (columns - 2, (rows - 3), 0f), Quaternion.identity).transform.SetParent(boardHolder);
	}

	void InitialiseGridPositionsList () {
		gridPositions.Clear ();

		for (int x = 1; x < columns - 1; x++) {
			for (int y = 1; y < rows - 1; y++) {
				if (!(x == columns - 2 && ((rows - y) > 0 && (rows - y) < 7)) && !(x == columns - 3 && y == rows - 2)) // TODO: Make it cleaner
					gridPositions.Add (new Vector3 (x, y, 0f));
			}
		}
	}

	Vector3 RandomPositions () {
		int randomIndex        = Random.Range (0, gridPositions.Count);
		Vector3 randomPosition = gridPositions [randomIndex];
		gridPositions.RemoveAt (randomIndex);

		return randomPosition;
	}
		
    private void PlaceCoalAtRandomPositions (int minimum, int maximum) {
        int objectCount = Random.Range (minimum, maximum + 1);

        for (int i = 0; i < objectCount; i++) {
            int randomIndex = Random.Range (0, gridPositions.Count);
            Vector3 randomPosition = gridPositions[randomIndex];
            gridPositions.RemoveAt (randomIndex);

			Instantiate (coal, randomPosition, Quaternion.identity).transform.SetParent (boardHolder);
        }
    }

	void PlaceObjectsAtRandomPositions (GameObject[] tileArray, int minimum, int maximum) {
		int objectCount = Random.Range (minimum, maximum + 1);

		for (int i = 0; i < objectCount; i++) {
			Vector3 randomPosition = RandomPositions ();
			GameObject tileChoice  = tileArray[Random.Range (0, tileArray.Length)];
			Instantiate (tileChoice, randomPosition, Quaternion.identity).transform.SetParent (boardHolder);
		}
	}

	List<Vector3> PlaceObjectAtRandomPositions (GameObject gameObject, int minimum, int maximum) {
        int objectCount = Random.Range (minimum, maximum + 1);
        List<Vector3> objectReservedPositions = new List<Vector3> ();

        for (int i = 0; i < objectCount; i++) {
			int randomIndex = Random.Range (0, gridPositions.Count);
			Vector3 randomPosition = gridPositions[randomIndex];
			gridPositions.RemoveAt (randomIndex);

			Instantiate (gameObject, randomPosition, Quaternion.identity).transform.SetParent (boardHolder);
        }

        return objectReservedPositions;
    }
}