using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ActionTask<T> : LeafTask<T>
{
	/// <summary>ActionTask Contructor.</summary>
	public ActionTask()
	{
		//...
	}

	/// <summary>Overload ActionTask Constructor.</summary>
	/// <param name="_action">The State return function that belongs to this ActionTask.</param>
	/// <param name="_parameters">Condition Parameters [from 1 to n].</param>
	public ActionTask(ParametersTask _action, params T[]  _parameters)
	{
		leafType = LeafTypes.MultipleParameters;
		parametersTask = _action;
		parameters = _parameters;
	}

	/// <summary>Overload ActionTask Constructor.</summary>
	/// <param name="_action">The State return function that belongs to this ActionTask.</param>
	/// <param name="_parameter">Condition Parameters.</param>
	public ActionTask(ParameterTask _action, T  _parameter)
	{
		leafType = LeafTypes.SingleParameter;
		parameterTask = _action;
		parameter = _parameter;
	}

	/// <summary>Overload ActionTask Constructor.</summary>
	/// <param name="_action">The State return funtion that belongs o this ActionTask.</summary>
	public ActionTask(Func<States> _action)
	{
		leafType = LeafTypes.NoParameter;
		task = _action;
	}

	/// <summary>ActionTask Destructor.</summary>
	~ActionTask()
	{
		//...
	}

	/// <summary>Executes action.</summary>
	/// <returns>State of the action.</returns>
	public override States Tick()
	{
		switch(leafType)
		{
			case LeafTypes.NoParameter:
			return task();
			break;

			case LeafTypes.SingleParameter:
			return parameterTask(parameter);
			break;

			case LeafTypes.MultipleParameters:
			return parametersTask(parameters);
			break;
		}
		
		return state = States.Failure; 
	}
}
