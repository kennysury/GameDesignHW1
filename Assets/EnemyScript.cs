using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

	public Sprite dieSprite;


	SpriteRenderer spriteRend;
	AudioSource dieSound;
	GameManager gm;
	ParticleSystem particle;
	int life = 2;

	// Use this for initialization
	void Start () {
		spriteRend = this.GetComponent<SpriteRenderer> ();
		dieSound = this.GetComponent<AudioSource> ();
		particle = this.GetComponent<ParticleSystem> ();
		gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D other){
		life--;
		if (life == 0)
			die ();
	}

	void die(){
		spriteRend.sprite = dieSprite;
		dieSound.Play ();
		gm.EnemyDie ();
		particle.Play ();
	}
}
