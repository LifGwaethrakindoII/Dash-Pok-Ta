using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoidlessUtilities
{
public static class VoidlessCollections
{

#region Arrays:
	/// <summary>Initializes array of T elements to their default constructors.</summary>
	/// <param name="_array">Array to initialize.</param>
	/// <param name="_length">Length of the array.</param>
	/// <returns>Initialized Array of T elements.</returns>
	public static T[] InitializeArray<T>(this T[] _array, int _length) where T : new()
	{
	    T[] array = new T[_length];

	    for (int i = 0; i < _length; ++i)
	    {
	        array[i] = new T();
	    }

	    return array;
	}
#endregion

#region Lists:
	/// <summary>Gets list of T Components from Object List.</summary>
	/// <param name="_list">The Object List.</param>
	/// <returns>List of Object List's T Component.</returns>
	public static List<T> GetComponentsFromGameObjects<T>(this List<GameObject> _list) where T : UnityEngine.Object
	{
		List<T> newList =  new List<T>();

		foreach(GameObject _unityObject in _list)
		{
			if(_unityObject.Has<T>()) newList.Add(_unityObject.GetComponent<T>());
		}

		return newList;
	}

	/// <summary>Destroys all GameObjects contained on a list of UnityEngine's Components.</summary>
	/// <param name="_list">The List containing the GameObjects.</param>
	public static void DestroyAllGameObjects<T>(this List<T> _list) where T : UnityEngine.Component
	{
		foreach(T _component in _list)
		{
			if(Application.isPlaying) UnityEngine.Object.Destroy(_component.gameObject);
			else if(Application.isEditor) UnityEngine.Object.DestroyImmediate(_component.gameObject);
		}
	}

	/// <summary>Destroys all Object elements on List.</summary>
	/// <param name="_list">List containing the Object elements.</param>
	public static void DestroyAllElements<T>(this List<T> _list) where T : UnityEngine.Object
	{
		foreach(T UnityObject in _list)
		{
			if(Application.isPlaying) UnityEngine.Object.Destroy(UnityObject);
			else if(Application.isEditor) UnityEngine.Object.DestroyImmediate(UnityObject);
		}
	}

	/// <summary>Adds List Elements to List.</summary>
	/// <param name="_list">The List that will have its elements added.</param>
	/// <param name="_elementsList">The List that contains the new Elements that will be added.</param>
	/// <returns>List with newly addedElements.</returns>
	public static List<T> AddElements<T>(this List<T> _list, List<T> _elementsList)
	{
		foreach(T element in _elementsList)
		{
			_list.Add(element);
		}

		return _list;
	}

	/// <summary>Finds index that accomplishes given predicate.</summary>
	/// <param name="_list">List that is going to look for the successful index.</param>
	/// <param name="_condition">Condition that will be evaluated on each element to determine the successful index.</param>
	/// <returns>Successful index that accomplishes the evaluated condition.</returns>
	public static int GetIndexThatAccomplishes<T>(this List<T> _list, System.Predicate<T> _condition)
	{
		for(int i = 0; i < _list.Count; i++)
		{
			if(_condition(_list[i])) return i;	
		}

		return 0;
	}

	/// <summary>Gets if list has something on all indexes.</summary>
	/// <param name="_list">List containing the T elements.</param>
	/// <returns>True if all elements on list are different from null.</returns>
	public static bool ListFull<T>(this List<T> _list)
	{
		foreach(T element in _list)
		{
			if(element == null) return false;
		}

	return true;
	}

	/// <summary>Checks if all elements on list accomplishes all conditions.</summary>
	/// <param name="_list">List that will have all its elements evaluated.</param>
	/// <param name="_condition">Condition that all elements have to accomplish for the method to return true.</param>
	/// <returns>If all elements on list accomplish the condition.</returns>
	public static bool AllAccomplish<T>(this List<T> _list, System.Predicate<T> _condition)
	{
		foreach(T element in _list)
		{
			if(!_condition(element)) return false;
		}

		return true;
	}

	/// <summary>Gets a list with the elements that accomplish a certain condition.</summary>
	/// <param name="_list">List that willhave its elements evaluated.</param>
	/// <param name="_condition">Condition that each element of the List has to accomplish.</param>
	/// <returns>List with all the elemets that accomplished the condition.</returns>
	public static List<T> GetAllThatAccomplish<T>(this List<T> _list, System.Predicate<T> _condition)
	{
		List<T> accomplishedList = new List<T>();

		foreach(T element in _list)
		{
			if(_condition(element)) accomplishedList.Add(element);
		}

		return accomplishedList;
	}

	/// <summary>[EXPERIMENAL] Generic sorting [Algorithm by Lonk].</summary>
	/// <param name="_list">List that will be sorted.</param>
	/// <param name="_ignoreIfNull">Proceed to switching even if the next element is Null?.</param>
	/// <returns>Sorted List.</returns>
	public static List<T> LonkSort<T>(this List<T> _list, bool _ignoreIfNull)
	{
		T displacedElement = default(T);
		T tempElement = default(T);

		for(int i = 0; i < _list.Count; i++)
		{
			if(i < (_list.Count - 1))
			{ /// If there is a next element.
				if(_list[i + 1] != null)
				{ /// If the next element is different than null.
					tempElement = _list[i];
					_list[i] = displacedElement;
					displacedElement = tempElement;
				}
				else if(!_ignoreIfNull) break;
			}
			else
			{ /// Else, this iterator is the last element.
				tempElement = _list[i];
				_list[i] = displacedElement;
				_list[0] = tempElement;
			}
		}

		return _list;
	}

	/// <summary>[EXPERIMENAL] Generic sorting [Algorithm by Lonk].</summary>
	/// <param name="_array">T that will be sorted.</param>
	/// <param name="_ignoreIfNull">Proceed to switching even if the next element is Null?.</param>
	/// <returns>Sorted T.</returns>
	public static T[] LonkSort<T>(this T[] _array, bool _ignoreIfNull)
	{
		T displacedElement = default(T);
		T tempElement = default(T);

		for(int i = 0; i < _array.Length; i++)
		{
			if(i < (_array.Length - 1))
			{ /// If there is a next element.
				if(_array[i + 1] != null)
				{ /// If the next element is different than null.
					tempElement = _array[i];
					_array[i] = displacedElement;
					displacedElement = tempElement;
				}
				else if(!_ignoreIfNull) break;
			}
			else
			{ /// Else, this iterator is the last element.
				tempElement = _array[i];
				_array[i] = displacedElement;
				_array[0] = tempElement;
			}
		}

		return _array;
	}
#endregion

#region Dictionaries:
	/// <summary>Initializes Dictionary from Collection of DictionarySerializer of the same KeyValuePair.</summary>
	/// <param name="_dictionary">Dictionary to initialize.</param>
	/// <param name="_dictionarySerializers">Collection of DictionarySerializer of the same KeyValuePair as the DictionarySerializer.</param>
	/// <param name="onInitializationEnds">[Optional] Action to invoke after the Initialization ends.</param>
	/// <returns>Initialized Dictionary.</returns>
	public static Dictionary<T, U> InitializeFrom<T, U>(this Dictionary<T, U> _dictionary, List<DictionarySerializer<T, U>> _dictionarySerializers, System.Action onInitializationEnds)
	{
		_dictionary = new Dictionary<T, U>();

		foreach(DictionarySerializer<T, U> item in _dictionarySerializers)
		{
			_dictionary.Add(item.key, item.value);
		}

		if(onInitializationEnds != null) onInitializationEnds();

		return _dictionary;
	}
#endregion

}
}