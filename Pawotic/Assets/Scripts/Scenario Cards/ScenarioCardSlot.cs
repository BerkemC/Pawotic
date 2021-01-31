using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ScenarioCardSlot : MonoBehaviour
{
    [SerializeField]
    private float slotSnapProximity;
    private float slotSnapProximityPwr;
    private int slotOrder;
    
    private ScenarioCard selectedCard;
    private Scenario connectedScenario;

    private AudioSource audioSource;

	public float SlotSnapProximityPwr { get => slotSnapProximityPwr; set => slotSnapProximityPwr = value; }
	public ScenarioCard SelectedCard { get => selectedCard; set => selectedCard = value; }
	public int SlotOrder { get => slotOrder; set => slotOrder = value; }

	private void Awake()
    {
        slotSnapProximityPwr = slotSnapProximity * slotSnapProximity;
        audioSource = GetComponent<AudioSource>();
    }

    public void Initialise(in Scenario scenario, int order)
    {
        connectedScenario = scenario;
        slotOrder = order;
    }

    public void SetSelectedCard(in ScenarioCard card)
	{
        if(selectedCard && selectedCard != card)
		{
            selectedCard.OnDettachedFromSlot();
		}

        selectedCard = card;
        connectedScenario.CheckScenarioCardCombination();
        audioSource.Play();
	}

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, slotSnapProximity);
    }
#endif
}
