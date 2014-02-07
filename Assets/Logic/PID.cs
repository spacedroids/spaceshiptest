using UnityEngine;
using System.Collections;

/// <summary>
/// A very simple PID controller component class.
/// </summary>
public class PID : MonoBehaviour
{
	public string myName;

	public float multiplier = 1;
	
	public float Kp = 1;
	public float Ki = 0;
	public float Kd = 0.1f;
	
	public float SliderKpMin = 0;
	public float SliderKpMax = 50;
	
	public float SliderKiMin = 0;
	public float SliderKiMax = 100;
	
	public float SliderKdMin = 0;
	public float SliderKdMax = 1;
	
	public bool debug = false;
	public float SliderPanelX = 0;
	public float SliderPanelY = 0;
	
	private float P, I, D;
	private float prevError;
	private float currError = 0; //for debug
	private float result = 0;
	
	void OnGUI()
	{
		if (debug == false)
			return;
		
		float dx = SliderPanelX;
		float dy = SliderPanelY;
		
		GUI.Box(new Rect(25 + dx, 5 + dy, 200, 40), "");

		int col = 25;
		Kp = GUI.HorizontalSlider(new Rect(col + dx, 5  + dy, 200, 10), Kp, SliderKpMax, SliderKpMin);
		Ki = GUI.HorizontalSlider(new Rect(col + dx, 20 + dy, 200, 10), Ki, SliderKiMax, SliderKiMin);
		Kd = GUI.HorizontalSlider(new Rect(col + dx, 35 + dy, 200, 10), Kd, SliderKdMax, SliderKdMin);
		
		GUIStyle style1 = new GUIStyle();
		style1.alignment = TextAnchor.MiddleRight;
		style1.fontStyle = FontStyle.Bold;
		style1.normal.textColor = Color.yellow;
		style1.fontSize = 9;

		col = 0;
		GUI.Label(new Rect(col + dx, 5  + dy, 20, 10), "Kp", style1);
		GUI.Label(new Rect(col + dx, 20 + dy, 20, 10), "Ki", style1);
		GUI.Label(new Rect(col + dx, 35 + dy, 20, 10), "Kd", style1);

		GUIStyle style2 = style1;

		col = 235;
		GUI.TextField(new Rect(col + dx, 5  + dy, 60, 10), Kp.ToString(), style2);
		GUI.TextField(new Rect(col + dx, 20 + dy, 60, 10), Ki.ToString(), style2);
		GUI.TextField(new Rect(col + dx, 35 + dy, 60, 10), Kd.ToString(), style2);
		
		GUI.Label(new Rect(0 + dx, -8 + dy, 200, 10), myName, style2);

		GUIStyle style3 = style2;
		style3.alignment = TextAnchor.MiddleLeft;

		col = 300;
		GUI.TextField(new Rect(col + dx, 5  + dy, 60, 10), "Curr: " + currError, style3);
		GUI.TextField(new Rect(col + dx, 20 + dy, 60, 10), "Prev: " + prevError, style3);
		GUI.TextField(new Rect(col + dx, 35 + dy, 60, 10), "Push: " + result, style3);

	}
	
	public float GetOutput(float currentError)
	{
		currError = currentError; //debug
		float deltaTime = Time.deltaTime;
		P = currentError;
		I += P * deltaTime;
		D = (P - prevError) / deltaTime;
		prevError = currentError;

		result = multiplier * (P * Kp + I * Ki + D * Kd);
		return result;
	}
}
