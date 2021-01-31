using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(AudioSource))]
public class MessageBoxController : MonoBehaviour
{
	private static MessageBoxController inst;

	public static MessageBoxController Instance
	{
		get => inst;
		set => inst = value;
	}

	[SerializeField]
	private Text textBox;
	[SerializeField]
	private float perLetterTime;
	[SerializeField]
	private float messageDisplayTime;
	[SerializeField]
	private AnimationClip openClip;
	[SerializeField]
	private AnimationClip closeClip;
	private Animation animation;
	private string messageText;
	private Queue<string> queue;
	private float currentMessageDisplayTime;
	private AudioSource audioSource;

	private void Awake()
	{
		Instance = this;
		animation = GetComponent<Animation>();
		currentMessageDisplayTime = messageDisplayTime;
		queue = new Queue<string>();
		audioSource = GetComponent<AudioSource>();
	}

	public void DisplayMessage(string message, bool forcePlay = false)
	{
		if ((queue.Count == 0 && currentMessageDisplayTime >= messageDisplayTime) || forcePlay)
		{
			animation.clip = openClip;
			animation.Play();
			messageText = message;
			currentMessageDisplayTime = 0f;
			StartCoroutine(WriteMessage());
			audioSource.Play();
		}
		else
		{
			queue.Enqueue(message);
		}
	}

	private void StopDisplayingMessage()
	{
		animation.clip = closeClip;
		animation.Play();
		if (queue.Count > 0)
		{
			DisplayMessage(queue.Dequeue(), true);
		}
	}

	private void Update()
	{
		if (currentMessageDisplayTime < messageDisplayTime)
		{
			currentMessageDisplayTime += Time.deltaTime;
			if (currentMessageDisplayTime >= messageDisplayTime)
			{
				StopDisplayingMessage();
			}
		}
	}

	private IEnumerator WriteMessage()
	{
		textBox.text = "";
		for (int i = 0; i < messageText.Length; i++)
		{
			textBox.text += messageText[i];
			yield return new WaitForSeconds(perLetterTime);
		}
		yield return null;
	}
}
