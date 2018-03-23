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

	public GameObject   floorTile;
	public GameObject[] wallTiles;

	public GameObject   leftGateTile;
	public GameObject   centerGateTile;
	public GameObject   rightGateTile;

	public GameObject   trackTile;

	private Transform boardHolder;
	private List <Vector3> gridPositions = new List<Vector3>();

	void InitialiseList() {
		gridPositions.Clear ();

		for (int x = 1; x < columns - 1; x++) {
			for (int y = 1; y < rows - 1; y++) {
				gridPositions.Add (new Vector3 (x, y, 0f));
			}
		}
	}

	void BoardSetup () {
		boardHolder = new GameObject ("Board").transform;

		for (int x = -1; x < columns + 1; x++) {
			for (int y = -1; y < rows + 1; y++) {
				GameObject toInstantiate = floorTile;

				if (x == -1 || x == columns || y == -1 || y == rows) {
					toInstantiate = wallTiles[Random.Range(0,wallTiles.Length)];
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
			if ((rows - y) > 0 && (rows - y) < 7) {
				Instantiate (trackTile, new Vector3 (columns - 2, y, 0f), Quaternion.identity);
			}
		}
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
		BoardSetup ();
		InitialiseList ();
		TracksAtTop ();
	}
}
