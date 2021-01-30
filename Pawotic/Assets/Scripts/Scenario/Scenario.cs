using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenario : MonoBehaviour
{
    [SerializeField]
    private int scenarioOrder;
    [SerializeField]
    private ScenarioData scenario;
    [SerializeField]
    private ScenarioCardSlot[] slots;
	private PlayerInventory inventory;

	private void Awake()
	{
		if(slots == null)
		{
            return;
		}

		for (int i = 0; i < slots.Length; i++)
		{
			ref ScenarioCardSlot slot = ref slots[i];
			slot.Initialise(this);
		}

		inventory = FindObjectOfType<PlayerInventory>();
	}

	public void CheckScenarioCardCombination()
	{
		if (scenario == null || scenario.scenarioSequence == null)
		{
			return;
		}

		bool isCompleted = true;
		bool isAllDecisionsMade = true;
		for (int i = 0; i < slots.Length; i++)
		{
			ref ScenarioCardSlot slot = ref slots[i];
			
			if(slot.SlotOrder < 0 || slot.SlotOrder >= scenario.scenarioSequence.Length)
			{
				return;
			}

			isAllDecisionsMade &= (slot.SelectedCard != null);
			isCompleted &= (slot.SelectedCard == scenario.scenarioSequence[slot.SlotOrder]);
		}

		if(isCompleted)
		{
			OnScenarioIsCompleted();
		}
	}

	private void OnScenarioIsCompleted() 
	{
		if (!inventory) 
		{
			return;
		}

		inventory.AddScenarioCards(scenario.cardRewards);
	}
}
