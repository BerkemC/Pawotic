using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class ScenarioCard : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private ScenarioCardData data;
	private Canvas canvas;
	private Vector3 offsetToMouse;
	private bool isBeingDragged = false;
	private RectTransform initialParent;
	private Vector3 initialWorldPos;
	private ScenarioCardHolder holder;
	private int holderIndex;

	[SerializeField]
    private Text descriptionField;

	[SerializeField]
	private float goBackTime;
	private float currentGoBackTime;

	public ScenarioCardData Data { get => data; set => data = value; }
	private AudioSource audioSource;

	private void Awake()
	{
		canvas = FindObjectOfType<Canvas>();
		initialParent = transform.parent as RectTransform;
		audioSource = GetComponent<AudioSource>();
	}

	private void Start()
	{
		initialWorldPos = transform.position;
	}

	public void Initialise(in ScenarioCardData scenarioCardData, in ScenarioCardHolder cardHolder, int indexInHolder)
	{
		holderIndex = indexInHolder;
		holder = cardHolder;
		data = scenarioCardData;
		InitialiseUI();
	}

	private void InitialiseUI()
	{
		descriptionField.text = data.cardDescription;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
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
		holder.SelectedCardIndices.Add(holderIndex);
		currentGoBackTime = 0f;
	}

	public void OnDettachedFromSlot()
	{
		transform.parent = initialParent;
		StartCoroutine(MoveTowardsHolder());
		holder.SelectedCardIndices.Remove(holderIndex);
		audioSource.Play();
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

	public IEnumerator MoveTowardsHolder()
	{
		Vector3 startPos = transform.position;
		while(currentGoBackTime < goBackTime)
		{
			transform.position = Vector3.Lerp(startPos, initialWorldPos, currentGoBackTime / goBackTime);
			currentGoBackTime += Time.deltaTime;
			yield return new WaitForSeconds(Time.deltaTime);
		}
		
		transform.position = initialWorldPos;
		yield return null;
	}
}
