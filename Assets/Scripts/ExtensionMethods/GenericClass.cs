using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericClass<T> : MonoBehaviour
{
	private T _type;

	public T type 
	{
		get { return _type; }
		set { _type = value; }
	}
}
