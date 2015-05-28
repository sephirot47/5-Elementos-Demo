using System;
using UnityEngine;
using System.Collections.Generic;

//Espera a que se pulse el UP para finalizarlo
class PersistentComboStep : ControlledComboStep
{
    private float timePressed = 0.0f;

    public PersistentComboStep(String name, IComboInput inputDown, float timePressed, CustomAnimation anim)
                        : base(name, inputDown, anim)
    {
        this.timePressed = timePressed;
    }

    public PersistentComboStep(String name, IComboInput inputDown, float timePressed, IComboInput[] inputSimultaneous, CustomAnimation anim)
                        : base(name, inputDown, inputSimultaneous, anim)
    {
        this.timePressed = timePressed;
    }

    public override void Update()
    {
        if (!started)
        {
            if (inputDown.Down() && AllSimultaneousPressed() && NoWrongControlKeysPressed())
            {
                started = true;
                animation.Play();
                parentCombo.OnStepStarted(this);
            }
        }
        else
        {
            if (AllSimultaneousPressed() && NoWrongControlKeysPressed())
            {
                if (time < animation.GetDuration() - ControlledComboStep.blend && inputDown.Pressed())
                {
                    parentCombo.OnStepDoing(this, timePressed);
                }

                if (inputDown.Pressed()) time += Time.deltaTime;
                else if (time < animation.GetDuration() - ControlledComboStep.blend) Cancel();

                if (time >= animation.GetDuration() - ControlledComboStep.blend && inputDown.Up())
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
