using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class InfoBoxSize : MonoBehaviour
{
	public RectTransform RectTransform;

	public float aspectRatio;

	[ContextMenu("SetAspectRatio")]
	public void SetAspectRatio()
	{
		aspectRatio = RectTransform.sizeDelta.y / RectTransform.sizeDelta.x;
	}

	public void SetHeig()
	{
		RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, RectTransform.sizeDelta.x * aspectRatio);
	}

	private void Start()
	{
		SetHeig();
	}

	private void Update()
	{
		SetHeig();
	}

}
