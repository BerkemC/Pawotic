using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioCard : MonoBehaviour
{
    private ScenarioCardData data;

    [SerializeField]
    private Text descriptionField;

    public void Initialise(ScenarioCardData scenarioCardData)
	{
		data = scenarioCardData;
		InitialiseUI();
	}

	private void InitialiseUI()
	{
		descriptionField.text = data.cardDescription;
	}
}
