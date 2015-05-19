using UnityEngine;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    private Player player;
    private PlayerAnimationManager anim;
    private PlayerComboManager playerComboMan;

    public float maxLife = 100.0f;
    private float currentLife;


    [SerializeField] private float guardAngle = 150.0f;
    [SerializeField] private float attack = 5.0f;

	void Start () 
    {
        player = GetComponent<Player>();
        anim = GetComponent<PlayerAnimationManager>();
        playerComboMan = GetComponent<PlayerComboManager>();

        currentLife = maxLife;
	}
	
	void Update () 
    {
	    
	}

    public void Attack(Enemy e)
    {

    }

    public void ReceiveAttack(Enemy e)
    {
        if (!IsGuardingFrom(e))
        {
            currentLife -= e.GetAttack();
            playerComboMan.OnReceiveDamage();

            if (IsDead() && anim != null) {  Die(); }
            else if (anim != null) anim.Play(anim.ReceiveDamage);
        }
    }

    private bool IsGuardingFrom(Enemy e)
    {
        if (playerComboMan.IsComboingCombo("guard"))
        {
            Vector3 dirPlayerToEnemy = (e.transform.position - transform.position).normalized;
            return Mathf.Acos(Vector3.Dot(transform.forward, dirPlayerToEnemy)) * 180.0f/Mathf.PI < guardAngle * 0.5f;
        }

        return false;
    }

    public void Die()
    {
        currentLife = 0;
    }

    public bool IsDead() { return currentLife <= 0; }
    public float GetCurrentLife() { return currentLife; }
    public float GetMaxLife() { return maxLife; }
    public float GetAttack() { return attack; }
}
