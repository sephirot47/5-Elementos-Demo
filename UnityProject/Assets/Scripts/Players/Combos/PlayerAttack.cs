using System;
using UnityEngine;
using System.Collections.Generic;

class PlayerAttack
{
    private float attackMultiplier; //Mutiplicador de la fuerza de ataque normal del player
    private float attackRange;      //Lo lejos que llega el ataque
    private float attackAngle;     //Lo amplio que llega el ataque (angulo del cono)

    //Indica en que momento del combo se produce el ataque
    //Siendo 0.0f el inicio, y 1.0f el final
    private float whenAttack;
    private bool isAreaAttack; //Si es false, solo puede atacar al target. Si es true, puede atacar a mas de uno, y no necesariamente el target
    private bool attackedInThisStep = false; //Guarda si en el actual step ya ha atacado

    public PlayerAttack(float attackRange = 4.0f, float attackAngle = 180.0f, float attackMultiplier = 1.0f, float whenAttack = 0.5f, bool isAreaAttack = false)
    {
        this.attackRange = attackRange;
        this.attackAngle = attackAngle;
        this.attackMultiplier = attackMultiplier;
        this.whenAttack = whenAttack;
        this.isAreaAttack = isAreaAttack;
    }

    public void Initialize()
    {
        attackedInThisStep = false;
    }

    public bool AttackedInThisStep()
    {
        return attackedInThisStep;
    }

    public bool CanAttack(GameObject from, GameObject to, float normalizedAnimationTime)
    {
        if (normalizedAnimationTime < whenAttack) return false;

        //Si no es en area, solo puede atacar al target
        if (!isAreaAttack && to != Core.selectedPlayer.GetTarget()) return false;

        float d = Vector3.Distance(from.transform.position, to.transform.position);
        if (d <= attackRange)
        {
            if(IsInAngle(from.transform.position, to.transform.position, from.transform.forward, attackAngle))
            {
                attackedInThisStep = true;
                return true;
            }
        }

        return false;
    }

    /*Dice si el objeto TARGET esta dentro del arco de radio infinito, centrado en AXIS, y de
      abertura de ANGLE grados, desde el objeto GO*/
    private bool IsInAngle(Vector3 goPos, Vector3 targetPos, Vector3 axis, float angle)
    {
        Vector3 dirGoToTarget = (Core.PlaneVector(targetPos) - Core.PlaneVector(goPos)).normalized;
        return Mathf.Acos(Vector3.Dot(axis.normalized, dirGoToTarget)) * 180.0f / Mathf.PI < angle * 0.5f;
    }
}
