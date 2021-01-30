using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScenarioCard : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private ScenarioCardData data;
	private Canvas canvas;
	private Vector3 offsetToMouse;
	private bool isBeingDragged = false;
	private RectTransform initialParent;
	private Vector3 initialWorldPos;

	[SerializeField]
    private Text descriptionField;
	private void Awake()
	{
		canvas = FindObjectOfType<Canvas>();
		initialParent = transform.parent as RectTransform;
		descriptionField.text = Random.Range(0, 172492).ToString();
	}

	public void Initialise(in ScenarioCardData scenarioCardData)
	{
		data = scenarioCardData;
		InitialiseUI();
	}

	private void InitialiseUI()
	{
		descriptionField.text = data.cardDescription;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		initialWorldPos = transform.position;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		ScenarioCardSlot[] slots = GameObject.FindObjectsOfType<ScenarioCardSlot>();

		for (int i = 0; i < slots.Length; i++)
		{
			ref var slot = ref slots[i];
			if ((slot.transform.position - transform.position).sqrMagnitude <= slot.SlotSnapProximityPwr)
			{
				OnAttachedToSlot(slot);
				return;
			}
		} 

		if (transform.parent != initialParent)
		{
			OnDettachedFromSlot();
			return;
		}

		transform.position = initialWorldPos;
	}

	private void OnAttachedToSlot(in ScenarioCardSlot slot)
	{
		slot.SetSelectedCard(this);
		transform.parent = canvas.transform;
		transform.position = slot.transform.position;
	}

	public void OnDettachedFromSlot()
	{
		transform.parent = initialParent;
	}

	public void OnDrag(PointerEventData eventData)
	{
		RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
																Input.mousePosition,
																canvas.worldCamera,
																out Vector2 pos);
		pos = canvas.transform.TransformPoint(pos);
		transform.position = pos;
	}
}
