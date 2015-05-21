using UnityEngine;
using System.Collections;

public class EnemyAnimation : MonoBehaviour 
{
    private Enemy e;
    private EnemyCombat ecombat;
    private Animation anim;

	void Start () 
    {
        e = GetComponent<Enemy>();
        ecombat = GetComponent<EnemyCombat>();
        anim = GetComponent<Animation>();
	}
	
	void Update () 
    {
        Vector3 movement = e.GetMovement();
        Debug.Log(movement);

	    if(Core.PlaneVector(movement).magnitude > 0.05f)
        {
            if (!anim.IsPlaying("Run")) anim.CrossFade("Run");
        }
        else
        {
            if (!anim.IsPlaying("Idle")) anim.CrossFade("Idle");
        }
	}
}
