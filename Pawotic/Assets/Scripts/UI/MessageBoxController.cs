using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animation))]
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
	private float currentMessageDisplayTime;

	private void Awake()
	{
		Instance = this;
		animation = GetComponent<Animation>();
	}

	public void DisplayMessage(string message)
	{
		animation.clip = openClip;
		animation.Play();
		messageText = message;
		currentMessageDisplayTime = 0f;
		StartCoroutine(WriteMessage());
	}

	private void StopDisplayingMessage()
	{
		animation.clip = closeClip;
		animation.Play();
	}

	private void Update()
	{
		if(currentMessageDisplayTime < messageDisplayTime)
		{
			currentMessageDisplayTime += Time.deltaTime;
			if(currentMessageDisplayTime >= messageDisplayTime)
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
