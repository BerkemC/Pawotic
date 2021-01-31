using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Scenario))]
public class FinalScenario : MonoBehaviour
{
	private Scenario finalScenario;
	private Scenario[] allScenarios;
	[SerializeField]
	private GameObject[] componentsToToggle;


	private void Awake()
	{
		allScenarios = FindObjectsOfType<Scenario>(true);
		finalScenario = GetComponent<Scenario>();
		SetReferencedObjectStates(false);
	}

	private void SetReferencedObjectStates(bool state)
	{
		for (int i = 0; i < componentsToToggle.Length; i++)
		{
			ref GameObject comp = ref componentsToToggle[i];
			comp.SetActive(state);
		}
	}

	public void CheckAllScenarioCompletions()
	{
		bool isAllCompleted = true;
		for (int i = 0; i < allScenarios.Length; i++)
		{
			ref Scenario scenario = ref allScenarios[i];
			if(scenario != finalScenario)
			{
				isAllCompleted &= scenario.IsCompleted;
			}
		}

		if (isAllCompleted)
		{
			OnAllScenariosAreCompleted();
		}
	}

	private void OnAllScenariosAreCompleted()
	{
		SetReferencedObjectStates(true);
		finalScenario.enabled = true;
	}
}
