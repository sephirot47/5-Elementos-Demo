using UnityEngine;
using System.Collections;

public class Targetable : MonoBehaviour 
{
    public Transform crosshairTransform;

    public Vector3 GetCrosshairPos()
    {
        return crosshairTransform == null ? Vector3.zero : crosshairTransform.position;
    }
}
