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

    [SerializeField] private float guardAngle = 220.0f;
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

    public void TryAttack(ComboStep step, float normalizedTime = -1.0f)
    {
        if (!player.IsSelected()) return;

        PlayerComboAttack comboAttack = null;
        try { comboAttack = (PlayerComboAttack) step.GetParentCombo(); }
        catch (InvalidCastException e) { return; } //El combo al que pertenece el step no era un combo de ataque, ergo no es un ataque
        
        PlayerAttack comboAttackStep = comboAttack.GetPlayerAttack(step); //Obtenemos el PlayerAttack asociado a este step
        if (comboAttackStep == null) return;

        if(!comboAttackStep.AttackedInThisStep())
        {
            float animNormalizedTime = normalizedTime == -1.0f ? step.GetAnimation().GetNormalizedTime() : normalizedTime;
            List<GameObject> enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));

            foreach (GameObject enemy in enemies)
            {
                if (comboAttackStep.CanAttack(gameObject, enemy, animNormalizedTime)) 
                {
                    enemy.GetComponent<Enemy>().ReceiveAttack(attack); //FIXME Multiplicar por attack multiplier
                }
            }
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

    //OJO QUE ESTOS NO SON DEL LISTENER, LOS LLAMA PlayerComboManager !!!!!!!!!!!!!!!!   ;)
    public void OnComboStepStarted(ComboStep step)
    {
        //Same as on OnComboStepFinished, lo unico que al start no tiene mucho sentido pero meh
        TryAttack(step, 0.0f);
    }

    public void OnComboStepDoing(ComboStep step, float time)
    {
        TryAttack(step);
    }

    public void OnComboStepFinished(ComboStep step)
    {
        //Se lo he de pasar asi ya que se puede pasar al siguiente step sin acabarlo, por el tema
        //de blending y tal
        //Por lo tanto, a lo mejor en estos momentos el normalizedTime de la animacion del step
        //podria valer quizas 0.6, y ya al siguiente frame pasar al siguiente step :)
        TryAttack(step, 1.0f);
        //Debug.Log("FINISHED " + step.GetName());
    }
    /////////////////////////////////
}
