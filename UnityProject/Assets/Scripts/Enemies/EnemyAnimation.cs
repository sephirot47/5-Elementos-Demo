using UnityEngine;
using System.Collections;

public class EnemyAnimation : MonoBehaviour, IComboListener, ICustomAnimationListener
{
    private Enemy e;
    private EnemyCombat ecombat;
    private Animation anim;

    public CustomAnimation Idle, Run, Attack, ReceiveDamage, Die;

    private ComboManager comboManager;
    private SimulatedCombo comboAttack;

	void Start()
    {
        e = GetComponent<Enemy>();
        ecombat = GetComponent<EnemyCombat>();
        anim = GetComponent<Animation>();

        Idle = new CustomAnimation("Idle", anim);
        Run = new CustomAnimation("Run", anim);

        Attack = new CustomAnimation("Attack", anim);
        ReceiveDamage = new CustomAnimation("ReceiveDamage", anim, 1.0f, this);
        Die = new CustomAnimation("Die", anim, 1.0f, this);

        comboManager = new ComboManager();
        
        comboAttack = new SimulatedCombo("Attack", comboManager);
        comboAttack.AppendStep(new SimulatedComboStep("stepAttack", Attack));
        comboManager.AddCombo(comboAttack);

        comboManager.AddListener(this);
	}
	
    public void PlayAttack()
    {
        if(!comboManager.AnyComboBeingDone())
            comboAttack.Simulate();
    }

	void Update()
    {
        Die.Update();
        ReceiveDamage.Update();

        if (!GameState.IsPlaying() || GetComponent<EnemyCombat>().Dead()) return;

        comboManager.Update();

        Vector3 movement = e.GetMovement();
        if (!comboManager.AnyComboBeingDone() && !ReceiveDamage.IsPlaying())
        {
	        if(Core.PlaneVector(movement).magnitude > 0.05f)
            {
                if (!Run.IsPlaying()) Run.Play();
            }
            else
            {
                if (!Idle.IsPlaying()) Idle.Play();
            }
        }
	}

    public void OnDie()
    {
        comboManager.CancelAllCombos();
        if(!ReceiveDamage.IsPlaying()) Die.Play();
    }

    public void OnReceiveAttack()
    {
        if (ecombat.Dead() && !Die.IsPlaying()) { Die.Play(); return; }
        
        comboManager.CancelAllCombos();
        ReceiveDamage.ForcePlay();
    }

    public void OnAnimationStarted(CustomAnimation anim)
    {
        //Debug.Log("Animation Started " + anim.GetName());
    }

    public void OnAnimationFinished(CustomAnimation anim)
    {
        //Debug.Log("Animation Finished " + anim.GetName());
        if(anim == Die)
        {
            Destroy(gameObject, 3.0f);
        }
        else if(anim == ReceiveDamage)
        {
            if (ecombat.Dead()) Die.Play();
            else if( !Attack.IsPlaying() || ReceiveDamage.IsPlaying() )
                Idle.Play();
        }
    }

    //Llamado mientras un combo step largo se esta haciendo
    public void OnComboStepDoing(ComboStep step, float time)
    {
    }

    //Llamado cuando se cancela un combostep
    public void OnComboStepCancelled(ComboStep step)
    {
        //Debug.Log("Cancelled " + step.GetName());
    }

    //Llamado al iniciarse un step
    public void OnComboStepStarted(ComboStep step)
    {
        //Debug.Log("Started " + step.GetName());
    }

    //Llamado al finalizar un step
    public void OnComboStepFinished(ComboStep step)
    {
       // Debug.Log("Finished " + step.GetName());
        GetComponent<EnemyCombat>().OnAttackFinished();
    }



    //Llamado cuando se ha empezado un combo
    public void OnComboStarted(Combo combo)
    {
      //  Debug.Log("Started " + combo.GetName());
    }

    //Llamado cuando se ha acabado un combo entero
    public void OnComboFinished(Combo combo)
    {
      //  Debug.Log("Finished " + combo.GetName());
    }

    //Llamado cuando se ha acabado un combo entero
    public void OnComboCancelled(Combo combo)
    {
      //  Debug.Log("Cancelled " + combo.GetName());
    }
}
