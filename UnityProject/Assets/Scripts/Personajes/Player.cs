using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	public float speed = 8.0f, boostFading = 0.95f,
				 rotSpeed = 5.0f,
				 jumpForce = 0.2f;
	public float boostForce = 2.0f;

	public bool selected = false;
	public float attack = 5.0f;
	
	public Vector3 movement = Vector3.zero;

	public Vector3 boost = Vector3.zero;
	private int jumpsDone = 0;
	private float aggro = 0.0f;
	private CharacterController controller;

	public GameObject target;

	void Start()
	{
		controller = GetComponent<CharacterController>();
	}

	void FixedUpdate()
	{
		//En fixed update ya que son aceleraciones
		boost *= boostFading;
		movement.y += Core.gravity; //gravity
		movement.y = Mathf.Max(movement.y, -2000.0f * Time.deltaTime); //evitar caida brusca al saltar
		//
	}

	void Update()
	{
		if(selected)
		{
			SelectedMove();
			if(Input.GetKeyDown(KeyCode.Space) && jumpsDone < 2) Jump();
		}
		else FollowSelected(); //SIGUEN AL PERSONAJE SELECCIONADO

		controller.Move((movement + boost * speed) * Time.deltaTime);
		if (IsGrounded()) jumpsDone = 0;
		//
	}

	private void SelectedMove()
	{
		float axisX = Input.GetAxis ("Horizontal"), axisY = Input.GetAxis ("Vertical");

		float movementY = movement.y; //Lo reestablecemos al final para que no quede afectado por el movimiento en x, z;
		movement = Vector3.zero;

		Vector3 dir = ((Camera.main.transform.forward * axisY) + (Camera.main.transform.right * axisX)).normalized;
		movement += dir * speed;
		
		movement.y = 0;
		if (movement.magnitude > 0.5f) 
		{
			Quaternion newRot = Quaternion.LookRotation(movement);
			transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * speed);
		}

		movement.y = movementY; //Reestablecido
	}

	private void FollowSelected()
	{
		if(Core.selectedPlayer == null) return;

		float movementY = movement.y; //Lo reestablecemos al final para que no quede afectado por el movimiento en x, z;
		movement = Vector3.zero;

		Vector3 selectedPlayerPos = Core.selectedPlayer.gameObject.transform.position;
		float distanceToSelected = Vector3.Distance(transform.position, selectedPlayerPos);

		if(distanceToSelected > Core.playerToPlayerFollowDistance)
		{
			//Ve hacia al jugador
			Vector3 dir = (selectedPlayerPos - transform.position); 
			dir.y = 0; dir.Normalize();

			movement += dir * distanceToSelected;
			if(movement.magnitude > speed) movement = dir * speed;
			movement.y = 0;
			//

			//Separamos a los seguidores
			Vector3 otherFollowerPos = Core.GetOtherFollowerPlayer(GetComponent<Player>()).gameObject.transform.position; 
			float dist = Vector3.Distance(transform.position, otherFollowerPos); 

			//Repulsion entre los seguidores
			Vector3 repulsionDir = transform.position - otherFollowerPos;
			repulsionDir.y = 0; repulsionDir.Normalize();
			movement += repulsionDir.normalized * speed * 1.0f/dist;

			//Modificamos hacia donde miran
			if(movement == Vector3.zero) //Si estan quietos, que miren donde mira el pj seleccionado
			{
				Vector3 selectedForward = Core.selectedPlayer.gameObject.transform.forward;
				transform.forward = Vector3.Lerp(transform.forward, selectedForward, Time.deltaTime * rotSpeed);
			}
			else transform.forward = Vector3.Lerp(transform.forward, movement, Time.deltaTime * rotSpeed);
		}

		movement.y = movementY; //Reestablecido
	}
	
	private void Shoot()
	{
		Vector3 origin = transform.position + Vector3.up * 1.0f;

		GameObject proj = Instantiate(Resources.Load("Projectile"), 
		                              origin, Quaternion.identity) as GameObject;

		Vector3 shootDir = (CameraControl.GetLookPoint() - origin).normalized;
		if(target != null) 
		{
			shootDir = (target.transform.position - origin).normalized;
		}
		
		proj.GetComponent<Projectile>().dir = shootDir;
		proj.transform.forward = shootDir; 
		proj.GetComponent<Projectile>().shooterPlayer = this;
	}

	//COMBO RELATED STUFF /////////////////////////
	public void OnClickComboDown(KeyCode heldControlKey, int clicksSeguidos, int pressedButton) // 0 = left, 1 = right
	{
		if(heldControlKey == KeyCode.None)
		{
			Shoot();
		}
		else if(heldControlKey == KeyCode.LeftControl)
		{
			GetComponent<PlayerAnimation>().Play("Combo1");
		}
		else if(heldControlKey == KeyCode.LeftShift)
		{
			GetComponent<PlayerAnimation>().Play("ReceiveDamage");
		}
	}

	public void OnClickCombo(KeyCode heldControlKey, int clicksSeguidos, int pressedButton) //Mantener pulsado
	{

	}

	public void OnKeyComboDone(string comboName)
	{
		Debug.Log (comboName);
		if(comboName == "forwardBoost") Boost(Camera.main.transform.forward);
		else if(comboName == "rightBoost") Boost(Camera.main.transform.right);
		else if(comboName == "leftBoost") Boost(-Camera.main.transform.right);
		else if(comboName == "backBoost") Boost(-Camera.main.transform.forward);
	}

	public void OnKeyComboKeyDown(string comboName, KeyCode key)
	{

	}
	///////////////////////////////


	public void OnApplyDamage()
	{
		aggro += attack;
	}

	private void Jump()
	{
		++jumpsDone;
		movement.y = jumpForce;
	}

	private void Boost(Vector3 dir)
	{
		if(boost.magnitude > 0.2f) return; //Aun no ha acabado el boost anterior
		boost = dir * boostForce;
	}

	public float GetAggro()
	{
		return aggro;
	}

	private bool IsGrounded()
	{
		RaycastHit hit;
		return Physics.Raycast( controller.transform.position, Vector3.down, out hit, 0.3f, ~(1 << LayerMask.NameToLayer ("Player")) );
	}

	public bool IsJumping()
	{
		return jumpsDone > 0;
	}
}
