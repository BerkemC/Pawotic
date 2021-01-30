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

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();	
	}

	private void LateUpdate()
	{
		Vector3 resultingVector = GetResultingMovementVector();
		rb.velocity = resultingVector * movementSpeed;

		if(Input.GetKeyDown(KeyCode.Space))
		{
			LoadManager.LoadTargetSceneStatic(0);
		}
	}

	private static Vector3 GetResultingMovementVector()
	{
		float inputHorizontal = Input.GetAxis(HORIZONTAL_AXIS);
		float inputVectical = Input.GetAxis(VERTICAL_AXIS);
		
		return new Vector3(inputHorizontal, inputVectical, 0f);
	}
}
