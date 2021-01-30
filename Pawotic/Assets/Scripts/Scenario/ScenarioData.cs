using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scenario Data", menuName = "Custom/Scenario/Create Scenario Data")]
public class ScenarioData : ScriptableObject
{
	[Multiline]
	public string scenarioText;
	public ScenarioCardData[] scenarioSequence;
}
