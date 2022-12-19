using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ConditionTask<T> : LeafTask<T>
{
	/// <summary>ConditionTask Constructor.</summry>
	public ConditionTask()
	{
		//...
	}

	/// <summary>Overload ConditionTask Constructor.</summary>
	/// <param name="_condition">The State return function that belongs to this ConnditionTask.</param>
	/// <param name="_parameters">Condition Parameters [from 1 to n].</param>
	public ConditionTask(ParametersTask _condition, params T[]  _parameters)
	{
		leafType = LeafTypes.MultipleParameters;
		parametersTask = _condition;
		parameters = _parameters;
	}

	/// <summary>Overload ConditionTask Constructor.</summary>
	/// <param name="_condition">The State return function that belongs to this ConnditionTask.</param>
	/// <param name="_parameter">Condition Parameter.</param>
	public ConditionTask(ParameterTask _condition, T  _parameter)
	{
		leafType = LeafTypes.SingleParameter;
		parameterTask = _condition;
		parameter = _parameter;
	}

	/// <summary>Overload ConditionTask Constructor.</summary>
	/// <param name="_condition">The State return function that belongs to this ConnditionTask.</param>
	public ConditionTask(Func<States> _condition)
	{
		leafType = LeafTypes.NoParameter;
		task = _condition;
	}

	/// <summary>ConditionTask Destructor.</summary>
	~ConditionTask()
	{
		//...
	}

	/// <summary>Checks each statusCondition.</summary>
	/// <returns>State of the statusCondition that was approved.</returns>
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
