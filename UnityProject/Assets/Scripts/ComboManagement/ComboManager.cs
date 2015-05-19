using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComboManager : MonoBehaviour 
{
	private static List<Combo> combos = new List<Combo>();
    private static float time = float.PositiveInfinity, 
                         comboDelay = 0.0f; //Delay entre combo y combo 

	void Start () 
	{
	}
	
	void Update()
	{
        time += Time.deltaTime;
        if (time > comboDelay)
        {
            foreach (Combo combo in combos) combo.Update();
        }
	}

	public static void AddCombo(Combo combo)
	{
		combos.Add(combo);
	}

	//Llamado por PlayerSwitchManager
	public static void OnPlayerSelectedChange()
	{
		CancelAllCombos();
	}

	//Llamado cuando se ha empezado un combo
	public static void OnComboStarted(Combo combo)
	{
		foreach(Player p in Core.playerList)
            p.GetComponent<PlayerComboManager>().OnComboStarted(combo);
	}

    //Llamado cuando se ha cancelado un combo entero
    public static void OnComboCancelled(Combo combo)
    {
        foreach (Player p in Core.playerList)
            p.GetComponent<PlayerComboManager>().OnComboCancelled(combo);

        time = 0.0f;
    }
	
	//Llamado cuando se ha acabado un combo entero
	public static void OnComboFinished(Combo combo)
	{
		foreach(Player p in Core.playerList)
            p.GetComponent<PlayerComboManager>().OnComboFinished(combo);

        time = 0.0f;
	}
	
	//SOLO llamado si el combo step es de mantener pulsado.
	//Si no, se llamara a OnComboStepDone
	public static void OnComboStepDoing(ComboStep step, float time)
	{
		foreach(Player p in Core.playerList)
            p.GetComponent<PlayerComboManager>().OnComboStepDoing(step, time);
	}
	
	public static void OnComboStepCancelled(ComboStep step)
	{
		foreach(Player p in Core.playerList)
            p.GetComponent<PlayerComboManager>().OnComboStepCancelled(step);
	}

    //Llamado cuando un step de un combo se ha acabado
    public static void OnComboStepStarted(ComboStep step)
    {
        foreach (Player p in Core.playerList)
            p.GetComponent<PlayerComboManager>().OnComboStepStarted(step);
    }

	public static void OnComboStepFinished(ComboStep step)
	{
		foreach(Player p in Core.playerList)
            p.GetComponent<PlayerComboManager>().OnComboStepFinished(step);
	}

	//Dice si hay algun combo haciendose
	public static bool AnyComboBeingDone()
	{
		foreach(Combo c in combos)
            if( c.BeingDone() ) return true;
		return false;
	}

	private static void CancelCombo(string comboName)
	{
		foreach(Combo combo in combos)
			if(combo.GetName() == comboName)
				combo.Cancel();
	}

	public static void CancelAllCombos()
	{
		foreach(Combo combo in combos)
            combo.Cancel();
	}

    public static void OnComboStepFinished(ComboStep comboStep, float timePressed)
    {
        foreach (Player p in Core.playerList)
            p.GetComponent<PlayerComboManager>().OnComboStepDoing(comboStep, timePressed);
    }
}
