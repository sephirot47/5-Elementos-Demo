using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Combo
{
	private List<ComboStep> steps;
    private readonly float delay = ComboStep.blend * 2.0f;
	private float timeDelay = 0.0f; //Para contar el tiempo pasado
	private string name = "No name";
	private int currentStep = 0;
    private bool started = false, enabled = true;
    private ComboManager comboManager;

	public Combo(string name, ComboManager cb)
	{
		this.name = name;
        steps = new List<ComboStep>();
        comboManager = cb;
	}

    public Combo(string name, ComboStep[] comboSteps, ComboManager cb)
        : this(name, cb)
	{
		steps.AddRange(comboSteps);
	}

	//Must be called by the ComboManager :)
	public void Update()
    {
        if (!enabled) return;

        if (currentStep < steps.Count)
        {
            if(!steps[currentStep].Started() && currentStep > 0) //Si estamos entre step y step
            {
                timeDelay += Time.deltaTime;
                if(timeDelay > delay)
                {
                    Cancel(); //Si excede el tiempo de delay entre step y step, cancelamos
                    return;
                }
            }

            steps[currentStep].Update();
        }
        else timeDelay = 0.0f;
    }

	public void NextStep()
	{
		++currentStep; 
		timeDelay = 0.0f;
	}
	
	public bool BeingDone()
	{
		return started;
	}

	public void AppendStep(ComboStep step)
	{
		steps.Add(step);
        step.SetParentCombo(this);
	}

	public bool Finished()
	{
		return currentStep >= steps.Count;
	}

	public string GetName()
	{
		return name;
	}

    public void Initialize()
    {
        currentStep = 0;
        timeDelay = 0.0f;
        started = false;
        foreach (ComboStep step in steps) step.Initialize();
    }

	public void Cancel()
    {
        Initialize();
        comboManager.OnComboCancelled(this);
	}

    public void OnStepStarted(ComboStep step)
    {
        if (currentStep == 0)
        {
            started = true;
            comboManager.OnComboStarted(this);
        }
        comboManager.OnComboStepStarted(step);
    }

    public void OnStepCancelled(ComboStep step)
    {
        Cancel();
        comboManager.OnComboStepCancelled(step);
        comboManager.OnComboCancelled(this);
    }

    public void OnStepDoing(ComboStep comboStep, float timePressed)
    {
        comboManager.OnComboStepFinished(comboStep, timePressed);
    }

    public void OnStepFinished(ComboStep step)
    {
        ++currentStep;
        comboManager.OnComboStepFinished(step);
        if(currentStep < steps.Count) 
        {
            steps[currentStep].Initialize();
        }
        else //Combo finished
        {
            Initialize();
            comboManager.OnComboFinished(this);
        }
    }

    public void SetEnabled(bool enabled)
    {
        this.enabled = enabled;
    }

    public List<ComboStep> GetSteps()
    {
        return steps;
    }

}
