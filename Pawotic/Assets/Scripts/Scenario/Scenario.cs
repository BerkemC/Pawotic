using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
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
	private PlayerController controller;
	private bool isActivated;
	private bool isCompleted;
	private ScenarioCancelButton cancelButton;
	private AudioSource audioSource;
	[SerializeField]
	private AudioClip openSound;
	[SerializeField]
	private AudioClip successSound;
	[SerializeField]
	private AudioClip failureSound;
	private FinalScenario finalScenario;
	private ScenarioCardHolder holder;

	public bool IsCompleted { get => isCompleted; set => isCompleted = value; }

	private void Awake()
	{
		inventory = FindObjectOfType<PlayerInventory>();
		controller = inventory.GetComponent<PlayerController>();
		finalScenario = FindObjectOfType<FinalScenario>(true);
		audioSource = GetComponent<AudioSource>();
		holder = FindObjectOfType<ScenarioCardHolder>();
	}

	public void Initialise()
	{
		if(isActivated)
		{
			return;
		}
		
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
		isActivated = true;
		controller.SetControlState(false);
		
		cancelButton = FindObjectOfType<ScenarioCancelButton>(true);
		cancelButton.gameObject.SetActive(true);

		cancelButton.GetComponent<Button>().onClick.AddListener(() =>
		{
			ResetContent(true);
			cancelButton.gameObject.SetActive(false);
		});

		audioSource.clip = openSound;
		audioSource.Play();
		holder.SelectedCardIndices.Clear();
		holder.UpdateView();
	}

	public void ResetContent(bool dettachContent = false)
	{
		if(dettachContent)
		{
			for (int i = 0; i < slots.Length; i++)
			{
				ref ScenarioCardSlot slot = ref slots[i];
				if(slot.SelectedCard)
				{
					slot.SelectedCard.OnDettachedFromSlot();
				}
			}
			holder.SelectedCardIndices.Clear();
			holder.UpdateView();
		}

		for (int i = contentParent.childCount - 1; i > -1; --i)
		{
			Destroy(contentParent.GetChild(i).gameObject);
		}

		if(isActivated && !isCompleted)
		{
			isActivated = false;
		}
		controller.SetControlState(true);
	}

	public void CheckScenarioCardCombination()
	{
		if (scenario == null || scenario.scenarioSequence == null)
		{
			return;
		}

		isCompleted = true;
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
				isCompleted = false;
				return;
			}
			isCompleted &= (slot.SelectedCard.Data == scenario.scenarioSequence[slot.SlotOrder]);
		}

		if(isCompleted)
		{
			OnScenarioIsCompleted();
		}
		else
		{
			audioSource.clip = failureSound;
			audioSource.Play();
		}
	}

	private void OnScenarioIsCompleted() 
	{
		if (!inventory) 
		{
			return;
		}

		if(scenario.cardRewards != null)
		{
			inventory.AddScenarioCards(scenario.cardRewards);
		}
		ResetContent(true);
		MessageBoxController.Instance.DisplayMessage(scenario.scenarioText);
		MessageBoxController.Instance.DisplayMessage("Congratulations! You are one step closer to find Poco! You have received more scenario cards to solve the mystery!");
		audioSource.clip = successSound;
		audioSource.Play();
		finalScenario.CheckAllScenarioCompletions();
		cancelButton.gameObject.SetActive(false);
	}
}
