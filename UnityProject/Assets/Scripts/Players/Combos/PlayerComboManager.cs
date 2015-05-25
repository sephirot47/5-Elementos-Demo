using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerComboManager : MonoBehaviour, IComboListener
{
    private Player player;
    private PlayerMovement playerMov;
	private PlayerAnimationManager anim;

    private PlayerComboAttack groundCombo, aerialCombo, explosionCombo;
    private ControlledCombo guardCombo, chargedJumpCombo;
    private ControlledCombo dodgeRight, dodgeLeft, dodgeBack, dodgeForward;

    private List<Combo> groundCombos;
    private List<Combo> aerialCombos;

    private ComboManager comboManager;

	void Start() 
	{
		player = GetComponent<Player>();
        playerMov = GetComponent<PlayerMovement>();
		anim = GetComponent<PlayerAnimationManager>();

        groundCombos = new List<Combo>();
        aerialCombos = new List<Combo>();

        comboManager = new ComboManager(this);

        ComboInputKey jump = new ComboInputKey(KeyCode.Space);
        ComboInputKey shift = new ComboInputKey(KeyCode.LeftShift);
        ComboInputKey attack = new ComboInputKey(KeyCode.E);

        //GROUND COMBO //////////////////////
        groundCombo = new PlayerComboAttack("ground", comboManager);
        groundCombo.AppendStep(new InstantComboStep("ground1", attack, anim.ComboGround1), new PlayerAttack(4.0f, 90.0f, 1.0f, 0.05f));
        groundCombo.AppendStep(new InstantComboStep("ground2", attack, anim.ComboGround2), new PlayerAttack(8.0f, 360.0f, 1.0f, 0.4f));
        groundCombo.AppendStep(new InstantComboStep("ground3", attack, anim.ComboGround3), new PlayerAttack(4.0f, 90.0f, 1.0f, 0.8f));
        groundCombo.AppendStep(new InstantComboStep("ground4", attack, anim.ComboGround4), new PlayerAttack(4.0f, 90.0f, 1.0f, 0.6f));
        comboManager.AddCombo(groundCombo);
        //////////////////////////////////////

        aerialCombo = new PlayerComboAttack("aerial", comboManager);
        aerialCombo.AppendStep(new InstantComboStep("aerial1", attack, anim.ComboAerial), new PlayerAttack(4.0f, 90.0f, 1.0f, 0.75f));
        comboManager.AddCombo(aerialCombo);

        //Afecta la mitad, pero ataca a todos los de alrededor, y en un rango mayor
        explosionCombo = new PlayerComboAttack("explosion", comboManager);
        explosionCombo.AppendStep(
            new InstantComboStep("explosion0", attack, new IComboInput[]{ shift }, anim.ComboGround2),
            new PlayerAttack(8.0f, 360.0f, 0.5f, 0.4f) );
        comboManager.AddCombo(explosionCombo);

        guardCombo = new ControlledCombo("guard", comboManager);
        guardCombo.AppendStep(new InfiniteComboStep("guard0", shift, anim.GuardBegin));
        comboManager.AddCombo(guardCombo);

        chargedJumpCombo = new ControlledCombo("chargedJump", comboManager);
        chargedJumpCombo.AppendStep(new DurableComboStep("chargedJump0", shift, 1.0f, new IComboInput[] { shift }, anim.Explosion));
        chargedJumpCombo.AppendStep(new InstantComboStep("chargedJump1", jump, new IComboInput[] { shift }, anim.Fall));
        comboManager.AddCombo(chargedJumpCombo);

        /////// DODGES //////////////////////////////////////////////
        dodgeRight = new ControlledCombo("dodgeRight", comboManager);
        dodgeRight.AppendStep(
            new InstantComboStep("dodgeRight0", new ComboInputKey(KeyCode.D), new IComboInput[] { shift }, anim.Fall));
        comboManager.AddCombo(dodgeRight);

        dodgeLeft = new ControlledCombo("dodgeLeft", comboManager);
        dodgeLeft.AppendStep(
            new InstantComboStep("dodgeLeft0", new ComboInputKey(KeyCode.A), new IComboInput[] { shift }, anim.Fall));
        comboManager.AddCombo(dodgeLeft);

        dodgeBack = new ControlledCombo("dodgeBack", comboManager);
        dodgeBack.AppendStep(
            new InstantComboStep("dodgeBack0", new ComboInputKey(KeyCode.S), new IComboInput[] { shift }, anim.Fall));
        comboManager.AddCombo(dodgeBack);

        dodgeForward = new ControlledCombo("dodgeForward", comboManager);
        dodgeForward.AppendStep(
            new InstantComboStep("dodgeForward0", new ComboInputKey(KeyCode.W), new IComboInput[] { shift }, anim.Fall));
        comboManager.AddCombo(dodgeForward);
        /////////////////////////////////////////////////////////////////////

        groundCombos.Add(groundCombo);
        groundCombos.Add(explosionCombo);
        groundCombos.Add(guardCombo);
        groundCombos.Add(dodgeRight);
        groundCombos.Add(dodgeLeft);
        groundCombos.Add(dodgeBack);
        groundCombos.Add(dodgeForward);

        aerialCombos.Add(aerialCombo);
        aerialCombos.Add(chargedJumpCombo);

        DisableAllCombos();
	}
	
	void Update() 
	{
        comboManager.Update();

        if (!player.IsSelected())
        {
            DisableAllCombos();
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

        if (combo.GetName().Contains("chargedJump") || combo.GetName().Contains("aerial"))
        {
            playerMov.SetSuspendedInAir(true);
        }
	}
	
	//Llamado cuando se ha acabado un combo entero
	public void OnComboFinished(Combo combo)
	{
		if(!player.IsSelected()) return;

        playerMov.SetSuspendedInAir(false);
	}

    //Llamado cuando se ha acabado un combo entero
    public void OnComboCancelled(Combo combo)
    {
        if (!player.IsSelected()) return;

        //Debug.Log("Cancelled " + combo.GetName());
        playerMov.SetSuspendedInAir(false);
    }
	
	//SOLO llamado si el combo step es de mantener pulsado.
	//Si no, se llamara a OnComboStepDone
	public void OnComboStepDoing(ComboStep step, float time)
	{
		if(!player.IsSelected()) return;
        if(player.GetTarget() != null)
        { 
            GetComponent<PlayerCombat>().OnComboStepDoing(step, time);
            if ( !step.GetName().Contains("chargedJump") )
                player.transform.forward = player.GetTarget().transform.position - player.transform.position;
        }
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
        ControlledComboStep ccs = null;

        try { ccs = (ControlledComboStep) step;  }
        catch (InvalidCastException e) { return; }

        GetComponent<PlayerCombat>().OnComboStepStarted(ccs);

        if (step.GetName() == "chargedJump1")
        {
            playerMov.SetSuspendedInAir(false);
            playerMov.Boost(transform.forward, 1.5f);
        }
        else if(step.GetName() == "dodgeRight0")
        {
            playerMov.Boost(Camera.main.transform.right);
        }
        else if (step.GetName() == "dodgeLeft0")
        {
            playerMov.Boost(-Camera.main.transform.right);
        }
        else if (step.GetName() == "dodgeForward0")
        {
            playerMov.Boost(Camera.main.transform.forward);
        }
        else if (step.GetName() == "dodgeBack0")
        {
            playerMov.Boost(-Camera.main.transform.forward);
        }

        //Debug.Log("Started step " + step.GetName());
    }

	public void OnComboStepFinished(ComboStep step)
	{
        if (!player.IsSelected()) return;
        ControlledComboStep ccs = null;

        try { ccs = (ControlledComboStep)step; }
        catch (InvalidCastException e) { return; }

        GetComponent<PlayerCombat>().OnComboStepFinished(ccs);
        //Debug.Log("Finished step " + step.GetName());
	}

    public void DisableAllCombos()
    {
        foreach (ControlledCombo c in groundCombos) c.SetEnabled(false);
        foreach (ControlledCombo c in aerialCombos) c.SetEnabled(false);
    }

    public void EnableGroundCombos()
    {
        foreach (ControlledCombo c in groundCombos) c.SetEnabled(true);
    }

    public void EnableAerialCombos()
    {
        foreach (ControlledCombo c in aerialCombos) c.SetEnabled(true);
    }


	public void OnReceiveDamage()
	{
	}

    public bool IsComboingStep(string stepName)
    {
        foreach (ControlledCombo c in groundCombos)
            foreach(ControlledComboStep s in c.GetSteps())
                if (s.Started() && s.GetName() == stepName) return true;

        foreach (ControlledCombo c in aerialCombos)
            foreach (ControlledComboStep s in c.GetSteps())
                if (s.Started() && s.GetName() == stepName) return true;

        return false;
    }

    public bool IsComboingCombo(string comboName) 
    { 
        foreach (ControlledCombo c in groundCombos)
            if(c.BeingDone() && c.GetName() == comboName) return true;

        foreach (ControlledCombo c in aerialCombos)
            if (c.BeingDone() && c.GetName() == comboName) return true;

        return false;
    }

    public void CancelAllCombos()
    {
        if (comboManager != null) 
            comboManager.CancelAllCombos();
    }

    public bool AnyComboBeingDone() { return comboManager.AnyComboBeingDone(); }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   