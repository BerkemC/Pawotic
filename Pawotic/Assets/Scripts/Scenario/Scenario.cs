using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenario : MonoBehaviour
{
    [SerializeField]
    private int scenarioOrder;
    [SerializeField]
    private ScenarioData scenario;
    private ScenarioCardSlot[] slots;
	[SerializeField]
	private GameObject slotPrefab;
	[SerializeField]
	private Transform contentParent;
	[SerializeField]
	private float slotSize;
	private PlayerInventory inventory;
	

	private void Awake()
	{
		inventory = FindObjectOfType<PlayerInventory>();
		Initialise();
	}

	public void Initialise()
	{
		ResetContent();

		slots = new ScenarioCardSlot[scenario.scenarioSequence.Length];
		for (int i = 0; i < scenario.scenarioSequence.Length; i++)
		{
			GameObject child = Instantiate(slotPrefab, contentParent);
			child.transform.localPosition = (i * Vector3.right * slotSize);
			ScenarioCardSlot slot = child.GetComponent<ScenarioCardSlot>();
			slot.Initialise(this, i);
			slots[i] = slot;
		}
	}

	public void ResetContent()
	{
		for (int i = contentParent.childCount - 1; i > -1; --i)
		{
			Destroy(contentParent.GetChild(i).gameObject);
		}
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
			if(!isAllDecisionsMade)
			{
				return;
			}
			isCompleted &= (slot.SelectedCard.Data == scenario.scenarioSequence[slot.SlotOrder]);
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
		for (int i = 0; i < slots.Length; i++)
		{
			ref ScenarioCardSlot slot = ref slots[i];
			slot.SelectedCard.OnDettachedFromSlot();
		}
		inventory.AddScenarioCards(scenario.cardRewards);
		ResetContent();
	}
}
