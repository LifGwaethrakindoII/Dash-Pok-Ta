using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverterNode<T> : DecoratorNode<T>
{
	private InvertPolicy[] InvertPolicies; 					/// <summary>Collection ofInvert Policies the Inverter Node has.</summary>

	public class InvertPolicy
	{
		public States conditionState{ get; private set; } 	/// <summary>If is State Policy.</summary>
		public States resultState{ get; private set; } 		/// <summary>Turn to State Policy.</summary>

		/// <summary>InvertPolicy Constructor.</summary>
		/// <param name="_conditionState">InvertPolicy's conditionState.</param>
		/// <param name="_resultState">InvertPolicy's resultState.</param>
		public InvertPolicy(States _conditionState, States _resultState)
		{
			conditionState = _conditionState;
			resultState = _resultState;
		}
	}

	/// <summary>InverterNode Constructor.</summary>
	public InverterNode()
	{
		//...
	}

	/// <summary>Overload InverterNode Constructor.</summary>
	/// <param name="_leafChild">InverterNode's LeafTask child node.</param>
	/// <param name="_invertPolicies">InverterNode's Invert Policies.</param>
	public InverterNode(LeafTask<T> _leafChild, params InvertPolicy[] _invertPolicies)
	{
		leafChild = _leafChild;
		AddNode(leafChild);
		InvertPolicies = _invertPolicies;
	}

	/// <summary>InverterNode Destructor.</summary>
	~InverterNode()
	{
		//...
	}

	/// <summary>Inverts returned leafChild's State.</summary>
	/// <returns>leafChild's inverted State.</summary>
	public override States Tick()
	{
		States leafChildState = leafChild.Tick();

		foreach(InvertPolicy invertPolicy in InvertPolicies)
		{
			if(leafChildState == invertPolicy.conditionState) return state = invertPolicy.resultState;
			else continue;
		}

		return state = leafChildState;
	}
}
