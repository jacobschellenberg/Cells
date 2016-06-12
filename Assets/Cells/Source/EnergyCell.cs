using UnityEngine;
using System.Collections;

public class EnergyCell : Cell {

	public Vector2 RespawnTime = new Vector2(15.0f, 30.0f);

	public override IEnumerator OnEaten() {
		this.gameObject.SetActive (false);
		yield return new WaitForSeconds (Random.Range(RespawnTime.x, RespawnTime.y));
		this.gameObject.SetActive (true);
	}
}
