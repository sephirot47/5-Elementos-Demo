using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Combo
{
	[SerializeField]
	private List<ComboStep> steps;
	private float time = 0.0f; //Para contar el tiempo pasado
	private string name = "No name";
	private int currentStep = 0;
	private bool started = false;

	public Combo(string name)
	{
		this.name = name;
		steps = new List<ComboStep>();
	}

	public Combo(string name, ComboStep[] comboSteps)
	{
		this.name = name;
		steps = new List<ComboStep>();
		steps.AddRange(comboSteps);
	}

	//Must be called by the ComboManager :)
	public void Update()
	{
		if(Done()) 
		{
			//Combo done
			ComboManager.OnComboDone(this);
			ResetCombo();
		}

		time += Time.deltaTime;

		//Esto solo se comprueba si el combo no se esta haciendo
		if(!steps[currentStep].BeingDone())
		{
			if( AfterCorrectTime() ) ResetCombo(); //Se ha pasado, reseteamos combo
			else if( BeforeCorrectTime() ) return; //Esperamos
		}

		if(steps[currentStep].BeingDone() && !started)
		{
			time = 0.0f;
			started = true;
			ComboManager.OnComboStarted(this);
		}

		steps[currentStep].Update();

		//Para evitar que el comboo se corte cuando se tiene que pulsar una tecla
		//durante un tiempo mayor que el delay del combo :)
		if(steps[currentStep].BeingDone())
		{
			time = 0.0f;
			if(!started)
			{
				started = true;
				ComboManager.OnComboStarted(this);
			}
		}

        if(steps[currentStep].Cancelled())
        {
			ResetCombo();
		}
        else
        {
			//Solo un step por frame
			if(steps[currentStep].Done())
			{
				NextStep();
			}
		}
	}

	public void NextStep()
	{
		//Yay, step hecho, vamos al siguiente!!!
		++currentStep; 
		time = 0.0f; //El time se reinicia obviamente
	}
	
	public bool BeingDone()
	{
		return started;
	}

	public void ResetCombo()
	{
		currentStep = 0;
		time = 0.0f;
		started = false;

		foreach(ComboStep step in steps)
		{
			step.Reset();
		}
	}

	private bool BeforeCorrectTime()
	{
		if(currentStep - 1 < 0) return false;
		return time < steps[currentStep].GetNextStepInputInterval().first;
	}

	private bool AfterCorrectTime()
	{
		if(currentStep - 1 < 0) return false;
		return time > steps[currentStep].GetNextStepInputInterval().second;
	}

	public void AppendStep(ComboStep step)
	{
		steps.Add(step);

		int i;
		for(i = 0; i <  steps.Count - 1; ++i)
		{
			steps[i].SetIsLast(false);
		}
		steps[i].SetIsLast(true);
	}

	public bool Done()
	{
		return currentStep >= steps.Count;
	}

	public string GetName()
	{
		return name;
	}

	public void Cancel()
	{
		//En este orden
		steps[currentStep].Cancel();
		ResetCombo();
	}
}
