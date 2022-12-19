using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoidlessUtilities
{
public interface IPoolObject
{
	/// <summary>Independent Actions made when this Pool Object is being created.</summary>
	void OnObjectCreation();

	/// <summary>Actions made when this Pool Object is being activated.</summary>
	void OnObjectActivation();

	/// <summary>Actions made when this Pool Object is being reseted.</summary>
	void OnObjectReset();

	/// <summary>Actions made when this Pool Object is being destroyed.</summary>
	void OnObjectDestruction();
}
}