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
			if(card.addedOnGameStart)
			{
				acquiredCards.Add(card);
			}
		}
	}

	public void AddScenarioCards(in ScenarioCardData card)
	{
		acquiredCards.Add(card);
		holder.UpdateView();
	}

	public void AddScenarioCards(in ScenarioCardData[] cards)
	{
		acquiredCards.AddRange(cards);
		holder.UpdateView();
	}
}
