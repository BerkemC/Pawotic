using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadManager : MonoBehaviour
{
	private const int LoadingScene = 1;
	private static int targetScene = -1;

	[SerializeField]
	private Slider progressSlider;
	[SerializeField]
	private bool dontLoadSceneOnAwake;

	private void Awake()
	{
		if(targetScene == -1 || dontLoadSceneOnAwake)
		{
			return;
		}

		StartCoroutine(LoadSceneAsync(targetScene));
	}

	public static void LoadTargetSceneStatic(int sceneIndex)
	{
		Load(sceneIndex);
	}

	public void LoadTargetScene(int sceneIndex)
	{
		Load(sceneIndex);
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	private static void Load(int sceneIndex)
	{
		targetScene = sceneIndex;
		SceneManager.LoadScene(LoadingScene);
	}

	IEnumerator LoadSceneAsync(int sceneIndex)
	{
		AsyncOperation progress = SceneManager.LoadSceneAsync(sceneIndex);

		while (!progress.isDone)
		{
			float value = progress.progress / 0.9f;
			progressSlider.value = value;

			yield return null;
		}
		targetScene = -1;
	}
}