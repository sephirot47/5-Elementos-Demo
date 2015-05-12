using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComboManager : MonoBehaviour 
{
	private static List<Combo> combos = new List<Combo>();

	void Start () 
	{
	}
	
	void Update()
	{
		foreach(Combo combo in combos)
		{
			combo.Update();
		}
	}

	public static void AddCombo(Combo combo)
	{
		combos.Add(combo);
	}

	//Llamado por PlayerSwitchManager
	public static void OnPlayerSelectedChange()
	{
		ResetAllCombos();
	}

	//Llamado cuando se ha empezado un combo
	public static void OnComboStarted(string comboName)
	{
		foreach(Player p in Core.playerList)
		{
			p.GetComponent<PlayerComboHandler>().OnComboStarted(comboName);
		}
	}
	
	//Llamado cuando se ha acabado un combo entero
	public static void OnComboDone(string comboName)
	{
		foreach(Player p in Core.playerList)
		{
			p.GetComponent<PlayerComboHandler>().OnComboDone(comboName);
		}
	}
	
	//SOLO llamado si el combo step es de mantener pulsado.
	//Si no, se llamara a OnComboStepDone
	public static void OnComboStepStarted(string stepName)
	{
		foreach(Player p in Core.playerList)
		{
			p.GetComponent<PlayerComboHandler>().OnComboStepStarted(stepName);
		}
	}
	
	//Llamado cuando un step de un combo se ha acabado
	public static void OnComboStepDone(string stepName)
	{
		foreach(Player p in Core.playerList)
		{
			p.GetComponent<PlayerComboHandler>().OnComboStepDone(stepName);
		}
	}
	
	private static void ResetAllCombos()
	{
		foreach(Combo combo in combos)
		{
			combo.ResetCombo();
		}
	}
}
