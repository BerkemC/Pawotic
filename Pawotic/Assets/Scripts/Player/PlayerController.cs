using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
	private const string VERTICAL_AXIS = "Vertical";
	private const string HORIZONTAL_AXIS = "Horizontal";

	[SerializeField]
	private float movementSpeed;

	private Rigidbody2D rb;
	private Animation animation;
	private Camera mainCamera;
	[SerializeField]
	private SpriteRenderer renderer;
	private bool controlState;
	[SerializeField]
	private Sprite front;
	[SerializeField]
	private Sprite back;
	[SerializeField]
	private Sprite right;
	[SerializeField]
	private Sprite left;
	private Vector3 resultingVector;

	public Vector3 ResultingVector { get => resultingVector; set => resultingVector = value; }

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		animation = GetComponent<Animation>();
		mainCamera = Camera.main;
		controlState = true;
	}

	private void Update()
	{
		if (!controlState)
		{
			return;
		}

		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = mainCamera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.transform.position.z));
			if (Physics.Raycast(ray, out RaycastHit hit))
			{
				Scenario scenario = hit.transform.GetComponent<Scenario>();
				if (scenario != null)
				{
					scenario.Initialise();
				}
			}
		}
	}

	private void LateUpdate()
	{
		if (!controlState)
		{
			return;
		}

		resultingVector = GetResultingMovementVector();
		rb.velocity = (resultingVector * movementSpeed);

		if(resultingVector.y > 0f)
		{
			renderer.sprite = back;
		}
		else if(resultingVector.y < 0f)
		{
			renderer.sprite = front;
		}
		else if(resultingVector.x > 0f)
		{
			renderer.sprite = right;
		}
		else if(resultingVector.x < 0f)
		{
			renderer.sprite = left;
		}

		if (resultingVector.sqrMagnitude > (0.1f * 0.1f))
		{
			if (!animation.isPlaying)
			{
				animation.Play();
			}
		}
		else
		{
			if (animation.isPlaying)
			{
				animation.Stop();
			}
		}
	}

	private static Vector3 GetResultingMovementVector()
	{
		float inputHorizontal = Input.GetAxis(HORIZONTAL_AXIS);
		float inputVectical = Input.GetAxis(VERTICAL_AXIS);

		return new Vector3(inputHorizontal, inputVectical, 0f);
	}

	public void SetControlState(bool state)
	{
		controlState = state;

		if (!controlState)
		{
			rb.velocity = Vector3.zero;
			if (animation.isPlaying)
			{
				animation.Stop();
			}
		}
	}
}
