using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComboStep 
{
	private IComboInput inputDown; //Boton que se ha de pulsar(DOWN) para que se haga el step (Solo es una tecla)
	private List<IComboInput> inputSimultaneous; //Botones que hay que mantener pulsados SIMULTANEAMENTE para que el DOWN se acepte
	private float timeRequired = 0.0f; //Tiempo que se ha de mantener el inputDown con sus inputSimultaneous para que se de por bueno el step
	
	private string id = "";
	private float timeDown = 0.0f; //Para contar el tiempo pasado
	private bool startedPressing = false, cancelled = false, firstUpdate = true, lastStep = false;


	//Pulsacion instantanea de un input
	public ComboStep(IComboInput inputDown)
	{
		this.inputDown = inputDown;
		inputSimultaneous = new List<IComboInput>();
	}

	//Pulsacion de un input durante x tiempo
	public ComboStep(IComboInput inputDown, float timeRequired)
	{
		this.inputDown = inputDown;
		this.inputSimultaneous = new List<IComboInput>();
		this.timeRequired = timeRequired;
	}

	//Pulsacion instantanea de un input, con 1 o mas simultaneos pulsados
	public ComboStep(IComboInput inputDown, IComboInput[] inputSimultaneous)
	{
		this.inputDown = inputDown;
		this.inputSimultaneous = new List<IComboInput>();
		this.inputSimultaneous.AddRange(inputSimultaneous);
	}
	
	//Pulsacion de un input durante x tiempo, con simultaneos pulsados
	public ComboStep(IComboInput inputDown, float timeRequired, IComboInput[] inputSimultaneous)
	{
		this.inputDown = inputDown;
		this.inputSimultaneous = new List<IComboInput>();
		this.inputSimultaneous.AddRange(inputSimultaneous);
		this.timeRequired = timeRequired;
	}

	//Must be called by Combo Update, ONLY when its the currentstep!!! :)
	public void Update()
	{
		timeDown += Time.deltaTime;

		if(inputDown.Down()) startedPressing = true;
		else if(inputDown.Up() && startedPressing && timeDown < timeRequired) {
			Debug.Log("Step cancelled");
			cancelled = true; //el step se cancela si levantas despues de haberlo empezado(haberlo tenido pulsado 0.1 secs)
		}

		if( !BeingDone() && timeDown < timeRequired) timeDown = 0.0f;
	}

	public bool Done()
	{
		bool done = false;

		if(timeRequired == 0.0f)
		{
			done = inputDown.Down() &&
			   	   AllSimultaneousPressed();
		}
		else
		{
			//Para evitar quedarse pulsando la misma tecla y hacerte el combo dejandola pulsada
			//En el ultimo step, si te pasas del tiempo acaba el combo igualmente
			//En los de antes, has de soltar antes de hacer el siguiente step
			bool inputDownOk = (lastStep ? inputDown.Pressed() : inputDown.Up()); 

			done = inputDownOk &&
				   AllSimultaneousPressed() &&
			       timeDown >= timeRequired;

			/*
			Debug.Log(inputDownOk);
			Debug.Log(AllSimultaneousPressed());
			Debug.Log(timeDown >= timeRequired);
			Debug.Log("_________________");*/
		}

		if(done) Debug.Log (inputDown.GetId() + " done");

		return done && !Cancelled();
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

	public bool Cancelled()
	{
		return cancelled;
	}

	//Indica si estan pressed las teclas que tocan. Util para que el Combo no corte el combo, porque imagina
	//que tienes un comboStep de pulsar A durante 999 segundos, pero el combo tiene un delay de 1 segundo. Ha de tener
	//alguna manera de comprobar esto para que no corte el combo porque no registra ningun done al cabo de un segundo
	public bool BeingDone()
	{
		return EverythingPressed();
	}

	public void SetId(string id)
	{
		this.id = id;
	}

	public string GetId() { return id; }
	//Ha de llamarlo Combo para resetearlo
	public void Reset()
	{
		timeDown = 0.0f;
		startedPressing = cancelled = false;
		firstUpdate = true;
	}
}
