using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerCombat : MonoBehaviour
{
    private Player player;
    private PlayerAnimationManager anim;
    private PlayerComboManager playerComboMan;

    public float maxLife = 100.0f;
    private float currentLife;

    [SerializeField] private float guardAngle = 150.0f;
    [SerializeField] private float attack = 5.0f;
    [SerializeField] private float attackRange = 10.0f, attackAngle = 90.0f;

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
            return IsInAngle(transform.position, e.transform.position, transform.forward, guardAngle);

        return false;
    }

    /*Dice si el objeto TARGET esta dentro del arco de radio infinito, centrado en AXIS, y de
      abertura de ANGLE grados, desde el objeto GO*/
    private bool IsInAngle(Vector3 goPos, Vector3 targetPos, Vector3 axis, float angle)
    {
        Vector3 dirGoToTarget = ( Core.PlaneVector(targetPos) - Core.PlaneVector(goPos) ).normalized;
        return Mathf.Acos(Vector3.Dot(axis.normalized, dirGoToTarget)) * 180.0f / Mathf.PI < angle * 0.5f;
    }

    public void OnComboStepStarted(ControlledComboStep step)
    {
        TryAttack(step, true);
    }

    public void OnComboStepFinished(ControlledComboStep step)
    {
        TryAttack(step, false);
    }

    public void TryAttack(ControlledComboStep step, bool onStepStarted)
    {
        if (!player.IsSelected()) return;

        PlayerComboAttack comboAttack = null;
        try { comboAttack = (PlayerComboAttack) step.GetParentCombo(); }
        catch (InvalidCastException e)
        {
            return;  //El combo al que pertenece el step no era un combo de ataque, ergo no es un ataque
        }

        PlayerAttack comboAttackStep = comboAttack.GetPlayerAttack(step); //Obtenemos el PlayerAttack asociado a este step
        if (comboAttackStep == null) return;

        List<GameObject> enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        foreach (GameObject go in enemies)
        {
            if (comboAttackStep.CanAttack(gameObject, go, onStepStarted))
                go.GetComponent<Enemy>().ReceiveAttack(attack);
        }
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
