using System;
using UnityEngine;
using System.Collections.Generic;

class PersistentComboStep : ComboStep
{
    private float timePressed = 0.0f;

    public PersistentComboStep(String name, IComboInput inputDown, float timePressed, PlayerAnimation anim)
                        : base(name, inputDown, anim)
    {
        this.timePressed = timePressed;
    }

    public PersistentComboStep(String name, IComboInput inputDown, float timePressed, IComboInput[] inputSimultaneous, PlayerAnimation anim)
                        : base(name, inputDown, inputSimultaneous, anim)
    {
        this.timePressed = timePressed;
    }

    public override void Update()
    {
        if (!started)
        {
            if (inputDown.Pressed() && AllSimultaneousPressed())
            {
                started = true;
                animation.Play();
                parentCombo.OnStepStarted(this);
            }
        }
        else
        {
            if (AllSimultaneousPressed())
            {
                if (inputDown.Pressed()) time += Time.deltaTime;
                else if (time < animation.GetDuration() - ComboStep.blend) Cancel();

                if (time >= animation.GetDuration() - ComboStep.blend && inputDown.Up())
                {
                    parentCombo.OnStepFinished(this);
                }
            }
            else
            {
                if (time >= animation.GetDuration()) parentCombo.OnStepFinished(this);
                else Cancel();
            }
        }
    }

}
