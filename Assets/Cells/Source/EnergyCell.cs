using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class EnergyCell : Cell {

	public Vector2 RespawnTime = new Vector2(15.0f, 30.0f);

	float nextRespawnTime;
	float timer;

	MeshRenderer meshRenderer;

	void Start() {
		meshRenderer = this.GetComponent<MeshRenderer> ();
		IsActive = true;
	}

	void Update() {
		if (!IsActive) {
			timer += Time.deltaTime;

			if (timer > nextRespawnTime) {
				IsActive = true;
				meshRenderer.enabled = true;
				timer = 0;
			}
		}
	}

	public override void RpcOnEaten() {
		nextRespawnTime = Random.Range (RespawnTime.x, RespawnTime.y);
		StartCoroutine (OnEaten ());
	}

	public override IEnumerator OnEaten() {
		IsActive = false;
		meshRenderer.enabled = false;
		yield return null;
	}
}
