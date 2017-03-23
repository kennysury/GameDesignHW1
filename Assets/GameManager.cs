using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public int[] TotalLife;
	public int[] TotalEnemy;
	public int StageNum;
	public GameObject RockPrefab;
	public GameObject[] EnemyHouses;
	public Camera MainCamera;
	public GameObject Rocks;
	public GameObject[] LifeImages;

	public Text VicText;
	public Button NextStageBut, RetryBut;

	private int lifeNum, enemyNum;
	private GameObject theEnemyHouse;
	private bool isStageOver = false;

	// Use this for initialization
	void Start () {
		NextStageBut.onClick.AddListener (() => nextstage());
		RetryBut.onClick.AddListener (() => retry());
		spawnEnemyHouse ();
		NextShoot ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void victroy(){
		isStageOver = true;
		if (StageNum == EnemyHouses.Length - 1) {
			VicText.text = "ALL CLEAR!!";
		} else {
			VicText.text = "VICTORY!!";
			NextStageBut.gameObject.SetActive (true);
		}
	}

	void fail(){
		VicText.text = "殘念!!";
		RetryBut.gameObject.SetActive (true);
	}

	void nextstage(){
		StageNum++;
		VicText.text = "";
		NextStageBut.gameObject.SetActive (false);
		spawnEnemyHouse ();
		destroyRocks ();
		NextShoot ();
	}

	void retry(){
		VicText.text = "";
		RetryBut.gameObject.SetActive (false);
		spawnEnemyHouse ();
		destroyRocks ();
		NextShoot ();
	}

	void destroyRocks(){
		foreach(Transform child in Rocks.transform){
			Destroy (child.gameObject);
		}
	}

	void spawnEnemyHouse(){
		if (theEnemyHouse != null) {
			Destroy (theEnemyHouse);
		}
		lifeNum = TotalLife[StageNum];
		enemyNum = TotalEnemy[StageNum];

		for (int i = 0; i < LifeImages.Length; i++) {
			if (i < lifeNum)
				LifeImages [i].SetActive (true);
			else
				LifeImages [i].SetActive (false);
		}

		isStageOver = false;
		theEnemyHouse = GameObject.Instantiate (EnemyHouses [StageNum]);
	}

	public void NextShoot(){
		if (isStageOver)
			return;
		if (lifeNum > 0) {
			GameObject newRock = GameObject.Instantiate (RockPrefab);
			MainCamera.GetComponent<FollowTheRock> ().TheRock = newRock.GetComponent<rockScript>();
			newRock.transform.parent = Rocks.transform;
			lifeNum--;
			LifeImages [lifeNum].SetActive (false);
		} else {
			fail ();
		}
	}

	public void EnemyDie(){
		enemyNum--;
		if (enemyNum == 0) {
			victroy ();
		}
	}
}
