using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTheRock : MonoBehaviour {

	public rockScript TheRock;
	public float farLeft, farRight, farBot, farTop;

	void Update () {
		if (TheRock.rockState == rockScript.RockState.RELEASE || TheRock.rockState == rockScript.RockState.DEADBALL) {
			Vector3 newPos = transform.position;
			newPos.x = TheRock.transform.position.x;
			newPos.x = Mathf.Clamp (newPos.x, farLeft, farRight);
			newPos.y = TheRock.transform.position.y;
			newPos.y = Mathf.Clamp (newPos.y, farBot, farTop);
			transform.position = newPos;
		} else {
			Vector3 newPos = transform.position;
			newPos.x = farLeft;
			newPos.y = farBot;
			transform.position = newPos;
		}
	}
}
