using UnityEngine;
using System.Collections;

public class EnemyCombat : MonoBehaviour {
	
	Enemy enemy;
	[SerializeField] private float attack = 10.0f; //fuerza de ataque
	[SerializeField] private float attackRange = 2.0f; //Rango de ataque(lo lejos que llega)
    [SerializeField] private float attackRate = 1.0f; //Seconds / attack
	[SerializeField] private float rotSpeed = 5.0f; //Velocidad de rotacion hacia el objetivo
    [SerializeField] private float recoverDelay = 1.0f; //Tiempo que tarda en recuperarse despues de recibir un ataque!
    [SerializeField] protected float maxLife = 100.0f; //vida maxima

    private float currentLife; //vida actual

    private float attackRateTime = 0.0f;
    private float recoverTime = 0.0f;

	void Start () 
	{
		enemy = GetComponentInParent<Enemy>();
        currentLife = maxLife; //Empieza con 100% vida
	}

	void Update () 
	{
		if(!GameState.IsPlaying()) return;

        if (currentLife <= 0)
        {
            Die();
            return;
        }

		attackRateTime += Time.deltaTime;
        recoverTime += Time.deltaTime;
        if (recoverTime < recoverDelay) attackRateTime = 0.0f; //mientras se este recuperando de un golpe, no ataca

		Player target = enemy.GetTarget();
		if(target != null)
		{
			Vector3 movement = Vector3.zero;
			float distanceToTarget = Vector3.Distance(target.gameObject.transform.position, transform.position);

			if(distanceToTarget < attackRange) //Ya esta cerca del player, attaaack!
			{
				if(attackRateTime >= attackRate)
				{
					Attack(target);
				}
			}
			else //Aun esta lejos del player, a perseguirlo!
			{
				//A perseguir al player que mas aggro tieneeee!		
				Vector3 dir = target.gameObject.transform.position - transform.position;
				dir = new Vector3(dir.x, 0, dir.z);
				
				movement += dir.normalized * enemy.speed;
			}

			//Miramos hacia el jugador
			Quaternion newRot =  Quaternion.LookRotation(Core.PlaneVector(target.transform.position - transform.position));
			transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * rotSpeed);

			enemy.controller.Move(movement * Time.deltaTime);
		}
	}

    public void ReceiveAttack(float damage)
    {
        currentLife -= damage;
        attackRateTime = recoverTime = 0.0f;
    }

	public void Attack(Player p)
	{
		p.GetComponent<PlayerCombat>().ReceiveAttack(GetComponent<Enemy>());
		attackRateTime = 0.0f;
	}

    public void Die()
    {
        Destroy(gameObject);
    }

    public float GetCurrentLife() { return currentLife; }
    public float GetMaxLife() { return maxLife; }
    public float GetAttack() { return attack; }
    public float GetAttackRange() { return attackRange; }
    public float GetAttackRate() { return attackRate; }
}
