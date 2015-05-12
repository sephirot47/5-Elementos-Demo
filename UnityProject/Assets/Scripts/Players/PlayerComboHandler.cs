using UnityEngine;
using System.Collections;

public class PlayerComboHandler : MonoBehaviour 
{
	private Player player;

	void Start() 
	{
		player = GetComponent<Player>();

		if(player == Core.kaji) //Combos de kaji
		{
			ComboInputKey space = new ComboInputKey(KeyCode.Space);
			ComboInputKey t = new ComboInputKey(KeyCode.T);
			ComboInputClick leftClick = new ComboInputClick(ComboInputClick.LEFT);

			Combo chargedPunchCombo = new Combo("chargePunchCombo");
			chargedPunchCombo.AppendStep( new ComboStep("puno1", false, leftClick) );
			chargedPunchCombo.AppendStep( new ComboStep("puno2", false, leftClick) );
			chargedPunchCombo.AppendStep(new ComboStep ("punoCargado", true, leftClick, 1.0f));
			
			Combo quickPunchCombo = new Combo("quickPunchCombo");
			quickPunchCombo.AppendStep( new ComboStep("puno1", false, leftClick) );
			quickPunchCombo.AppendStep( new ComboStep("puno2", false, leftClick) );
			quickPunchCombo.AppendStep( new ComboStep("puno3", true, leftClick) );
			
			Combo quickPunchJumpCombo = new Combo("quickPunchJumpCombo");
			quickPunchJumpCombo.AppendStep( new ComboStep("puno1", false, leftClick) );
			quickPunchJumpCombo.AppendStep( new ComboStep("puno2", false, leftClick) );
			quickPunchJumpCombo.AppendStep( new ComboStep("jump", false, space) );
			quickPunchJumpCombo.AppendStep( new ComboStep("punoAbajo", true, leftClick) );

			ComboManager.AddCombo(chargedPunchCombo);
			ComboManager.AddCombo(quickPunchCombo);
			ComboManager.AddCombo(quickPunchJumpCombo);
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
		if(player.IsDead()) return;
	}
	
	//Llamado cuando se ha empezado un combo
	public void OnComboStarted(string comboName)
	{
		if(!player.IsSelected()) return;

		Debug.Log("Started " + comboName);
	}
	
	//Llamado cuando se ha acabado un combo entero
	public void OnComboDone(string comboName)
	{
		if(!player.IsSelected()) return;

		Debug.Log("Done " + comboName);
	}
	
	//SOLO llamado si el combo step es de mantener pulsado.
	//Si no, se llamara a OnComboStepDone
	public void OnComboStepStarted(string stepName)
	{
		if(!player.IsSelected()) return;

		Debug.Log("Started " + stepName);
	}
	
	//Llamado cuando un step de un combo se ha acabado
	public void OnComboStepDone(string stepName)
	{
		if(!player.IsSelected()) return;

		Debug.Log("Done " + stepName);
	}
}
