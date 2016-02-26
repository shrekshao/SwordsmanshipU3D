using UnityEngine;
using System.Collections;

public class ControlGUIScript : MonoBehaviour {

	public float Value = 0.5f;
	public float Fade = 0.01f;

	public GUIBarScript GBS;

	public Vector2 Offset;

	public Vector2 LabelOffSet;

	public string playText = "Play";
	public bool IsPlaying = false;

	void Start()
	{
		GBS = GetComponent<GUIBarScript>();
	}

	void OnGUI() 
	{
		if (GBS == null)
		{
			return;
		}

		if (IsPlaying != true)
		{
			GUIStyle LabelStyle =  new GUIStyle();
			LabelStyle.normal.textColor = Color.black;
			LabelStyle.fontSize = 18;
			GUI.Label(new Rect(GBS.Position.x + Offset.x + LabelOffSet.x, GBS.Position.y + Offset.y + LabelOffSet.y - 40, 100, 25),"Value",LabelStyle);
			Value = GUI.HorizontalSlider(new Rect(GBS.Position.x + Offset.x , GBS.Position.y + Offset.y - 40, 180, 25), Value, 0.0F, 1F);
		}

		if (GUI.Button(new Rect(GBS.Position.x + Offset.x + LabelOffSet.x, GBS.Position.y + Offset.y + LabelOffSet.y - 80, 100, 25),playText ))
		{
			if (IsPlaying == true)
			{
				IsPlaying = false;
				playText = "Play";
			}
			else
			{
				IsPlaying = true;
				playText = "Stop";
            }
            
        }

	}

	void Update () 
	{
		if (GBS == null)
		{
			return;
		}

		if (IsPlaying == true)
		{
			GBS.Value = ((Mathf.Sin (Time.time)/2f) + 0.5f) * 1.01f;
        }
		else
		{
			GBS.Value = Value;
		}

	}
}
