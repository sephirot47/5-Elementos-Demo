using System;
using UnityEngine;
using System.Collections.Generic;

class InfiniteComboStep : ControlledComboStep
{
    public InfiniteComboStep(String name, IComboInput inputDown, CustomAnimation anim) : base(name, inputDown, anim)
    {
    }

    public override void Update()
    {
        if (!started)
        {
            if (inputDown.Pressed())
            {
                started = true;
                animation.Play();
                parentCombo.OnStepStarted(this);
            }
        }
        else
        {
            time += Time.deltaTime;
            parentCombo.OnStepDoing(this, time);
            if (inputDown.Up() || (!inputDown.Pressed() && !inputDown.Down()) )
            {
                parentCombo.OnStepFinished(this);
            }
        }
    }
}
