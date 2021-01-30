using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerInventory : MonoBehaviour
{
	private List<ScenarioCardData> acquiredCards;

	public void AddScenarioCards(ScenarioCardData card)
	{
		acquiredCards.Add(card);
	}

	public void AddScenarioCards(ScenarioCardData[] cards)
	{
		acquiredCards.AddRange(cards);
	}
}
