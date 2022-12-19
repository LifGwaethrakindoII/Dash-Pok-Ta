using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class MyExtensionMethods
{
	/// <summary>Gets clamped value (int).</summary>
	/// <param name="_value">Value that will be clamped.</param>
	/// <param name="_min">Minimum value clamped.</param>
	/// <param name="_max">Maximum value clamped.</param>
	/// <returns>New Value clamped (as int).</returns>
	public static int ClampValue(this int _value, int _min, int _max)
	{
		return _value = Mathf.Clamp(_value, _min, _max);
	}

	/// <summary>Gets clamped value (float).</summary>
	/// <param name="_value">Value that will be clamped.</param>
	/// <param name="_min">Minimum value clamped.</param>
	/// <param name="_max">Maximum value clamped.</param>
	/// <returns>New Value clamped (as float).</returns>
	public static float ClampValue(this float _value, float _min, float _max)
	{
		return _value = Mathf.Clamp(_value, _min, _max);
	}

	/// <summary>Destroys all GameObjects contained on the list.</summary>
	/// <param name="_list">The List containing the GameObjects.</param>
	public static void DestroyAllObjectsFromTransforms(this List<Transform> _list)
	{
		foreach(Transform _transform in _list)
		{
			if(Application.isPlaying) UnityEngine.Object.Destroy(_transform.gameObject);
			else if(Application.isEditor) UnityEngine.Object.DestroyImmediate(_transform.gameObject);
		}
	}

	/// <summary>Destroys all Object elements on List.</summary>
	/// <param name="_list">List containing the Object elements.</param>
	public static void DestroyAllElements<T>(this List<T> _list) where T : Object
	{
		foreach(T UnityObject in _list)
		{
			if(Application.isPlaying) Object.Destroy(UnityObject);
			else if(Application.isEditor) Object.DestroyImmediate(UnityObject);
		}
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

	/// <summary>Gets the position (Vector3) from a list of Transforms.</summary>
	/// <param name="_list">The list of Transforms from where the Vector3 list will be created.</param>
	/// <returns>List of the Transform positions (Vector3).</returns>
	public static List<Vector3> GetTransformListPositions(this List<Transform> _list)
	{
		List<Vector3> newList = new List<Vector3>();

		foreach(Transform trans in _list)
		{
			newList.Add(trans.position);
		}

		return newList;
	}

	/// <summary>Gets the rotations (Quaternion) from a list of Transforms.</summary>
	/// <param name="_list">The list of Transforms from where the Quaternion list will be created.</param>
	/// <returns>List of the Transform rotation (Quaternion).</returns>
	public static List<Quaternion> GetTransformListRotations(this List<Transform> _list)
	{
		List <Quaternion> newList = new List<Quaternion>();

		foreach(Transform _transform in _list)
		{
			newList.Add(_transform.rotation);
		}

		return newList;
	}

	/// <summary>Gets the position (Vector3) from a list of GameObjects.</summary>
	/// <param name="_list">The list of GameObjects from where the Vector3 list will be created.</param>
	/// <returns>List of the GameObject positions (Vector3).</returns>
	public static List<Vector3> GetGameObjectListPositions(this List<GameObject> _list)
	{
		List<Vector3> newList = new List<Vector3>();

		foreach(GameObject obj in _list)
		{
			if(obj != null) newList.Add(obj.transform.position);
		}

		return newList;
	}

	/// <summary>Gets the distances (float) between List of GameObjects and GameObject.</summary>
	/// <param name="_list">The list of GameObjects from where the distance will be measured.</param>
	/// <param name="_object">The GameObject from where the List of GameObjects will measure the distance.</param>
	/// <returns>List of the distances (float).</returns>
	public static List<float> GetDistancesFromObject(this List<GameObject> _list, GameObject _object)
	{
		List<float> newList = new List<float>();

		foreach(GameObject obj in _list)
		{
			newList.Add(Vector3.Distance(obj.transform.position, _object.transform.position));
		}

		return newList;
	}

	/// <summary>Gets the distances (float) between List of Vector3 and Vector3.</summary>
	/// <param name="_list">The list of Vector3 from where the distance will be measured.</param>
	/// <param name="_targetPoint">The Vector3 from where the List of Vector3 will measure the distance.</param>
	/// <returns>List of the distances (float).</returns>
	public static List<float> GetDistancesFromVectors(this List<Vector3> _list, Vector3 _targetPoint)
	{
		List<float> newList = new List<float>();

		foreach(Vector3 point in _list)
		{
			newList.Add(Vector3.Distance(point, _targetPoint));
		}

		return newList;
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

	/*/// <summary>Gets the distances (float) between List of GameObjects and GameObject.</summary>
	/// <param name="_list">The list of T Objects from where the distance will be measured.</param>
	/// <param name="_targetPoint">The point from where the List of GameObjects will measure the distance.</param>
	/// <returns>List of the distances (float).</returns>
	public static List<float> GetDistancesFromElements<T>(this List<T> _list, Vector3 _targetPoint) where T : Object
	{
		List<float> newList = new List<float>();
		T type;

		switch(type)
		{
			case typeof(gameObject):
			foreach(GameObject obj in _list)
			{
				newList.Add(Vector3.Distance(obj.transform.position, _targetPoint));
			}
			break;

			case typeof(Transform):
			foreach(Transform _transform in _list)
			{
				newList.Add(Vector3.Distance(_transform.position, _targetPoint));
			}
			break;

			case typeof(Vector3):
			foreach(Vector3 point in _list)
			{
				newList.Add(Vector3.Distance(point, _targetPoint));
			}
			break;

			case typeof(Vector2):
			break;

			default:
			Debug.LogError("Generic type T does not belong on switch dominion.");
			break;
		}

		return newList;
	}*/

	///\ TODO: Adapt this function to be a library function (with parameters overload) \\\
	/// <summary>Gets the second minium value of a float list.</summary>
	/// <param name="_list">The list of floats from where the second least value will be given.</param>
	/// <returns>Second minimum value of the list.</returns>
	public static float GetSecondMinimum(this List<float> _list)
	{
		//So they enter by default on minimum range.
		float least = Mathf.Infinity;
		float secondLeast = Mathf.Infinity;

		foreach(float number in  _list)
		{
			if(number <= least) //If current number is lower than the least value, then the prior least value passes as the secondLeast, and the least updates.
			{
				secondLeast = least;
				least = number;
			}
			else if(number < secondLeast) //If at least the current number is lower than the current second, update the second.
			secondLeast = number;
		}

		return secondLeast;
	}

	/// <summary>Gets the Vector3 property with the highest value.</summary>
	/// <param name="_vector3">The Vector3 that will compare its components.</param>
	/// <returns>Highest value between Vector3 components.</returns>
	public static float GetMaxVectorProperty(this Vector3 _vector3)
	{
		return Mathf.Max(_vector3.x, _vector3.y, _vector3.z);
	}

	/// <summary>Gets the Vector3 property with the lowest value.</summary>
	/// <param name="_vector3">The Vector3 that will compare its components.</param>
	/// <returns>Lowest value between Vector3 components.</returns>
	public static float GetMinVectorProperty(this Vector3 _vector3)
	{
		return Mathf.Min(_vector3.x, _vector3.y, _vector3.z);
	}

	/// <summary>Gets the Transform being visible on viewport.</summary>
	/// <param name="_transform">The Transform that will check if it's on viewport.</param>
	/// <returns>Transform being seen on viewport (bool).</returns>
	public static bool IsVisibleToCamera(this Transform _transform)
	{
		Vector3 transformView = Camera.main.WorldToViewportPoint(_transform.position);

		return (transformView.x > 0.0f && transformView.x < 1.0f && transformView.y > 0.0f && transformView.y < 1.0f);
	}

	/// <summary>Gets If GameObject has Component attached.</summary>
	/// <param name="_object">The GameObject that will check if has T Componentn attached.</param>
	/// <returns>GameObject has Component T (bool).</returns>
	public static bool Has<T>(this GameObject _object) where T : Object
	{
		return (_object.GetComponent<T>() != null);
	}

	/// <summary>Gets the direction vector towards target position.</summary>
	/// <param name="_fromPosition">The position from where de direction points.</param>
	/// <param name="_targetPosition">The position where the _fromPosition heads to.</param>
	/// <returns>Direction towards target point (Vector3).</returns>
	public static Vector3 GetDirectionTowards(this Vector3 _fromPosition, Vector3 _targetPosition)
	{
		return (_targetPosition - _fromPosition);
	}

#region Vector3:
	/// <summary>Rounds Vector3 components.</summary>
	/// <param name="_vector3">The Vector3 that will have its components rounded.</param>
	/// <returns>Vector3 with components rounded (0 or 1).</returns>
	public static Vector3 Round(this Vector3 _vector3)
	{
		return _vector3 = new Vector3(Mathf.Round(_vector3.x), Mathf.Round(_vector3.y), Mathf.Round(_vector3.z));
	}

	/// <summary>Sets Vector3 X.</summary>
	/// <param name="_vector">The Vector3 that will have its X modified.</param>
	/// <param name="_x">Updated Vector3 X Component.</param>
	public static Vector3 SetX(this Vector3 _vector, float _x)
	{
		return _vector = new Vector3(_x, _vector.y, _vector.z);
	}

	/// <summary>Sets Vector3 Y.</summary>
	/// <param name="_vector">The Vector3 that will have its Y modified.</param>
	/// <param name="_x">Updated Vector3 Y Component.</param>
	public static Vector3 SetY(this Vector3 _vector, float _y)
	{
		return _vector = new Vector3(_vector.x, _y, _vector.z);
	}

	/// <summary>Sets Vector3 Z.</summary>
	/// <param name="_vector">The Vector3 that will have its Z modified.</param>
	/// <param name="_x">Updated Vector3 Z Component.</param>
	public static Vector3 SetZ(this Vector3 _vector, float _z)
	{
		return _vector = new Vector3(_vector.x, _vector.y, _z);
	}

	/// <summary>Sets Vector3 X and Y.</summary>
	/// <param name="_vector">The Vector3 that will have its X and Y modified.</param>
	/// <param name="_x">Updated Vector3 X Component.</param>
	/// <param name="_y">Updated Vector3 Y Component.</param>
	public static Vector3 SetXY(this Vector3 _vector, float _x, float _y)
	{
		return _vector = new Vector3(_x, _y, _vector.z);
	}

	/// <summary>Sets Vector3 X and Z.</summary>
	/// <param name="_vector">The Vector3 that will have its X and Z modified.</param>
	/// <param name="_x">Updated Vector3 X Component.</param>
	/// <param name="_z">Updated Vector3 Z Component.</param>
	public static Vector3 SetXZ(this Vector3 _vector, float _x, float _z)
	{
		return _vector = new Vector3(_x, _vector.y, _z);
	}

	/// <summary>Sets Vector3 Y and Z.</summary>
	/// <param name="_vector">The Vector3 that will have its Y and Z modified.</param>
	/// <param name="_y">Updated Vector3 Y Component.</param>
	/// <param name="_z">Updated Vector3 Z Component.</param>
	public static Vector3 SetYZ(this Vector3 _vector, float _y, float _z)
	{
		return _vector = new Vector3(_vector.x, _y, _z);
	}
#endregion

#region Colors:
	/// <summary>Sets Color Alpha.</summary>
	/// <param name="_color">The Color that will have its Alpha modified.</param>
	/// <param name="_alpha">Updated Color Alpha Component.</param>
	/// <returns>New modified Color.</returns>
	public static Color SetAlpha(this Color _color, float _alpha)
	{
		return _color = new Color(_color.r, _color.g, _color.b, _alpha.ClampValue(-1.0f, 1.0f));
	}

	/// <summary>Sets Color Red.</summary>
	/// <param name="_color">The Color that will have its Red modified.</param>
	/// <param name="_red">Updated Color Red Component.</param>
	/// <returns>New modified Color.</returns>
	public static Color SetRed(this Color _color, float _red)
	{
		return _color = new Color(_red.ClampValue(-1.0f, 1.0f), _color.g, _color.b, _color.a);
	}

	/// <summary>Sets Color Green.</summary>
	/// <param name="_color">The Color that will have its Green modified.</param>
	/// <param name="_green">Updated Color Green Component.</param>
	/// <returns>New modified Color.</returns>
	public static Color SetGreen(this Color _color, float _green)
	{
		return _color = new Color(_color.r, _green.ClampValue(-1.0f, 1.0f), _color.b, _color.a);
	}

	/// <summary>Sets Color Blue.</summary>
	/// <param name="_color">The Color that will have its Blue modified.</param>
	/// <param name="_blue">Updated Color Blue Component.</param>
	/// <returns>New modified Color.</returns>
	public static Color SetBlue(this Color _color, float _blue)
	{
		return _color = new Color(_color.r, _color.g, _blue.ClampValue(-1.0f, 1.0f), _color.a);
	}
#endregion

#region Patriarchy:
	/// <summary>Gets all childs from parent.</summary>
	/// <param name="_transform">The Transform that owns the childs.</param>
	/// <returns>List of childs.</returns>
	public static List<Transform> GetChilds(this Transform _transform)
	{
		List<Transform> newList = new List<Transform>();

		foreach(Transform child in _transform)
		{
			newList.Add(child);
		}

		return newList;
	}

	/// <summary>Gets all childs from parent.</summary>
	/// <param name="_transform">The Transform that owns the childs.</param>
	/// <returns>List of childs.</returns>
	public static List<Transform> GetChildsWith<T>(this Transform _transform) where T : Object
	{
		List<Transform> newList = new List<Transform>();

		foreach(Transform child in _transform)
		{
			if(child.gameObject.Has<T>()) newList.Add(child);		
		}

		return newList;
	}

	/// <summary>Destroys all childs from parent.</summary>
	/// <param name="_transform">The Transform that owns the childs.</param>
	public static void KillAllChilds(this Transform _transform)
	{
		foreach(Transform child in _transform)
		{
			if(Application.isPlaying) Object.Destroy(child.gameObject);
			else if(Application.isEditor) Object.DestroyImmediate(child.gameObject);
		}
	}

	/// <summary>Destroys all childs from parent with T component.</summary>
	/// <param name="_transform">The Transform that owns the childs.</param>
	public static void KillAllChildsWith<T>(this Transform _transform) where T : Object
	{
		foreach(Transform child in _transform)
		{
			if(child.gameObject.GetComponent<T>() != null)
			{
				if(Application.isPlaying) Object.Destroy(child.gameObject);
				else if(Application.isEditor) Object.DestroyImmediate(child.gameObject);
			}		
		}
	}

	/// <summary>Adopts child from former Transform.</summary>
	/// <param name="_transform">The new parent of the childs.</param>
	/// <param name="_formerParent">The former parent of the childs.</param>
	/// <returns>List of new childs.</returns>
	public static List<Transform> AdoptChilds(this Transform _transform, Transform _formerParent)
	{
		List<Transform> newList = new List<Transform>();

		foreach(Transform child in _formerParent)
		{
			child.parent = _transform;
			newList.Add(child);
		}

		return newList;
	}

	/// <summary>Gets a List of child's T Components.</summary>
	/// <param name="_transform">The Transform that owns the Childs.</summary>
	/// <returns>List of T Components contained in Childs.</returns>
	public static List<T> GetComponentsFromChilds<T>(this Transform _transform) where T : Object
	{
		List<T> newList = new List<T>();

		foreach(Transform child in _transform)
		{
			if(child.gameObject.Has<T>()) newList.Add(child.gameObject.GetComponent<T>());
		}

		return newList;
	}

	/// <summary>Adopts child that posses T component from former Transform.</summary>
	/// <param name="_transform">The new parent of the childs.</param>
	/// <param name="_formerParent">The former parent of the childs.</param>
	/// <returns>List of new childs with T Component.</returns>
	public static List<GameObject> AdoptChildsWith<T>(this Transform _transform, Transform _formerParent) where T : Object
	{
		List<GameObject> newList = new List<GameObject>();

		foreach(Transform child in _formerParent)
		{
			if(child.gameObject.GetComponent<T>() != null)
			{
				child.parent = _transform;
				newList.Add(child.gameObject/*.GetComponent<T>()*/);
			}
		}

		return newList;
	}
#endregion

	/// <summary>Casts list of Component to a list of GameObject.</summary>
	/// <param name="_list">The Component List.</param>
	/// <returns>List GameObject with T Component.</returns>
	public static List<GameObject> CastComponentListAsGameObject<T>(this List<T> _list) where T : Object
	{
		List<GameObject> newList =  new List<GameObject>();

		foreach(T component in _list)
		{
			newList.Add(component as GameObject);
		}

		return newList;
	}

	/// <summary>Gets list of T Components from GameObject List.</summary>
	/// <param name="_list">The GameObject List.</param>
	/// <returns>List of GameObject List's T Component.</returns>
	public static List<T> GetComponentsFromGameObjects<T>(this List<GameObject> _list) where T : Object
	{
		List<T> newList =  new List<T>();

		foreach(GameObject UnityObject in _list)
		{
			if(UnityObject.Has<T>()) newList.Add(UnityObject.GetComponent<T>());
		}

		return newList;
	}
}