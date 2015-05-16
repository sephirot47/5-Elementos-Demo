using UnityEngine;
using System.Collections;

public class PlayerComboManager : MonoBehaviour 
{
	private bool comboing = false;

    private Player player;
    private PlayerMovement playerMov;
	private PlayerAnimationManager anim;

	void Start() 
	{
		player = GetComponent<Player>();
        playerMov = GetComponent<PlayerMovement>();
		anim = GetComponent<PlayerAnimationManager>();

		if(player == Core.kaji) //Combos de kaji
		{
			ComboInputKey jump = new ComboInputKey(KeyCode.Space);
			ComboInputKey guard = new ComboInputKey(KeyCode.LeftShift);
			ComboInputKey attack = new ComboInputKey(KeyCode.E);

            //Combo guardCombo = new Combo("Guard");
              //  guardCombo.AppendStep(new InstantComboStep("Guard", guard, 9999.0f, PlayerAnimationManager.GuardBegin));
            //ComboManager.AddCombo(guardCombo);
			
			//Combo flamethrower = new Combo("Flamethrower");
			//	flamethrower.AppendStep( new ComboStep("Flamethrower", attack, 9999.0f, new IComboInput[]{guard},
           //                              PlayerAnimationManager.Fall));
			//ComboManager.AddCombo(flamethrower);

         //   Combo aerial = new Combo("AerialCombo");
          //      aerial.AppendStep(new ComboStep("Hit0", attack, PlayerAnimationManager.ComboAerial));
          //  ComboManager.AddCombo(aerial);


			Combo punching = new Combo("Punching");
                punching.AppendStep(new PersistentComboStep("Punch0", attack, 3.0f, anim.Explosion));
                punching.AppendStep(new InstantComboStep("Punch1", attack, anim.ComboGround));
                punching.AppendStep(new InstantComboStep("Punch2", attack, anim.ComboAerial));
			ComboManager.AddCombo(punching);
		}
		else if(player == Core.zap) //Combos de zap
		{
		}
		else if(player == Core.lluvia) //Combos de lluvia
		{
		}
	}
	
	void Update() 
	{
        if (!player.IsSelected()) comboing = false;
        Debug.Log(comboing);
		if(player.IsDead()) return;
	}
	
	//Llamado cuando se ha empezado un combo
	public void OnComboStarted(Combo combo)
	{
		if(!player.IsSelected()) return;

	    Debug.Log("Started " + combo.GetName());

		comboing = true;
	}
	
	//Llamado cuando se ha acabado un combo entero
	public void OnComboFinished(Combo combo)
	{
		if(!player.IsSelected()) return;

		Debug.Log("Finished " + combo.GetName());
		if(!ComboManager.AnyComboBeingDone())
		{
			comboing = false;
		}
	}

    //Llamado cuando se ha acabado un combo entero
    public void OnComboCancelled(Combo combo)
    {
        if (!player.IsSelected()) return;

        Debug.Log("Cancelled " + combo.GetName());

        //Si no hay ningun combo haciendose, vuelve a idle
        if (!ComboManager.AnyComboBeingDone())
        {
            comboing = false;
        }
    }
	
	//SOLO llamado si el combo step es de mantener pulsado.
	//Si no, se llamara a OnComboStepDone
	public void OnComboStepDoing(ComboStep step, float time)
	{
		if(!player.IsSelected()) return;

        comboing = true;
        Debug.Log("Doing step " + step.GetName());
	}


    public void OnComboStepCancelled(ComboStep step)
	{
		if(!player.IsSelected()) return;

		Debug.Log("Cancelled step " + step.GetName());
	}

    public void OnComboStepStarted(ComboStep step)
    {
        if (!player.IsSelected()) return;

        Debug.Log("Started step " + step.GetName());
    }

	public void OnComboStepFinished(ComboStep step)
	{
		if(!player.IsSelected()) return;

        Debug.Log("Finished step " + step.GetName());
	}

	public void OnReceiveDamage()
	{
		ComboManager.CancelAllCombos();
	}

	public bool IsComboing() { return comboing; }
}
