﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerCell : Cell {

	public int ID;
	public bool IsAlive = false;
	public float CameraDistance = 15;
	public float CameraDistanceIncreaseRate = 0.1f;
	public float MinMoveSpeed = 0.1f;
	public float MoveSpeed = 10; // The smaller the cell, the quicker it moves.
	public float CellsEaten = 0;
	public float GrowthRate = 0.1f; // Grow at this rate for every cell eaten.
	public float SpeedReductionRate = 0.05f;

	CameraController cameraController;
	GameController gameController;

	NetworkIdentity networkIdentity;

	public override void OnStartLocalPlayer()
	{
		GetComponent<MeshRenderer>().material.color = Color.red;
	}

	void Awake() {
		networkIdentity = this.GetComponent<NetworkIdentity> ();	
	}

	void Start() {
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();

//		if (ID == gameController.ClientId) {
		if(networkIdentity.isLocalPlayer) {
			cameraController = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraController> ();

			cameraController.SetTarget (this.transform);
			cameraController.SetDistance (-CameraDistance);
		}

		if(isServer) {
			gameController.SpawnEnergyCells (); // if server
		}
	}

	void Update() {
//		if (ID == gameController.ClientId) {
		if(networkIdentity.isLocalPlayer) {
			float vertical = Input.GetAxis ("Vertical");
			float horizontal = Input.GetAxis ("Horizontal");

			if (vertical > 0 && this.transform.position.y < (gameController.bounds.y / 2) - 1) {
				this.transform.Translate (Vector3.up * MoveSpeed * Time.deltaTime);
			} else if (vertical < 0 && this.transform.position.y > (-gameController.bounds.y / 2) + 1) {
				this.transform.Translate (Vector3.down * MoveSpeed * Time.deltaTime);
			}

			if (horizontal > 0 && this.transform.position.x < (gameController.bounds.x / 2) - 1) {
				this.transform.Translate (Vector3.right * MoveSpeed * Time.deltaTime);
			} else if (horizontal < 0 && this.transform.position.x > (-gameController.bounds.x / 2) + 1) {
				this.transform.Translate (Vector3.left * MoveSpeed * Time.deltaTime);
			}
		}
	}

	public void OnInitialize(int id) {
		MoveSpeed = 10;
		Size = 1;
		CellsEaten = 0;
		GrowthRate = 0.1f;
		IsAlive = true;
		ID = id;

		this.transform.localScale = new Vector3 (Size, Size, 0.1f);
	}

	void Grow(float amount) {
		Size += amount;

		if (MoveSpeed > MinMoveSpeed) {
			MoveSpeed -= SpeedReductionRate;
		}

		if (networkIdentity.isLocalPlayer) {
			CameraDistance += CameraDistanceIncreaseRate;
			cameraController.SetDistance (-CameraDistance);
		}

		Vector3 newSize = this.transform.localScale;
		newSize.x += amount;
		newSize.y += amount;

		this.transform.localScale = newSize;
	}

	void OnEatCell(Cell cell) {
		CellsEaten++;
		Worth += GetWorth(cell);
		Grow (GetWorth(cell));
	}
	 
	public override void RpcOnEaten() {
		StartCoroutine (OnEaten ());
	}

	public override IEnumerator OnEaten() {
		IsAlive = false;
		Destroy (this.gameObject);
		yield return null;
	}

	void OnTriggerEnter(Collider hit) {
//		if (ID == gameController.ClientId) {
//		if(networkIdentity.isLocalPlayer) {
			Cell cell = hit.GetComponent<Cell> ();

		if (cell != null && cell.CanEat(this) && cell.IsActive) {
			OnEatCell (cell);
			cell.RpcOnEaten ();
		}
//		}
	}
}
