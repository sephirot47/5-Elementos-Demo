﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAreaAttack : MonoBehaviour
{
    PlayerMovement playerMov;
    PlayerCombat playerCombat;
    PlayerComboManager playerComboManager;

    public float lightHeight = 10.0f;
    public float movementSpeed = 8.0f, //Velocidad del lerp de la posicion
                 rotSpeed = 1.0f;      //Velocidad del lerp de la rotacion de la light hacia el terreno

    public Color colorInRange = Color.white, 
                 colorNotInRange = Color.red;
    public float damageMultiplierInCenter = 0.5f;
    public float damageRadius = 10.0f;
    public float markRangeRadius = 40.0f;

    private GameObject areaLight;
    private float originalLightIntensity;
    private bool areaMode = false;

	void Start () 
    {
        playerMov = GetComponent<PlayerMovement>();
        playerCombat = GetComponent<PlayerCombat>();
        playerComboManager = GetComponent<PlayerComboManager>();
	}
	
	void Update ()
    {
        if (Input.GetMouseButtonDown(2) && GetComponent<Player>().IsSelected())
        {
            if(!areaMode)
            {
                areaMode = true;
                playerComboManager.CancelAllCombos();
                areaLight = Instantiate(Resources.Load("AreaLight")) as GameObject;
                areaLight.transform.position = GetAreaLightPosWithoutHeight();
                areaLight.transform.forward = -GetAreaLightNormal();
                originalLightIntensity = areaLight.GetComponent<Light>().intensity;
            }
            else areaMode = false;
        }

        if (Input.GetMouseButtonDown(1) && areaMode) areaMode = false;

        if(areaMode)
        {
            Vector3 newPos = GetAreaLightPos();
            areaLight.transform.position = Vector3.Lerp(areaLight.transform.position, newPos, Time.deltaTime * movementSpeed);

            Vector3 targetNormal = GetAreaLightNormal();
            Quaternion newRot = Quaternion.LookRotation(-targetNormal);
            areaLight.transform.rotation = Quaternion.Lerp(areaLight.transform.rotation, newRot, Time.deltaTime * rotSpeed);

            Vector3 playerForward = Core.PlaneVector(areaLight.transform.position - transform.position).normalized;
            Quaternion newPlayerRot = Quaternion.LookRotation(playerForward);
            transform.rotation = Quaternion.Lerp(transform.rotation, newPlayerRot, Time.deltaTime * playerMov.rotSpeed);

            float d = Vector3.Distance(transform.position, areaLight.transform.position);
            if (d <= markRangeRadius)
            {
                if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
                {
                    areaMode = false;
                    Attack();
                }
                areaLight.GetComponent<Light>().color = colorInRange;
            }
            else
            {
                areaLight.GetComponent<Light>().color = colorNotInRange;
            }

            //float dIntensity =  (d == 0.0f ? 0.1f : d) * 2.0f;
          //  float intensity = Mathf.Min(originalLightIntensity, (range / dIntensity) * originalLightIntensity);
          //  areaLight.GetComponent<Light>().intensity = intensity;
        }

        if (!areaMode)
        {
            if (areaLight != null) Destroy(areaLight);
        }
	}

    private void Attack()
    {
        List<GameObject> enemies = new List<GameObject>();
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

        foreach(GameObject enemyGo in enemies)
        {
            EnemyCombat enemyCombat = enemyGo.GetComponent<EnemyCombat>();
            
            Vector3 areaCenter = GetAreaLightPosWithoutHeight();
            float d = Vector3.Distance(enemyGo.transform.position, areaCenter);
            if(d <= damageRadius)
            {
                float damageAttenuation = 1.0f - (d / damageRadius);
                float multiplier = damageMultiplierInCenter * damageAttenuation;
                enemyCombat.ReceiveAttack(playerCombat.GetAttack() * multiplier);
            }
        }
    }

    private Vector3 GetAreaLightNormal()
    {
        return CameraControl.GetMousePoint3DNormal();
    }

    private Vector3 GetAreaLightPosWithoutHeight() //Sin light height, asi hace fade out
    {
        return CameraControl.GetMousePoint3D();
    }

    private Vector3 GetAreaLightPos() 
    { 
        Vector3 targetPoint = CameraControl.GetMousePoint3D(),
                targetNormal = GetAreaLightNormal();
        return targetPoint + lightHeight * targetNormal;
    }

    public bool IsInAreaMode() { return areaMode;  }
}