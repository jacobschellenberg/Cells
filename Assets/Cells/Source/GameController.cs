using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public int ClientId = 0;
	public Vector2 bounds = new Vector2(1000, 1000);
	public Transform background;
	public PlayerCell playerCellPrefab;
	public EnergyCell energyCellPrefab;
	public float energyCellRatio = 0.10f;
	public int maxEnergyCells = 100;
	public float distanceFromBounds = 1.5f;

	Transform energyCellParent;
	List<PlayerCell> playerCells = new List<PlayerCell>();
	int totalCells;
	bool playerInitialized = false;

	void Start() {
		background.localScale = new Vector3 (bounds.x, bounds.y, 1);

		energyCellParent = new GameObject ("Energy Cells").transform;
		energyCellParent.position = Vector3.zero;

		maxEnergyCells = (int)(((bounds.x + bounds.y) / 2) * energyCellRatio);

		for(int i = 0; i < maxEnergyCells; i++) {
			SpawnEnergyCell (i);
		}
	}

	void Update() {
		if (!playerInitialized && Input.GetKeyDown(KeyCode.Return)) {
			NewPlayer ();
		}	
	}

	void SpawnEnergyCell(int id) {
		var energyCell = Instantiate (energyCellPrefab.gameObject, new Vector3 (Random.Range (-bounds.x / 2 + distanceFromBounds, bounds.x / 2 - distanceFromBounds), Random.Range (-bounds.y / 2 + distanceFromBounds, bounds.y / 2 - distanceFromBounds), 0), Quaternion.identity) as GameObject;
		energyCell.transform.SetParent (energyCellParent);
		energyCell.name = "Energy Cell: " + id;
	}

	void NewPlayer() {
		var playerCell = Instantiate(playerCellPrefab.gameObject, new Vector3 (Random.Range (-bounds.x / 2 + distanceFromBounds, bounds.x / 2 - distanceFromBounds), Random.Range (-bounds.y / 2 + distanceFromBounds, bounds.y / 2 - distanceFromBounds), 0), Quaternion.identity) as GameObject;
		playerCell.name = "Player Cell: " + totalCells;

		var cell = playerCell.GetComponent<PlayerCell> ();
		cell.OnInitialize (totalCells);

		playerCells.Add (cell);
		totalCells++;
	}
}
