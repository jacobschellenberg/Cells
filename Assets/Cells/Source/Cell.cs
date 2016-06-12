using UnityEngine;
using System.Collections;

public abstract class Cell : MonoBehaviour {

	public float Size = 0.5f; // 1 is the default cell size at 0.5 radius.
	public float Worth = 0.1f;

	public virtual float GetWorth() {
		return Worth;
	}

	public abstract IEnumerator OnEaten ();

	/// <summary>
	/// Determines whether the specified player cell can eat this cell.
	/// </summary>
	/// <returns><c>true</c> if specified cell can eat this cell; otherwise, <c>false</c>.</returns>
	/// <param name="playerCell">Player cell.</param>
	public bool CanEat(Cell playerCell) {
		Debug.Log ("Player Cell Size: " + playerCell.Size);
		Debug.Log ("Cell Size: " + Size);
		Debug.Log ("Calculated Size: " + (Size * 2));

		if (playerCell.Size >= (Size * 2)) {
			return true;
		}

		return false;
	}
}
