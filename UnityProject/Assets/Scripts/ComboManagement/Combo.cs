using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Combo
{
    private PlayerMovement playerMov;
    private bool aerialCombo = false;

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
        if (aerialCombo && playerMov.IsGrounded()) { Cancel(); return; }

		if(Done()) 
		{
			ComboManager.OnComboDone(this);
			ResetCombo();
		}

		time += Time.deltaTime;

		steps[currentStep].Update();

        if (steps[currentStep].BeingDone())
        {
            time = 0.0f;
            if (!started)
            {
                started = true;
                ComboManager.OnComboStarted(this);
            }
        }

        if(steps[currentStep].Cancelled()) ResetCombo();
        else
        {
            if (steps[currentStep].Done())  NextStep();
            else if ( AfterCorrectTime() ) Cancel();
		}
	}

	public void NextStep()
	{
		++currentStep; 
		time = 0.0f;
	}

    public bool BeforeCorrectTime()
    {
        if (currentStep - 1 < 0) return false;
        return time < steps[currentStep-1].GetNextStepInputInterval().first;
    }

    public bool AfterCorrectTime()
    {
        if (currentStep - 1 < 0) return false;
        return time > steps[currentStep-1].GetInputTimeThreshold();
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

		foreach(ComboStep step in steps) step.Reset();
	}
    
	public void AppendStep(ComboStep step)
	{
		steps.Add(step);

		int i;
		for(i = 0; i <  steps.Count - 1; ++i) steps[i].SetIsLast(false);
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
		steps[currentStep].Cancel();
		ResetCombo();
        ComboManager.OnComboCancel(this);
	}

    public void SetPlayerMovement(PlayerMovement mov)
    {
        playerMov = mov;
    }

    public void SetIsAerial(bool aerial)
    {
        aerialCombo = aerial;
    }
}
