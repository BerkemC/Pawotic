using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerInventory : MonoBehaviour
{
	private List<ScenarioCardData> acquiredCards;
	private ScenarioCardHolder holder;
	public List<ScenarioCardData> AcquiredCards { get => acquiredCards; set => acquiredCards = value; }

	private void Awake()
	{
		holder = FindObjectOfType<ScenarioCardHolder>();
		acquiredCards = new List<ScenarioCardData>();

		ScenarioCardData[] cards = Resources.LoadAll<ScenarioCardData>("Scenario Cards/");
		for (int i = 0; i < cards.Length; i++)
		{
			ref ScenarioCardData card = ref cards[i];
			if(card.addedOnGameStart && !acquiredCards.Contains(card))
			{
				acquiredCards.Add(card);
			}
		}
		ShuffleInventory();
	}

	public void AddScenarioCards(in ScenarioCardData card)
	{
		acquiredCards.Add(card);
		ShuffleInventory();
		holder.UpdateView();
	}

	public void AddScenarioCards(in ScenarioCardData[] cards)
	{
		acquiredCards.AddRange(cards);
		ShuffleInventory();
		holder.UpdateView();
	}

	private void ShuffleInventory()
	{
		for (int i = 0; i < acquiredCards.Count; i++)
		{
			SwapItems(i, Random.Range(0, acquiredCards.Count));
		}
	}

	private void SwapItems(int index0, int index1)
	{
		ScenarioCardData temp = acquiredCards[index0];
		acquiredCards[index0] = acquiredCards[index1];
		acquiredCards[index1] = temp;
	}
}
