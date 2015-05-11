using UnityEngine;
using System.Collections;

public class PlayerComboManager : MonoBehaviour 
{
	public enum AttackMode {Melee, Ranged, Elemental, Taunt};

	private AttackMode currentAttackMode = AttackMode.Melee;
	private bool attacking = false;

	private Player player;
	private PlayerAnimationManager anim;

	void Start() 
	{
		player = GetComponent<Player>();
		anim = GetComponent<PlayerAnimationManager>();

		if(player == Core.kaji) //Combos de kaji
		{
			ComboInputKey jump = new ComboInputKey(KeyCode.Space);
			ComboInputKey guard = new ComboInputKey(KeyCode.LeftControl);
			ComboInputKey attack = new ComboInputKey(KeyCode.E);
			
			
			Combo flamethrower = new Combo("Flamethrower");
				flamethrower.AppendStep( new ComboStep("Flamethrower", attack, 9999.0f, new IComboInput[]{guard}, 
										 PlayerAnimationManager.GuardBegin) );
			ComboManager.AddCombo(flamethrower);

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
			anim.Play(PlayerAnimationManager.Idle0);
		}
	}
	
	//SOLO llamado si el combo step es de mantener pulsado.
	//Si no, se llamara a OnComboStepDone
	public void OnComboStepDoing(ComboStep step, float time)
	{
		if(!player.IsSelected()) return;

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
			anim.Play(PlayerAnimationManager.Idle0);
		}
	}

	//Llamado cuando un step de un combo se ha acabado
	public void OnComboStepDone(ComboStep step)
	{
		if(!player.IsSelected()) return;

		//Debug.Log("Done " + step.GetName());
	}

	public void OnReceiveDamage()
	{
		ComboManager.CancelAllCombos();
	}

	public AttackMode GetAttackMode() { return currentAttackMode; }
	public bool IsAttacking() { return attacking; }

}
