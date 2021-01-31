using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioCardHolder : MonoBehaviour
{
	[SerializeField]
	private Button upButton;
	[SerializeField]
	private Button downButton;
	[SerializeField]
	private GameObject cardVisualPrefab;
	[SerializeField]
	private Transform contentParent;
	[SerializeField]
	private int stepSize;
	[SerializeField]
	private float cardSize;
	[SerializeField]
	private float topPadding;

	private List<int> selectedCardIndices;
	private PlayerInventory inventory;
	private int currentStartingIndex;
	private bool canGoUp;
	private bool canGoDown;

	public List<int> SelectedCardIndices { get => selectedCardIndices; set => selectedCardIndices = value; }

	private void Start()
	{
		inventory = FindObjectOfType<PlayerInventory>();
		selectedCardIndices = new List<int>();

		upButton.onClick.AddListener(() =>
		{
			SlideList(-4);
		});

		downButton.onClick.AddListener(() =>
		{
			SlideList(4);
		});

		UpdateView();
	}

	public void UpdateView()
	{
		for (int i = contentParent.childCount - 1; i > -1; --i)
		{
			Destroy(contentParent.GetChild(i).gameObject);
		}

		for (int i = 0; i < stepSize; i++)
		{
			int targetCardIndex = i + currentStartingIndex;
			if (inventory.AcquiredCards == null || targetCardIndex >= inventory.AcquiredCards.Count)
			{
				return;
			}
			if(selectedCardIndices.Contains(targetCardIndex))
			{
				continue;
			}

			GameObject child = Instantiate(cardVisualPrefab, contentParent);
			child.transform.localPosition = (-Vector3.one * topPadding) - (i * Vector3.up * cardSize);
			child.GetComponent<ScenarioCard>().Initialise(inventory.AcquiredCards[targetCardIndex], this, targetCardIndex);
		}
	}

	private void SlideList(int amount)
	{
		ShiftCurrentStartingIndexBy(amount);
		UpdateButtonStates();
		UpdateView();
	}

	private void UpdateButtonStates()
	{
		downButton.interactable = canGoDown;
		upButton.interactable = canGoUp;
	}

	private void ShiftCurrentStartingIndexBy(int amount)
	{
		currentStartingIndex = Mathf.Clamp(currentStartingIndex + amount, 0, inventory.AcquiredCards.Count - 1);
		canGoDown = (currentStartingIndex + stepSize) < inventory.AcquiredCards.Count; 
		canGoUp = (currentStartingIndex - stepSize) > -1; 
	}
}
