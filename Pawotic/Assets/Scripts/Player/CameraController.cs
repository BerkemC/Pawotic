using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	private const float StopThreshold = 0.1f;
	private const float StopThresholdPwr = StopThreshold * StopThreshold;

	private PlayerController player;

	[SerializeField]
	private float cameraSpeed;
	[SerializeField]
	private float fullSpeedTime;
	private float currentSpeedTime;
	private Vector3 positionOffset;

	[SerializeField]
	private AnimationCurve accelerationCurve;

	private void Awake()
	{
		player = FindObjectOfType<PlayerController>();
		positionOffset = player.transform.position - transform.position;
	}

	private void LateUpdate()
	{
		Vector3 playerPosition = player.transform.position;
		Vector3 direction = (playerPosition - transform.position) + (player.ResultingVector.y >= 0 ? -positionOffset : positionOffset);
		direction.z = 0f;

		if (direction.sqrMagnitude <= StopThresholdPwr)
		{
			currentSpeedTime = 0f;
		}
		else
		{
			Vector3 normalisedDirection = direction.normalized;
			float stepSize = cameraSpeed * accelerationCurve.Evaluate(currentSpeedTime / fullSpeedTime) * Time.deltaTime;
			transform.position += stepSize * normalisedDirection;
			currentSpeedTime += Time.deltaTime;
		}
	}
}
