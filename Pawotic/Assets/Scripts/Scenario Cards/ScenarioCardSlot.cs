using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioCardSlot : MonoBehaviour
{
    [SerializeField]
    private float slotSnapProximity;
    private float slotSnapProximityPwr;
    [SerializeField]
    private int slotOrder;
    
    private ScenarioCard selectedCard;
    private Scenario connectedScenario;

	public float SlotSnapProximityPwr { get => slotSnapProximityPwr; set => slotSnapProximityPwr = value; }
	public ScenarioCard SelectedCard { get => selectedCard; set => selectedCard = value; }
	public int SlotOrder { get => slotOrder; set => slotOrder = value; }

	private void Awake()
    {
        slotSnapProximityPwr = slotSnapProximity * slotSnapProximity;
    }

    public void Initialise(in Scenario scenario)
    {
        connectedScenario = scenario;
    }

    public void SetSelectedCard(in ScenarioCard card)
	{
        if(selectedCard)
		{
            selectedCard.OnDettachedFromSlot();
		}

        selectedCard = card;
        //Trigger Check
	}

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, slotSnapProximity);
    }
#endif
}
