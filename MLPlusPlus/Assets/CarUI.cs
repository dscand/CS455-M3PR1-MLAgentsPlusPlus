using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarUI : MonoBehaviour
{
	public Indicator Throttle;
	public Indicator Brake;
	public Indicator1 Steering;
	public IndicatorValue Speed;

	public void SetInputValues(float throttle, float brake, float steering)
	{
		Throttle.SetFill(throttle);
		Brake.SetFill(brake);
		Steering.SetFill(steering);
	}

	public void SetOutputValues(float speed)
	{
		Speed.SetValue(speed);
	}
}
