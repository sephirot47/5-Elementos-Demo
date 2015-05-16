using UnityEngine;
using System.Collections;

public class PlayerComboManager : MonoBehaviour 
{
	private bool attacking = false;

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

            Combo guardCombo = new Combo("Guard");
                guardCombo.AppendStep(new ComboStep("Guard", guard, 9999.0f, PlayerAnimationManager.GuardBegin));
            //ComboManager.AddCombo(guardCombo);
			
			Combo flamethrower = new Combo("Flamethrower");
				flamethrower.AppendStep( new ComboStep("Flamethrower", attack, 9999.0f, new IComboInput[]{guard},
                                         PlayerAnimationManager.Fall));
			ComboManager.AddCombo(flamethrower);

            Combo aerial = new Combo("AerialCombo");
                aerial.AppendStep(new ComboStep("Hit0", attack, PlayerAnimationManager.ComboAerial));
            ComboManager.AddCombo(aerial);
            aerial.SetIsAerial(true);
            aerial.SetPlayerMovement(playerMov);
                

			Combo punching = new Combo("Punching");
			    punching.AppendStep( new ComboStep("Punch0", attack, PlayerAnimationManager.Explosion) );
			    punching.AppendStep( new ComboStep("Punch1", attack, PlayerAnimationManager.ComboGround) );
			    punching.AppendStep( new ComboStep("Punch2", attack, PlayerAnimationManager.ComboAerial) );
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
		if(!player.IsSelected()) attacking = false;
		if(player.IsDead()) return;
	}
	
	//Llamado cuando se ha empezado un combo
	public void OnComboStarted(Combo combo)
	{
		if(!player.IsSelected()) return;

	    //Debug.Log("Started " + combo.GetName());

		attacking = true;
	}
	
	//Llamado cuando se ha acabado un combo entero
	public void OnComboDone(Combo combo)
	{
		if(!player.IsSelected()) return;

		//Debug.Log("Done " + combo.GetName());
		if(!ComboManager.AnyComboBeingDone())
		{
			attacking = false;
		}
	}

    //Llamado cuando se ha acabado un combo entero
    public void OnComboCancel(Combo combo)
    {
        if (!player.IsSelected()) return;

        //Debug.Log("Cancelled " + combo.GetName());

        //Si no hay ningun combo haciendose, vuelve a idle
        if (!ComboManager.AnyComboBeingDone())
        {
            attacking = false;
         //   //Debug.Log("PLAY IDLE");
        }
    }
	
	//SOLO llamado si el combo step es de mantener pulsado.
	//Si no, se llamara a OnComboStepDone
	public void OnComboStepDoing(ComboStep step, float time)
	{
		if(!player.IsSelected()) return;

        attacking = true;
        //Debug.Log("Doing " + step.GetName());

		anim.Play(step.GetAnimation());
	}

	//Llamado cuando un step de un combo se ha acabado
	public void OnComboStepCancelled(ComboStep step)
	{
		if(!player.IsSelected()) return;

		//Debug.Log("Cancelled " + step.GetName());

		//Si no hay ningun combo haciendose, vuelve a idle
		if(!ComboManager.AnyComboBeingDone())
		{
			attacking = false;
            //Debug.Log("PLAY IDLE");
		}
	}

	//Llamado cuando un step de un combo se ha acabado
	public void OnComboStepDone(ComboStep step)
	{
		if(!player.IsSelected()) return;

        attacking = false;
        //Debug.Log("Done " + step.GetName() + ", (" + step.GetNextStepInputInterval().first + ", " + step.GetNextStepInputInterval().second + ")");
	}

	public void OnReceiveDamage()
	{
		ComboManager.CancelAllCombos();
	}

	public bool IsAttacking() { return attacking; }
}
