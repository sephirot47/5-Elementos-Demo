using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ControlledComboStep : ComboStep
{
    public static readonly float blend = 0.3f; //segundos en los que puedes pulsar antes de que acabe la animacion
    private List<IComboInput> controlInputs = new List<IComboInput>();
	protected IComboInput inputDown; //Boton que se ha de pulsar(DOWN) para que se haga el step (Solo es una tecla)
    protected List<IComboInput> inputSimultaneous; //Botones que hay que mantener pulsados SIMULTANEAMENTE

    public ControlledComboStep(string name, IComboInput inputDown, CustomAnimation anim) : base(name, anim)
    {
        this.inputDown = inputDown;
        inputSimultaneous = new List<IComboInput>();

        controlInputs.Add(new ComboInputKey(KeyCode.LeftControl));
        controlInputs.Add(new ComboInputKey(KeyCode.RightControl));
        controlInputs.Add(new ComboInputKey(KeyCode.RightAlt));
        controlInputs.Add(new ComboInputKey(KeyCode.LeftAlt));
        controlInputs.Add(new ComboInputKey(KeyCode.LeftShift));
        controlInputs.Add(new ComboInputKey(KeyCode.RightShift));
    }

    public ControlledComboStep(string name, IComboInput inputDown, IComboInput[] inputSimultaneous, CustomAnimation anim) 
               :this(name, inputDown, anim)
    {
        this.inputSimultaneous.AddRange(inputSimultaneous);
    }


    protected bool AllSimultaneousPressed()
    {
        foreach (IComboInput i in inputSimultaneous)
            if (!i.Down() && !i.Pressed()) return false;
        return true;
    }

    //Indica si no se esta pulsando ninguna tecla de control que no le toca al combo,
    //Asi es como distinguimos entre comboSteps con la misma inputDown pero diferente tecla simultanea,
    //Para saber a cual darle "prioridad" :)
    protected bool NoWrongControlKeysPressed()
    {
        foreach (IComboInput i in controlInputs)
        {
            if (i.Down() || i.Pressed())
            {
                bool itsASimultaneousInput = false;
                foreach (IComboInput iSim in inputSimultaneous)
                {
                    if (i.GetId() == iSim.GetId())
                    {
                        itsASimultaneousInput = true;
                        break;
                    }
                }
                if (!itsASimultaneousInput) return false;
            }
        }
        return true;
    }
}
