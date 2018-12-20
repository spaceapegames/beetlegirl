using UnityEngine;
using System.Collections;
using System;

[RequireComponent (typeof (Rigidbody))]
public class BeetleGirl : MonoBehaviour
{
	public enum BeetleState
	{
		pathing, deathFreeze, deathFall, deaded
	}

	public SpriteRenderer spriteRenderer;
	private float deathFreezeEndTime;
	public Rigidbody rigid;
	private Vector2 fixedVelocity;
	public BeetleState state { private set; get; }
	public bool isMoving { private set; get; }
	public bool isAlive { private set; get; }
	private const float acceleration = 15f;
	public const float maxSpeed = 2.5f;
	private float endTime = 0;
	public GameObject fadeOutAnimation;

	public AudioClip SFXdeath;
	private Animator animator;

	public Action<BeetleState> OnStateChanged;

	private float UglyHackyMax = 2.5f;
	private float UglyHackyMin = -2.5f;

	public void SetMovement(bool newMovement)
	{
		if (isMoving != newMovement && state == BeetleState.pathing)
		{
			if (newMovement)
			{
				animator.Play ("Movement");
			}
			else
			{
				animator.Play ("Standing");
			}
		}
		isMoving = newMovement;
	}

	public void Kill()
	{
		Debug.Log ("(✿◕‿◕) mitsuru im dying");
		if (!isAlive)
			return;

		isAlive = false;
		rigid.isKinematic = true;
		SetState(BeetleState.deathFreeze);
		transform.Translate(0, 0, -2);
		deathFreezeEndTime = Time.time + 0.10f;

		animator.Play ("Death");
		SoundEffects.PlaySound(SoundEffectController.SoundTypes.death);

		MusicFade musicFade = FindObjectOfType<MusicFade>();
		if (musicFade != null)
			musicFade.DoFade();
	}

	void Start ()
	{
		if (spriteRenderer == null)
		{
			spriteRenderer = GetComponentInChildren<SpriteRenderer>();
			if (spriteRenderer == null)
			{
				Debug.LogError("(✿◕‿◕) Sort it out. No Sprite Renderer on Beetle Girl.");
			}
		}

		rigid = GetComponentInChildren<Rigidbody>();
		if (rigid == null)
		{
			Debug.LogError("(✿◕‿◕) Do me a favour. No Rigidbody on Beetle Girl.");
		}

		// Defaults
		isAlive = true;
		isMoving = true;
		fixedVelocity = Vector2.zero;
		SetState(BeetleState.pathing);

		animator = GetComponentInChildren<Animator>();
		animator.Play("Movement");


	}

	// Physics Update
	void FixedUpdate()
	{
		if (isAlive)
		{
			// This used to be a lot longer. I tried to be smart and I failed.
			bool collision = false;
			for (int i = 0; i < 3; i++)
			{
				RaycastHit info;

				if (Physics.Raycast(transform.position + new Vector3(-0.25f + (i * 0.25f), 0, 0), Vector3.down, out info, 0.5f))
				{
					Debug.DrawLine(transform.position + new Vector3(-0.25f + (i * 0.25f), 0, 0), transform.position + new Vector3(-0.25f + (i * 0.25f), -0.5f, 0f), Color.red, 1f);
					collision = true;
				}
			}
			rigid.AddForce(fixedVelocity, ForceMode.Impulse);
			fixedVelocity = Vector3.zero;

			// Clamp
			Vector3 velocity = rigid.velocity;
			velocity.x = Mathf.Clamp (velocity.x, -maxSpeed, maxSpeed);
			if (!collision)
			{
				velocity.x = 0;
			}
			rigid.velocity = velocity;

			if(transform.position.y > UglyHackyMax)
			{
				var newPos = transform.position;
				newPos.y = UglyHackyMin;
				transform.position = newPos;
				SetMovement(false);
			}
			else if(transform.position.y < UglyHackyMin)
			{
				var newPos = transform.position;
				newPos.y = UglyHackyMax;
				transform.position = newPos;
				SetMovement(false);
			}
			else
			{
				SetMovement(true);
			}
		}
		else
		{
			if (state == BeetleState.deathFreeze && Time.time > deathFreezeEndTime)
			{
				fixedVelocity.y = 0.25f;
				SetState(BeetleState.deathFall);
			}
			else if (state == BeetleState.deathFall)
			{
				Vector3 position = transform.position;
				fixedVelocity.y -= 1.0f * Time.fixedDeltaTime;
				position.y += fixedVelocity.y;
				transform.position = position;

				if (position.y < -10)
				{
					SetState(BeetleState.deaded);
				}

			}
		}
	}

	// Logic Update
	void Update ()
	{
		if (isAlive && isMoving)
		{
			fixedVelocity.x += acceleration * Time.deltaTime;
		}

		if (endTime <= 0 && !isAlive && state == BeetleState.deaded)
		{
			endTime = Time.time + 1.5f;
			GameObject go = Instantiate(fadeOutAnimation, Camera.main.transform.position + new Vector3(0, 0, 5), Quaternion.identity) as GameObject;
			go.transform.parent = Camera.main.transform;
			go.GetComponent<Animation>().Play();
		}

		if (endTime > 0 && Time.time > endTime)
		{
			Application.LoadLevel("Title");
		}
	}

	void SetState(BeetleState newState)
	{
		if(state != newState)
		{
			state = newState;
			if(OnStateChanged != null) OnStateChanged(state);
		}
	}
}
