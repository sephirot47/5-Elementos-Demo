using UnityEngine;
using System.Collections;

public class EnemyLifeBar : MonoBehaviour 
{
	public Enemy enemy;

	public float disappearTime = 5.0f;
	private float time = float.PositiveInfinity;

	private Vector3 originalLifeBarFillScale;
	private Transform lifeBarFill;

	void Start () 
	{
		if(!GameState.IsPlaying()) return;

		SetVisible(false);

		foreach(Transform t in transform)
		{
			if(t.gameObject.name == "Fill")
			{
				lifeBarFill = t;
				break;
			}
		}

		originalLifeBarFillScale = lifeBarFill.localScale;
	}

	void LateUpdate()
	{
		if(!GameState.IsPlaying() || GameState.AllPlayersDead()) return;

		time += Time.deltaTime;

		ScaleLifeBar();
		LookAtCamera();

		if(time >= disappearTime) SetVisible(false);
		else SetVisible(true);
	}

	private void ScaleLifeBar()
	{
		float percent = Mathf.Max(0.0f, (enemy.GetCurrentLife() / enemy.GetMaxLife())); 
		Vector3 s = new Vector3(originalLifeBarFillScale.x * percent,
		                        originalLifeBarFillScale.y,
		                        originalLifeBarFillScale.z);
		
		if(lifeBarFill.localScale.x != s.x) time = 0.0f;
		
		lifeBarFill.localScale = s;
	}

	private void LookAtCamera()
	{
		transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position);
	}

	private void SetVisible(bool visible)
	{
		foreach(MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
			mr.enabled = visible;
	}
}
