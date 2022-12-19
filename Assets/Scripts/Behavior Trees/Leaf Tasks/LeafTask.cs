using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class LeafTask<T> : TreeNode
{
	/// <summary>Condition delegate Type with multyple parameters.</summary>
	/// <param name="_parameters">Parameters that the delegate will take.</summary>
	public delegate States ParametersTask(params T[] _parameters);

	/// <summary>Condition delegate Type with single parameter.</summary>
	/// <param name="_parameter">Parameter that the delegate will take.</summary>
	public delegate States ParameterTask(T _parameter);

	private Func<States> _task; 				/// <summary>No parameter Function of States return type.</summary>
	private ParametersTask _parametersTask; 	/// <summary>Function of States return type.</summary>
	private ParameterTask _parameterTask; 		/// <summary>Single Parameter Funcion odf States return type.</summary>
	private MonoBehaviour _monoBehaviour; 		/// <summary>MonoBehaviour Class container of parameters.</summary>
	private T[] _parameters; 					/// <summary>LeafTask's parameters [optional].</summary>
	private T _parameter; 						/// <summary>LeafTask's parameter [optional].</summary>
	private LeafTypes _leafType; 				/// <summary>Leaf Type.</summary>

	/// <summary>Leaf Types Enumerator.</summary>
	public enum LeafTypes
	{
		NoParameter, 							/// <summary>No Parameter Leaf Type.</summary>
		SingleParameter, 						/// <summary>Single Parameter Leaf Type.</summary>
		MultipleParameters 						/// <summary>Multiple Parameters Leaf Type.</summary>
	}

#region Getters/Setters:
	/// <summary>Gets and Sets task property.</summary>
	public Func<States> task
	{
		get { return _task; }
		set
		{
			_task = value;
			_parametersTask = null;
			_parameterTask = null;
		}
	}

	/// <summary>Gets and Sets parametersTask property.</summary>
	public ParametersTask parametersTask
	{
		get { return _parametersTask; }
		set
		{
			_parametersTask = value;
			_parameterTask = null;
			_task = null;
		}
	}

	/// <summary>Gets and Sets parameterTask property.</summary>
	public ParameterTask parameterTask
	{
		get { return _parameterTask; }
		set
		{
			_parameterTask = value;
			_parametersTask = null;
			_task = null;
		}
	}

	/// <summary>Gets and Sets monoBehaviour property.</summary>
	public MonoBehaviour monoBehaviour
	{
		get { return _monoBehaviour; }
		set { _monoBehaviour = value; }
	}

	/// <summary>Gets and Sets parameters property.</summary>
	public T[] parameters
	{
		get { return _parameters; }
		set { _parameters = value; }
	}

	/// <summary>Gets and Sets parameter property.</summary>
	public T parameter
	{
		get { return _parameter; }
		set { _parameter = value; }
	}

	/// <summary>Gets and Sets leafType property.</summary>
	public LeafTypes leafType
	{
		get { return _leafType; }
		set { _leafType = value; }
	}

	/// <summary>Sets parameters to newParameters.</summary>
	/// <param name="_newParameters">New Parameters.</param>
	public void SetParameters(params T[] _newParameters)
	{
		parameters = _newParameters;
	}
#endregion

	/// <summary>LeafTask Constructor.</summary>
	public LeafTask()
	{
		//...
	}

	/// <summary>LeafTask Destructor.</summary>
	~LeafTask()
	{
		//...
	}

	/// <summary>Runs Tree Node.</summary>
	/// <returns>State of the Tick.</returns>
	public override States Tick()
	{
		return state;
	}
}
