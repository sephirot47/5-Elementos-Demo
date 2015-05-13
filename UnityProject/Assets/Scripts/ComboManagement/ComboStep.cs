using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComboStep 
{
	private IComboInput inputDown; //Boton que se ha de pulsar(DOWN) para que se haga el step (Solo es una tecla)
	private List<IComboInput> inputSimultaneous; //Botones que hay que mantener pulsados SIMULTANEAMENTE para que el DOWN se acepte
	private float timeRequired = 0.0f; //Tiempo que se ha de mantener el inputDown con sus inputSimultaneous para que se de por bueno el 

	//Cuanto menor sea este valor, mas tiempo tendra antes de que se acabe la animacion del step para pulsar la tecla
	//del siguiente, siendo 0 poder pulsarla desde el principio, y 1 poder pulsarla despues del final
	private readonly float animationDurationThresholdMultiplier = 0.9f; 
	private PlayerAnimation animation;

	private string name = "No name";
	private float timeDown = 0.0f; //Para contar el tiempo pasado
    private float timeSinceStarted = 0.0f;
	private bool startedPressing = false, cancelled = false, firstUpdate = true, lastStep = false, finished = false;
    private bool reallyDone = false;
	
	//Pulsacion instantanea de un input
	public ComboStep(string name, IComboInput inputDown, PlayerAnimation animation)
	{
		this.name = name;
		this.animation = animation;
		this.inputDown = inputDown;
		inputSimultaneous = new List<IComboInput>();
	}

	//Pulsacion de un input durante x tiempo
	public ComboStep(string name, IComboInput inputDown, float timeRequired, PlayerAnimation animation)
	{
		this.name = name;
		this.inputDown = inputDown;
		this.animation = animation;
		this.inputSimultaneous = new List<IComboInput>();
		this.timeRequired = timeRequired;
	}

	//Pulsacion instantanea de un input, con 1 o mas simultaneos pulsados
	public ComboStep(string name, IComboInput inputDown, IComboInput[] inputSimultaneous, PlayerAnimation animation)
	{
		this.name = name;
		this.inputDown = inputDown;
		this.animation = animation;
		this.inputSimultaneous = new List<IComboInput>();
		this.inputSimultaneous.AddRange(inputSimultaneous);
	}
	
	//Pulsacion de un input durante x tiempo, con simultaneos pulsados
	public ComboStep(string name, IComboInput inputDown, float timeRequired, IComboInput[] inputSimultaneous, PlayerAnimation animation)
	{
		this.name = name;
		this.inputDown = inputDown;
		this.animation = animation;
		this.inputSimultaneous = new List<IComboInput>();
		this.inputSimultaneous.AddRange(inputSimultaneous);
		this.timeRequired = timeRequired;
	}

	//Must be called by Combo Update, ONLY when its the currentstep!!! :)
	public void Update()
	{
		if(inputDown.Down()) startedPressing = true;
		else if(inputDown.Up() && startedPressing && timeDown < timeRequired)  Cancel();

        if (startedPressing)
        {
            timeDown += Time.deltaTime;
            timeSinceStarted += Time.deltaTime;
        }

		if(Done() && !finished)
		{
			finished = true;
			ComboManager.OnComboStepDone(this);
		}


        if (BeingDone()) ComboManager.OnComboStepDoing(this, timeDown);
        //if (!BeingDone() && timeDown < timeRequired) timeDown = 0.0f; //Para pulsaciones largas

        CheckIfDone();
	}

    private void CheckIfDone()
    {
        if (reallyDone || Cancelled()) return;

        if (timeRequired == 0.0f)
        {
            reallyDone = inputDown.Down() &&
                         AllSimultaneousPressed();
        }
        else
        {
            //bool inputDownOk = (lastStep ? inputDown.Pressed() : inputDown.Up()); 
            //HE PENSADO QUE ES MEJOR UP para todos. Dejo linea de arriba comentada por si acaso
            bool inputDownOk = inputDown.Up();

            reallyDone = inputDownOk &&
                         AllSimultaneousPressed() &&
                         timeDown >= timeRequired;

            /*
            Debug.Log(inputDownOk);
            Debug.Log(AllSimultaneousPressed());
            Debug.Log(timeDown >= timeRequired);
            Debug.Log("_________________");*/
        }
    }

	public bool Done()
	{
        return reallyDone && 
               timeSinceStarted >= GetNextStepInputInterval().first;
	}

	public void SetIsLast(bool last)
	{
		lastStep = last;
	}

	private bool EverythingPressed()
	{
		return inputDown.Pressed() && AllSimultaneousPressed();
	}

	//Dice las inputSimultaneous estan pulsadas ahora mismo
	private bool AllSimultaneousPressed()
	{
		foreach (IComboInput input in inputSimultaneous) 
		{
			if(!input.Pressed()) return false;
		}
		return true;
	}

	public void Cancel()
	{
		Reset();
		ComboManager.OnComboStepCancelled(this);
		cancelled = true; //el step se cancela si levantas despues de haberlo empezado(haberlo tenido pulsado 0.1 secs)
	}

	public bool Cancelled()
	{
		return cancelled;
	}

	//Indica si estan pressed las teclas que tocan. Util para que el Combo no corte el combo, porque imagina
	//que tienes un comboStep de pulsar A durante 999 segundos, pero el combo tiene un delay de 1 segundo. Ha de tener
	//alguna manera de comprobar esto para que no corte el combo porque no registra ningun done al cabo de un segundo
	public bool BeingDone()
	{
		return startedPressing && EverythingPressed();
	}
	
	public string GetName()
	{
		return name;
	}

	public void SetName(string name)
	{
		this.name = name;
	}

	//Ha de llamarlo Combo para resetearlo
	public void Reset()
	{
		timeDown = timeSinceStarted = 0.0f;
        reallyDone = false;
		startedPressing = cancelled = finished = false;
		firstUpdate = true;
	}

	public Pair<float, float> GetNextStepInputInterval() 
	{ 
		float animationDuration = PlayerAnimationManager.GetAnimationDuration(animation, Core.selectedPlayer);
		float min = animationDuration * animationDurationThresholdMultiplier;
		float max = animationDuration * 1.0f/animationDurationThresholdMultiplier;
		return new Pair<float, float>(min, max); 
	}
	public PlayerAnimation GetAnimation() { return animation; }
}
