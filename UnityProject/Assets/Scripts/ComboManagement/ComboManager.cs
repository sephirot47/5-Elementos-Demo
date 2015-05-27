using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComboManager 
{
	private List<Combo> combos = new List<Combo>();
    private float time = float.PositiveInfinity, 
                  comboDelay = 0.3f; //Delay entre combo y combo 
    private List<IComboListener> listeners;

    public ComboManager()
    {
        listeners = new List<IComboListener>();
    }
	
	public void Update()
	{
        time += Time.deltaTime;
        if (time > comboDelay)
        {
            foreach (Combo combo in combos) combo.Update();
        }
	}

	public void AddCombo(Combo combo)
	{
		combos.Add(combo);
	}

	//Llamado por PlayerSwitchManager
	public void OnPlayerSelectedChange()
	{
		CancelAllCombos();
	}

	//Llamado cuando se ha empezado un combo
	public void OnComboStarted(Combo combo)
    {
        foreach (IComboListener listener in listeners)
            listener.OnComboStarted(combo);
	}

    //Llamado cuando se ha cancelado un combo entero
    public void OnComboCancelled(Combo combo)
    {
        foreach (IComboListener listener in listeners)
            listener.OnComboCancelled(combo);
        time = 0.0f;
    }
	
	//Llamado cuando se ha acabado un combo entero
	public void OnComboFinished(Combo combo)
    {
        foreach (IComboListener listener in listeners)
            listener.OnComboFinished(combo);
        time = 0.0f;
        CancelAllCombos();
	}
	
	//SOLO llamado si el combo step es de mantener pulsado.
	//Si no, se llamara a OnComboStepDone
    public void OnComboStepDoing(ComboStep step, float time)
    {
        foreach (IComboListener listener in listeners)
            listener.OnComboStepDoing(step, time);
	}

    public void OnComboStepCancelled(ComboStep step)
    {
        foreach (IComboListener listener in listeners)
            listener.OnComboStepCancelled(step);
	}

    //Llamado cuando un step de un combo se ha acabado
    public void OnComboStepStarted(ComboStep step)
    {
        foreach (IComboListener listener in listeners)
            listener.OnComboStepStarted(step);
    }

    public void OnComboStepFinished(ComboStep step)
    {
        foreach (IComboListener listener in listeners)
            listener.OnComboStepFinished(step);
	}

	//Dice si hay algun combo haciendose
	public bool AnyComboBeingDone()
	{
		foreach(Combo c in combos)
            if( c.BeingDone() ) return true;
		return false;
	}

	private void CancelCombo(string comboName)
	{
		foreach(Combo combo in combos)
			if(combo.GetName() == comboName)
				combo.Cancel();
	}

	public void CancelAllCombos()
	{
		foreach(Combo combo in combos)
            combo.Cancel();
	}

    public void OnComboStepFinished(ComboStep comboStep, float timePressed)
    {
        foreach(IComboListener listener in listeners)
            listener.OnComboStepDoing(comboStep, timePressed);
    }

    internal void AddListener(IComboListener listener)
    {
        listeners.Add(listener);
    }
}
