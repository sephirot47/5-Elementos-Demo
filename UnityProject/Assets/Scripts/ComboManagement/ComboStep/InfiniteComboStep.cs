using System;
using UnityEngine;
using System.Collections.Generic;

class InfiniteComboStep : ComboStep
{
    private float timePressed = 0.0f;

    public InfiniteComboStep(String name, IComboInput inputDown, PlayerAnimation anim) : base(name, inputDown, anim)
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
            if (inputDown.Up())
            {
                parentCombo.OnStepFinished(this);
            }
        }
    }

}
