using UnityEngine;
using System.Collections;

public class PlayerLifebar : MonoBehaviour 
{
	public Player player;
	
	private Vector3 originalLifeBarFillScale;
	private RectTransform rTransform;

	void Start() 
	{
		rTransform = GetComponent<RectTransform>();
		originalLifeBarFillScale = rTransform.localScale;
	}
	
	void LateUpdate()
	{
		if(player == null) return;
		if(!GameState.IsPlaying()) return;

		ScaleLifeBar();
	}
	
	void ScaleLifeBar()
	{
		float percent = Mathf.Max(0.0f, (player.GetCurrentLife() / player.GetMaxLife())); 
		Vector3 s = new Vector3(originalLifeBarFillScale.x * percent, 
		                        originalLifeBarFillScale.y, 
		                        originalLifeBarFillScale.z);
		rTransform.localScale = s;
	}
}
