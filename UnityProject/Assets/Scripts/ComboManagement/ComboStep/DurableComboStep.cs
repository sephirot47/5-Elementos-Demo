using System;
using UnityEngine;
using System.Collections.Generic;

//En cuanto reproduce la animacion se finaliza, manteniendo la inputDown todo el rato,
//Si en algun momento antes de que acabe la animacion se suelta, el combo se cancela
class DurableComboStep : ControlledComboStep
{
    public DurableComboStep(String name, IComboInput inputDown, CustomAnimation anim)
        : base(name, inputDown, anim)
    {
    }

    public DurableComboStep(String name, IComboInput inputDown, IComboInput[] inputSimultaneous, CustomAnimation anim)
        : base(name, inputDown, inputSimultaneous, anim)
    {
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
            if (AllSimultaneousPressed())
            {
                time += Time.deltaTime;

                if (time >= animation.GetDuration() - ControlledComboStep.blend)
                {
                    parentCombo.OnStepFinished(this);
                }
                else
                {
                    if (inputDown.Down() || inputDown.Pressed()) parentCombo.OnStepDoing(this, time);
                    else Cancel();
                }
            }
            else Cancel();
        }
    }

}
