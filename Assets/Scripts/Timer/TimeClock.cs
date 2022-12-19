using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeClock
{
	private const float HOUR = 3600f; 						/// <summary>Hour value.</summary>
	private const float HOUR_NORMAL = (1f / HOUR); 			/// <summary>Hour Normal value.</summary>
	private const float MINUTE = 60f; 						/// <summary>Minute value.</summary>
	private const float MINUTE_NORMAL = (1f / MINUTE); 		/// <summary>Minute Normal value.</summary>
	private const float SECOND = 1f; 						/// <summary>Second value.</summary>
	private const float SECOND_NORMAL = (1f / SECOND); 		/// <summary>Second Normal value.</summary>
	private const float DECISECOND = 0.1f; 					/// <summary>Decisecond value.</summary>
	private const float CENTISECOND = 0.01f; 				/// <summary>Centisecond value.</summary>
	private const float MILISECOND = 0.001f; 				/// <summary>Milisecond value.</summary>

	public delegate void OnTimeEnds(); 						/// <summary>Delegate that is called when this TimeClock's time ends.</summary>
	public event OnTimeEnds onTimeEnds; 					/// <summary>OnTimeEnds subscription tag.</summary>

	private int _hours; 									/// <summary>TimeClock's Hours property.</summary>
	private int _minutes; 									/// <summary>TimeClock's Minutes property.</summary>
	private int _seconds; 									/// <summary>TimeClock's Seconds property.</summary>
	private int _deciseconds; 								/// <summary>TimeClock's Deciseconds property.</summary>
	private int _centiseconds; 								/// <summary>TimeClock's Centiseconds property.</summary>
	private int _miliseconds; 								/// <summary>TimeClock's Miliseconds property.</summary>
	private int _leftHours; 								/// <summary>TimeClock's Hours Left.</summary>
	private int _leftMinutes; 								/// <summary>TimeClock's Minutes Left.</summary>
	private int _leftSeconds; 								/// <summary>TimeClock's Seconds Left.</summary>
	private int _leftDeciSeconds; 							/// <summary>TimeClock's DeciSeconds Left.</summary>
	private int _leftCentiseconds; 							/// <summary>TimeClock's Centiseconds Left.</summary>
	private int _leftMiliseconds; 							/// <summary>TimeClock's Miliseconds Left.</summary>
	private float _initialTime; 							/// <summary>TimeClock's Initial Time.</summary>
	private float _leftTime; 								/// <summary>TimeClock's Time Left.</summary>
	private bool _running; 									/// <summary>Is the TimeClock Ticking? Flag. Enabled when the time reaches 0.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets hours property.</summary>
	public int hours
	{
		get { return _hours; }
		set { _hours = value; }
	}

	/// <summary>Gets and Sets minutes property.</summary>
	public int minutes
	{
		get { return _minutes; }
		set { _minutes = value; }
	}

	/// <summary>Gets and Sets seconds property.</summary>
	public int seconds
	{
		get { return _seconds; }
		set { _seconds = value; }
	}

	/// <summary>Gets and Sets deciseconds property.</summary>
	public int deciseconds
	{
		get { return _deciseconds; }
		set { _deciseconds = value; }
	}

	/// <summary>Gets and Sets centiseconds property.</summary>
	public int centiseconds
	{
		get { return _centiseconds; }
		set { _centiseconds = value; }
	}

	/// <summary>Gets and Sets miliseconds property.</summary>
	public int miliseconds
	{
		get { return _miliseconds; }
		set { _miliseconds = value; }
	}

	/// <summary>Gets leftHours property.</summary>
	public int leftHours
	{
		get { return _leftHours = Mathf.FloorToInt(leftTime * HOUR_NORMAL); }
	}

	/// <summary>Gets leftMinutes property.</summary>
	public int leftMinutes
	{
		get { return _leftMinutes = Mathf.FloorToInt(leftTime * MINUTE_NORMAL); }
	}

	/// <summary>Gets leftSeconds property.</summary>
	public int leftSeconds
	{
		get { return _leftSeconds = Mathf.FloorToInt(leftTime % MINUTE); }
	}

	/// <summary>Gets and Sets initialTime property.</summary>
	public float initialTime
	{
		get { return _initialTime; }
		private set { _initialTime = value; }
	}

	/// <summary>Gets and Sets leftTime property.</summary>
	public float leftTime
	{
		get { return _leftTime; }
		private set { _leftTime = value; }
	}

	/// <summary>Gets and Sets running property.</summary>
	public bool running
	{
		get { return _running; }
		private set { _running = value; }
	}
#endregion

	/// <summary>Class Constructor.</summary>
	/// <param name="_hours">TimeClock's hours.</summary>
	/// <param name="_minutes">TimeClock's minutes.</summary>
	/// <param name="_seconds">TimeClock's seconds.</summary>
	/// <param name="_deciseconds">TimeClock's deciseconds.</summary>
	/// <param name="_centiseconds">TimeClock's centiseconds.</summary>
	/// <param name="_miliseconds">TimeClock's miliseconds.</summary>
	public TimeClock(int _hours, int _minutes, int _seconds, int _deciseconds, int _centiseconds, int _miliseconds)
	{
		running = true;

		hours = _hours;
		minutes = _minutes;
		seconds = _seconds;
		deciseconds = _deciseconds;
		centiseconds = _centiseconds;
		miliseconds = _miliseconds;

		leftTime = initialTime = CalculateInitialTime();
	}

	/// <summary>Class Constructor.</summary>
	/// <param name="_minutes">TimeClock's minutes.</summary>
	/// <param name="_seconds">TimeClock's seconds.</summary>
	public TimeClock(int _minutes, int _seconds)
	{
		running = true;

		hours = 0;
		minutes = _minutes;
		seconds = _seconds;
		deciseconds = 0;
		centiseconds = 0;
		miliseconds = 0;
		
		leftTime = initialTime = CalculateInitialTime();
	}

	/// <summary>Calculates the initial time by the sumatory of all the TimeClock attributes.</summary>
	/// <returns>Calculated Initial Time.</returns>
	public float CalculateInitialTime()
	{
		return ((hours * HOUR) + (minutes * MINUTE) + (seconds * SECOND) + (deciseconds * DECISECOND) + (centiseconds * CENTISECOND) + (miliseconds * MILISECOND));
	}

	/// <summary>Ticks TimeClock's Time.</summary>
	public void Tick()
	{
		leftTime -= Time.deltaTime;

		if(leftTime <= 0f)
		{
			leftTime = 0.0f;
			running = false;
			if(onTimeEnds != null) onTimeEnds();
		}
	}

	/// <summary>[Overload Method] Ticks TimeClock's Time by Value.</summary>
	/// <param name="_time">Time Value that will be subtracted to leftTime.</summary>
	public void Tick(float _time)
	{
		leftTime -= _time;

		if(leftTime <= 0f)
		{
			leftTime = 0.0f;
			running = false;
			if(onTimeEnds != null) onTimeEnds();
		}
	}

	/// <summary>Stops TimeClock's Ticking.</summary>
	public void Stop()
	{

	}

	/// <summary>Resets TimeClock to it's default stats.</summary>
	public void Reset()
	{
		running = true;
		leftTime = initialTime = CalculateInitialTime();
	}
}
