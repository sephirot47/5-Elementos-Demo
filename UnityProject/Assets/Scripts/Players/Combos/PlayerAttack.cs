using System;
using UnityEngine;
using System.Collections.Generic;

class PlayerAttack
{
    private float attackMultiplier = 1.0f; //Mutiplicador de la fuerza de ataque normal del player
    private float attackRange = 3.0f;      //Lo lejos que llega el ataque
    private float attackAngle = 150.0f;     //Lo amplio que llega el ataque (radio del cono)

    //Indica si el ataque en si(el dano) se efectua al empezar la animacion(true) o al acabarla(false)
    private bool attackOnAnimationStart = true;    

    public PlayerAttack(float attackRange = 3.0f, float attackAngle = 150.0f, float attackMultiplier = 1.0f, 
                      bool attackOnAnimationStart = true)
    {
        this.attackRange = attackRange;
        this.attackAngle = attackAngle;
        this.attackMultiplier = attackMultiplier;
        this.attackOnAnimationStart = attackOnAnimationStart;
    }

    public bool CanAttack(GameObject from, GameObject to, bool isOnComboStarted)
    {
        if (attackOnAnimationStart) { if (!isOnComboStarted) return false; }
        else if (isOnComboStarted) return false;

        float d = Vector3.Distance(from.transform.position, to.transform.position);
        if (d <= attackRange)
        {
            return IsInAngle(from.transform.position, to.transform.position, from.transform.forward, attackAngle);
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
