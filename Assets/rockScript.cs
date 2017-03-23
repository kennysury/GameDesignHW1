using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rockScript : MonoBehaviour {

	public Camera MainCamera;
	public float rockRadius;
	public float ForceMagn;
	public Vector3[] FrontElasticPoints, BackElasticPoints;
	public float MaxY, MinY, MaxX, MinX;

	GameManager gm;
	LineRenderer frontElastic, backElastic;
	Rigidbody2D rigid;
	Vector3 midPoint;
	Vector3 lastPos, movement;
	float shootTime = 0f;

	AudioSource shootSound, collisionSound, punchSound;

	public enum RockState
	{
		IDEL, HOLD, RELEASE, DEADBALL
	};
	public RockState rockState = RockState.IDEL;

	Vector3 screenPoint, offset;

	// Use this for initialization
	void Start () {
		midPoint = (FrontElasticPoints [0] + BackElasticPoints [0]) / 2f;
		rigid = this.GetComponent<Rigidbody2D> ();
		MainCamera = Camera.main;
		gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		frontElastic = GameObject.Find ("FrontElastic").GetComponent<LineRenderer>();
		backElastic = GameObject.Find ("BackElastic").GetComponent<LineRenderer> ();
		shootSound = this.GetComponents<AudioSource> () [0];
		collisionSound = this.GetComponents<AudioSource> () [1];
		punchSound = this.GetComponents<AudioSource> () [2];
		frontElastic.sortingLayerName = "CatapultFront";
		backElastic.sortingLayerName = "CatapultBack";
	}
	
	// Update is called once per frame
	void Update () {
		if (rockState == RockState.IDEL) {
			transform.position = midPoint;
			resetElastic ();
		} else if (rockState == RockState.HOLD) {
			Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z) - offset;
			Vector3 curPos = MainCamera.ScreenToWorldPoint (curScreenPoint);
			curPos.x = Mathf.Clamp (curPos.x, MinX, MaxX);
			curPos.y = Mathf.Clamp (curPos.y, MinY, MaxY);
			transform.position = curPos;
			setElastic ();
		} else if (rockState == RockState.RELEASE) {
			resetElastic ();
			if ((rigid.velocity.magnitude <= 0.1f && shootTime > 8f) || shootTime > 15f) {
				gm.NextShoot ();
				rockState = RockState.DEADBALL;
			} else {
				shootTime += Time.deltaTime;
			}
		}

		frontElastic.SetPositions (FrontElasticPoints);
		backElastic.SetPositions (BackElasticPoints);

		movement = transform.position - lastPos;
		lastPos = transform.position;
	}

	void OnMouseDown(){
		if (rockState != RockState.IDEL)
			return;
		rockState = RockState.HOLD;

		screenPoint = MainCamera.WorldToScreenPoint (transform.position);
		offset = Input.mousePosition - screenPoint;
		offset.z = 0f;
	}

	void OnMouseUp(){
		if (rockState != RockState.HOLD)
			return;
		release ();
	}

	void release(){
		float force = Mathf.Sqrt( (transform.position - midPoint).magnitude );
		Vector3 direct3 = midPoint - transform.position;
		Vector2 direct = new Vector2 (direct3.x, direct3.y).normalized;
		if (force > 1.2f) {
			rockState = RockState.RELEASE;
			rigid.gravityScale = 1.0f;
			rigid.AddForce (force*direct*ForceMagn);
			shootSound.Play ();
		} else {
			rockState = RockState.IDEL;
		}
	}

	void resetElastic(){
		FrontElasticPoints [1] = midPoint;
		BackElasticPoints [1] = midPoint;
	}

	void setElastic(){
		float length = (transform.position - midPoint).magnitude + rockRadius;
		Vector3 holderPos = midPoint + length * (transform.position - midPoint).normalized;
		FrontElasticPoints [1] = holderPos;
		BackElasticPoints [1] = holderPos;
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "ENEMY") {
			punchSound.time = 0.2f;
			punchSound.Play ();
		} else {
			collisionSound.time = 0f;
			collisionSound.Play ();
		}
	}
}
