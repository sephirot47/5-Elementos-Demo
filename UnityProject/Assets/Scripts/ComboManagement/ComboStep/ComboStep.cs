using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ComboStep 
{
	protected IComboInput inputDown; //Boton que se ha de pulsar(DOWN) para que se haga el step (Solo es una tecla)
    protected List<IComboInput> inputSimultaneous; //Botones que hay que mantener pulsados SIMULTANEAMENTE
    protected Combo parentCombo;
    protected PlayerAnimation animation;
    protected float time;
    protected string name;
    protected bool started;

    public ComboStep(string name, PlayerAnimation anim)
    {
        this.name = name;
        animation = anim;

        inputDown = null;
        inputSimultaneous = new List<IComboInput>();
        parentCombo = null;
        time = 0.0f;
        started = false;
    }

    public abstract void Update();

    public void Initialize()
    {
        time = 0.0f;
        started = false;
    }

    public void Cancel()
    {
        Initialize();
        if (parentCombo != null) parentCombo.OnStepCancelled(this);
    }

    protected bool AllSimultaneousPressed()
    {
        foreach (IComboInput i in inputSimultaneous)
            if (!i.Down() || !i.Pressed()) return false;
        return true;
    }

    public bool Started()
    {
        return started;
    }

    public string GetName()
    {
        return name;
    }

    public void SetParentCombo(Combo c)
    {
        parentCombo = c;
    }
}
