using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimulatedComboStep : ComboStep
{
    public SimulatedComboStep(string name, CustomAnimation anim) : base(name, anim)
    {
    }

    public void Simulate()
    {
        Initialize();
        started = true;
        animation.Play();
        parentCombo.OnStepStarted(this);
    }

    public override void Update()
    {
        if (started)
        {
            time += Time.deltaTime;
            parentCombo.OnStepDoing(this, time);
            if(time > animation.GetDuration())
            {
                parentCombo.OnStepFinished(this);
            }
        }
    }
}
