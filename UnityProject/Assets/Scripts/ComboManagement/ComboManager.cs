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
			p.GetComponent<PlayerComboManager>().OnComboStarted(comboName);
		}
	}
	
	//Llamado cuando se ha acabado un combo entero
	public static void OnComboDone(string comboName)
	{
		foreach(Player p in Core.playerList)
		{
			p.GetComponent<PlayerComboManager>().OnComboDone(comboName);
		}
	}
	
	//SOLO llamado si el combo step es de mantener pulsado.
	//Si no, se llamara a OnComboStepDone
	public static void OnComboStepDoing(string stepName, float time)
	{
		foreach(Player p in Core.playerList)
		{
			p.GetComponent<PlayerComboManager>().OnComboStepDoing(stepName, time);
		}
	}
	
	public static void OnComboStepCancelled(string stepName)
	{
		foreach(Player p in Core.playerList)
		{
			p.GetComponent<PlayerComboManager>().OnComboStepCancelled(stepName);
		}
	}

	//Llamado cuando un step de un combo se ha acabado
	public static void OnComboStepDone(string stepName)
	{
		foreach(Player p in Core.playerList)
		{
			p.GetComponent<PlayerComboManager>().OnComboStepDone(stepName);
		}
	}

	//Dice si hay algun combo haciendose
	public static bool AnyComboBeingDone()
	{
		foreach(Combo c in combos)
		{
			if( c.BeingDone() ) return true;
		}
		return false;
	}

	private static void ResetCombo(string comboName)
	{
		foreach(Combo combo in combos)
		{
			if(combo.GetName() == comboName)
				combo.ResetCombo();
		}
	}

	private static void ResetAllCombos()
	{
		foreach(Combo combo in combos)
		{
			combo.ResetCombo();
		}
	}

	public static void CancelAllCombos()
	{
		foreach(Combo combo in combos)
		{
			combo.Cancel();
		}
	}

}
