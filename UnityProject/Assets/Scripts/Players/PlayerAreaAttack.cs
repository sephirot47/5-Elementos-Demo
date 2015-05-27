using UnityEngine;
using System.Collections;

public class PlayerAreaAttack : MonoBehaviour
{
    PlayerMovement playerMov;
    PlayerComboManager playerComboManager;

    public float lightHeight = 5.0f;
    public float movementSpeed = 8.0f, //Velocidad del lerp de la posicion
                 rotSpeed = 1.0f;      //Velocidad del lerp de la rotacion de la light hacia el terreno

    public Color colorInRange, colorNotInRange;
    public float range = 20.0f;

    private GameObject areaLight;
    private float originalLightIntensity;
    private bool areaMode = false;

	void Start () 
    {
        playerMov = GetComponent<PlayerMovement>();
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
                areaLight.transform.position = GetAreaLightPosInitial();
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

            float d = Vector3.Distance(transform.position, areaLight.transform.position);
            if (d <= range)
            {
                if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
                {
                    areaMode = false;
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

    private Vector3 GetAreaLightNormal()
    {
        return CameraControl.GetMousePoint3DNormal();
    }

    private Vector3 GetAreaLightPosInitial() //Sin light height, asi hace fade out
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
