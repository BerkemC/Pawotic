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

	[SerializeField]
	private AnimationCurve accelerationCurve;

	private void Awake()
	{
		player = FindObjectOfType<PlayerController>();
	}

	private void LateUpdate()
	{
		Vector3 playerPosition = player.transform.position;
		Vector3 direction = playerPosition - transform.position;
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
