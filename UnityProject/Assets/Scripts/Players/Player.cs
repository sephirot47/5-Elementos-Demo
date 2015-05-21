using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	private PlayerAnimationManager anim;
    private PlayerCombat playerCombat;

	private float aggro = 0.0f;

	void Start()
    {
		anim = GetComponent<PlayerAnimationManager>();
        playerCombat = GetComponent<PlayerCombat>();
	}

	void Update()
	{
		if(!IsSelected()) return;

        GameObject target = GetTarget();
		if(GameState.IsPlaying() && !GameState.AllPlayersDead())
		{
			if(Input.GetButtonDown("Speak"))
			{
				if(target != null && target.CompareTag("NPC"))
				{
					SpeakTo(target.GetComponent<NPC>());
				}
			}
		}
	}

	private void SpeakTo(NPC npc)
	{
		if(npc != null) npc.OnSpeakWithMe(this);
	}

	public GameObject GetTarget()
	{
		return GetComponent<PlayerTarget>().GetTarget();
	}

	public void Die() { playerCombat.Die(); }
	public bool IsDead() {  return playerCombat.IsDead(); }
    public bool IsSelected() { return Core.selectedPlayer == this; }

    public float GetAggro() { return aggro; }
    public void OnApplyDamage() { aggro += playerCombat.GetAttack(); }
    public float GetCurrentLife() { return playerCombat.GetCurrentLife(); }
    public float GetMaxLife() { return playerCombat.GetMaxLife(); }
    public Vector3 GetMovement() { return GetComponent<PlayerMovement>().movement; }
}
