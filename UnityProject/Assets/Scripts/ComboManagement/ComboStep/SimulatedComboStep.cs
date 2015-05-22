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
        parentCombo.OnStepStarted(this);
    }

    public override void Update()
    {
        if (started)
        {
            time += Time.deltaTime;
            if(time > animation.GetDuration())
            {
                parentCombo.OnStepFinished(this);
            }
        }
    }
}
