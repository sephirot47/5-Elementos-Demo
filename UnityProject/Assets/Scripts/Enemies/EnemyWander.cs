using UnityEngine;
using System.Collections;

public class EnemyWander : MonoBehaviour
{
	Enemy enemy;

	private float time; //Cada este tiempo, cambia de direccion
	private Vector3 randomDir; //Direccion random que cambia cada cierto tiempo

	private Vector3 originalPosition; //Posicion en la que spawnea, para tema de moverse a lo random cuando estan solos
	
	public float stopProbabilities = 0.7f;	
	public float wanderRange = 10.0f; //rango en el que 'vaga', cuando no tiene a nadie alrededor y tal
	public float wanderTime = 5.0f;

	void Start () 
	{
		enemy = GetComponentInParent<Enemy> ();
		originalPosition = enemy.transform.position;
		time = 0.0f;
		randomDir = Vector3.zero;
	}

	void Update () 
	{	
		if(!GameState.IsPlaying()) return;

		Vector3 movement = Vector3.zero;
		Vector3 dirToOriginal = (originalPosition - transform.position);
		if(enemy.target == null)
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
					movement += transform.forward * enemy.speed * 0.35f;
				}
				//
				
				time += Time.deltaTime;
			}
			else //Esta fuera de su zona, ha de volver echando leches hihi
			{
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dirToOriginal), Time.deltaTime);
				movement += transform.forward * enemy.speed * 0.5f;
			}
		}
		enemy.controller.Move(movement * Time.deltaTime);
	}

	Vector3 GetRandomDeviation()
	{
		float lower = 0.0f, upper = 0.3f;
		return new Vector3(Random.Range(lower, upper), 0.0f, Random.Range(lower, upper));
	}

	bool IsWalking()
	{
		return randomDir.magnitude > 0.0f;
	}

}
