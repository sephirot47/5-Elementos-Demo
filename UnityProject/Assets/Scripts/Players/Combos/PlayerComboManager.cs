using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerComboManager : MonoBehaviour 
{
	private bool comboing = false;

    private Player player;
    private PlayerMovement playerMov;
	private PlayerAnimationManager anim;

    private Dictionary<ComboStep, PlayerAttack> stepAttack; //Para cada combo step, contiene el ataque al que esta asociado

    private PlayerComboAttack groundCombo, aerialCombo, explosionCombo;
    private Combo guardCombo, chargedJumpCombo;

    private List<Combo> groundCombos;
    private List<Combo> aerialCombos;

	void Start() 
	{
		player = GetComponent<Player>();
        playerMov = GetComponent<PlayerMovement>();
		anim = GetComponent<PlayerAnimationManager>();

        stepAttack = new Dictionary<ComboStep, PlayerAttack>();

        groundCombos = new List<Combo>();
        aerialCombos = new List<Combo>();

        ComboInputKey jump = new ComboInputKey(KeyCode.Space);
        ComboInputKey shift = new ComboInputKey(KeyCode.LeftShift);
        ComboInputKey attack = new ComboInputKey(KeyCode.E);

        //GROUND COMBO //////////////////////
        groundCombo = new PlayerComboAttack("ground");
        groundCombo.AppendStep(new InstantComboStep("ground1", attack, anim.ComboGround1), new PlayerAttack() );
        groundCombo.AppendStep(new InstantComboStep("ground2", attack, anim.ComboGround2), new PlayerAttack());
        groundCombo.AppendStep(new InstantComboStep("ground3", attack, anim.ComboGround3), new PlayerAttack());
        groundCombo.AppendStep(new InstantComboStep("ground4", attack, anim.ComboGround4), new PlayerAttack());
        ComboManager.AddCombo(groundCombo);
        //////////////////////////////////////

        aerialCombo = new PlayerComboAttack("aerial");
        aerialCombo.AppendStep(new InstantComboStep("aerial1", attack, anim.ComboAerial), new PlayerAttack());
        ComboManager.AddCombo(aerialCombo);

        //Afecta la mitad, pero ataca a todos los de alrededor, y en un rango mayor
        explosionCombo = new PlayerComboAttack("explosion");
        explosionCombo.AppendStep(
            new PersistentComboStep("explosion0", attack, 3.0f, new IComboInput[]{ shift }, anim.Explosion),
            new PlayerAttack(10.0f, 360.0f, 0.5f, false) );
        ComboManager.AddCombo(explosionCombo);


        guardCombo = new Combo("guard");
        guardCombo.AppendStep(new InfiniteComboStep("guard0", shift, anim.GuardBegin));
        ComboManager.AddCombo(guardCombo);

        chargedJumpCombo = new Combo("chargedJump");
        chargedJumpCombo.AppendStep(new PersistentComboStep("chargedJump0", jump, 3.0f, new IComboInput[] { shift }, anim.Die));
        //ComboManager.AddCombo(chargedJumpCombo);

        groundCombos.Add(groundCombo);
        groundCombos.Add(explosionCombo);
        groundCombos.Add(guardCombo);

        aerialCombos.Add(aerialCombo);
        aerialCombos.Add(chargedJumpCombo);

        DisableAllCombos();
	}
	
	void Update() 
	{
        if (!player.IsSelected())
        {
            DisableAllCombos();
            comboing = false; 
            return;
        }

        if(player.IsDead()) return;

        if(playerMov.IsGrounded())
        {
            DisableAllCombos();
            EnableGroundCombos();
        }
        else
        {
            DisableAllCombos();
            EnableAerialCombos();
        }
	}
	
	//Llamado cuando se ha empezado un combo
	public void OnComboStarted(Combo combo)
	{
		if(!player.IsSelected()) return;
        if(player.GetTarget() != null) playerMov.LookToTarget();

	    //Debug.Log("Started " + combo.GetName());

        if (combo.GetName() == "chargedJump0" || combo.GetName() == "aerial1")
        {
            playerMov.SetSuspendedInAir(true);
        }

		comboing = true;
	}
	
	//Llamado cuando se ha acabado un combo entero
	public void OnComboFinished(Combo combo)
	{
		if(!player.IsSelected()) return;

        if (combo.GetName() == "chargedJump0" || combo.GetName() == "aerial1")
        {
            playerMov.SetSuspendedInAir(false);
        }

		//Debug.Log("Finished " + combo.GetName());
		if(!ComboManager.AnyComboBeingDone())
		{
			comboing = false;
		}
	}

    //Llamado cuando se ha acabado un combo entero
    public void OnComboCancelled(Combo combo)
    {
        if (!player.IsSelected()) return;

        //Debug.Log("Cancelled " + combo.GetName());

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
        //Debug.Log("Doing step " + step.GetName());
	}


    public void OnComboStepCancelled(ComboStep step)
	{
		if(!player.IsSelected()) return;

		Debug.Log("Cancelled step " + step.GetName());
	}

    public void OnComboStepStarted(ComboStep step)
    {
        if (!player.IsSelected()) return;

        GetComponent<PlayerCombat>().OnComboStepStarted(step);
        //Debug.Log("Started step " + step.GetName());
    }

	public void OnComboStepFinished(ComboStep step)
	{
		if(!player.IsSelected()) return;

        GetComponent<PlayerCombat>().OnComboStepFinished(step);
        //Debug.Log("Finished step " + step.GetName());
	}

    public void DisableAllCombos()
    {
        foreach (Combo c in groundCombos) c.SetEnabled(false);
        foreach (Combo c in aerialCombos) c.SetEnabled(false);
    }

    public void EnableGroundCombos()
    {
        foreach (Combo c in groundCombos) c.SetEnabled(true);
    }

    public void EnableAerialCombos()
    {
        foreach (Combo c in aerialCombos) c.SetEnabled(true);
    }


	public void OnReceiveDamage()
	{
	}

    public bool IsComboingStep(string stepName)
    {
        foreach (Combo c in groundCombos)
            foreach(ComboStep s in c.GetSteps())
                if (s.Started() && s.GetName() == stepName) return true;

        foreach (Combo c in aerialCombos)
            foreach (ComboStep s in c.GetSteps())
                if (s.Started() && s.GetName() == stepName) return true;

        return false;
    }

    public bool IsComboingCombo(string comboName) 
    { 
        foreach (Combo c in groundCombos)
            if(c.BeingDone() && c.GetName() == comboName) return true;

        foreach (Combo c in aerialCombos)
            if (c.BeingDone() && c.GetName() == comboName) return true;

        return false;
    }

	public bool IsComboing() { return comboing; }
}
