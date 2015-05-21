using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
	private Enemy enemy;
    private CharacterController controller;

	private float time; //Cada este tiempo, cambia de direccion
	private Vector3 randomDir; //Direccion random que cambia cada cierto tiempo

	[SerializeField] private float rotSpeed = 5.0f; //Velocidad de rotacion hacia el objetivo

	private Vector3 originalPosition; //Posicion en la que spawnea, para tema de moverse a lo random cuando estan solos

    public float speed = 0.7f;	
	public float stopProbabilities = 0.7f;	
	public float wanderRange = 10.0f; //rango en el que 'vaga', cuando no tiene a nadie alrededor y tal
	public float wanderTime = 5.0f;

    private Vector3 movement;

	void Start () 
	{
		enemy = GetComponent<Enemy> ();
		controller = GetComponent<CharacterController> ();
        originalPosition = enemy.transform.position;
        randomDir = Vector3.zero;
        movement = Vector3.zero;
		time = 0.0f;
	}

	void Update () 
	{	
		if(!GameState.IsPlaying()) return;

        Player targetP = GetComponent<EnemyTarget>().GetTarget();
        if (targetP == null) return;
        GameObject target = targetP.gameObject;

		Vector3 dirToOriginal = (originalPosition - transform.position);
		if(target == null)
		{
			if(dirToOriginal.magnitude < wanderRange)
			{
				//Hacemos que se mueva random cerca de por donde respawnea
				if(time >= wanderTime)
				{
					time = 0.0f;
					if(!IsWalking() || Random.Range(0.0f, 100.0f) > (100.0f * stopProbabilities))
					{
						randomDir = dirToOriginal.normalized + GetRandomDeviation();
						randomDir.y = 0;
						randomDir.Normalize();
					}
					else
					{
						randomDir = Vector3.zero; 
					}
				}
				if( IsWalking() )
				{
					//Mira hacia donde caminas, suavemente xd
					transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(randomDir), Time.deltaTime);
					movement = transform.forward * speed * 0.35f;
				}
				//
				
				time += Time.deltaTime;
			}
			else //Esta fuera de su zona, ha de volver echando leches hihi
			{
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dirToOriginal), Time.deltaTime);
				movement = transform.forward * speed * 0.5f;
			}
		}
        else //HAY TARGET
		{
            if (!GetComponent<EnemyCombat>().InRange()) //Aun esta lejos del player, a perseguirlo!
            {
                //A perseguir al player que mas aggro tieneeee!		
                Vector3 dir = target.gameObject.transform.position - transform.position;
                dir = new Vector3(dir.x, 0, dir.z);

                movement = dir.normalized * speed;
            }
            else movement.x = movement.z = 0.0f;

			//Miramos hacia el jugador
			Quaternion newRot =  Quaternion.LookRotation(Core.PlaneVector(target.transform.position - transform.position));
			transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * GetComponent<EnemyMovement>().rotSpeed);

			controller.Move(movement * Time.deltaTime);
		}

        movement += Vector3.up * Core.gravity; //gravity
        Debug.Log(movement + "~~~~~");
        controller.Move(movement * Time.deltaTime);
	}

	Vector3 GetRandomDeviation()
	{
		float lower = 0.0f, upper = 0.3f;
		return new Vector3(Random.Range(lower, upper), 0.0f, Random.Range(lower, upper));
	}

    public Vector3 GetMovement() { return movement; }


	bool IsWalking()
	{
		return randomDir.magnitude > 0.0f;
	}

}
