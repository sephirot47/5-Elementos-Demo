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
				flamethrower.AppendStep( new ComboStep("Flamethrower", true, attack, 9999.0f, new IComboInput[]{guard}) );
			ComboManager.AddCombo(flamethrower);

			Combo punching = new Combo("Punching", 1.0f);
				punching.AppendStep( new ComboStep("Punch0", false, attack) );
				punching.AppendStep( new ComboStep("Punch1", false, attack) );
				punching.AppendStep( new ComboStep("Punch2", true, attack) );
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
	public void OnComboStarted(string comboName)
	{
		if(!player.IsSelected()) return;

		Debug.Log("Started " + comboName);
		attacking = true;
	}
	
	//Llamado cuando se ha acabado un combo entero
	public void OnComboDone(string comboName)
	{
		if(!player.IsSelected()) return;

		Debug.Log("Done " + comboName);
		if(!ComboManager.AnyComboBeingDone())
		{
			attacking = false;
			anim.Play(PlayerAnimationManager.Idle0);
		}
	}
	
	//SOLO llamado si el combo step es de mantener pulsado.
	//Si no, se llamara a OnComboStepDone
	public void OnComboStepDoing(string stepName, float time)
	{
		if(!player.IsSelected()) return;

		if(stepName == "Flamethrower")
		{
			anim.Play(PlayerAnimationManager.ReceiveDamage);
		}
		else if(stepName == "Punch0")
		{
			Debug.Log ("PLAY EXPLOSION");
			anim.Play(PlayerAnimationManager.Explosion);
		}
		else if(stepName == "Punch1")
		{
			Debug.Log ("PLAY GUARDBEGIN");
			anim.Play(PlayerAnimationManager.ComboGround);
		}
		else if(stepName == "Punch2")
		{
			anim.Play(PlayerAnimationManager.ComboAerial);
		}
	}

	//Llamado cuando un step de un combo se ha acabado
	public void OnComboStepCancelled(string stepName)
	{
		if(!player.IsSelected()) return;
		
		Debug.Log("Cancelled " + stepName);

		//Si no hay ningun combo haciendose, vuelve a idle
		if(!ComboManager.AnyComboBeingDone())
		{
			attacking = false;
			Debug.Log ("PLAY IDLE0");
			anim.Play(PlayerAnimationManager.Idle0);
		}
	}

	//Llamado cuando un step de un combo se ha acabado
	public void OnComboStepDone(string stepName)
	{
		if(!player.IsSelected()) return;

		Debug.Log("Done " + stepName);
	}

	public void OnReceiveDamage()
	{
		ComboManager.CancelAllCombos();
	}

	public AttackMode GetAttackMode() { return currentAttackMode; }
	public bool IsAttacking() { return attacking; }

}
