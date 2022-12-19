using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Xml.Serialization;

namespace VoidlessUtilities
{
#region VoidlessMath:
	public class VoidlessMath
	{
		public const float PHI = 1.61803398874989484820458683436563811772030917980576f; 	/// <summary>Golden Ratio Constant.</summary>
		public const float E = 2.71828182845904523536028747135266249775724709369995f; 		/// <summary>Euler's Number Constant</summary>

		public enum CoordinatesModes 														/// <summary>Coordinates Modes.</summary>
		{
			XY, 																			/// <summary>X and Y Coordinate Mode.</summary>
			YX, 																			/// <summary>Y and X Coordinate Mode.</summary>
			XZ, 																			/// <summary>X and Z Coordinate Mode.</summary>
			ZY, 																			/// <summary>Z and Y Coordinate Mode.</summary>
			YZ, 																			/// <summary>Y and Z Coordinate Mode.</summary>
			ZX 																				/// <summary>Z and X Coordinate Mode.</summary>
		}

		/// <summary>Gets middle point between n number of points (positions).</summary>
		/// <param name="_points">The points from where the middle point will be calculated.</param>
		/// <returns>Middle point between n points.</returns>
		public static Vector3 GetMiddlePointBetween(params Vector3[] _points)
		{
			Vector3 middlePoint = Vector3.zero;

			foreach(Vector3 point in _points)
			{
				middlePoint += point;
			}

			return (middlePoint / _points.Length);
		}

		/// <summary>Gets normalized point between n number of points (positions).</summary>
		/// <param name="_normalizedValue">The normal of the points sumatory.</param>
		/// <param name="_points">The points from where the middle point will be calculated.</param>
		/// <returns>Normalized point between n points.</returns>
		public static Vector3 GetNormalizedPointBetween(float _normalizedValue, params Vector3[] _points)
		{
			_normalizedValue.ClampValue(-1.0f, 1.0f); //Clamp the value if the parameter given is below -1.0f, or above 1.0f
			Vector3 middlePoint = Vector3.zero;

			foreach(Vector3 point in _points)
			{
				middlePoint += point;
			}

			return (middlePoint * _normalizedValue);
		}

		/// <summary>Sets Integer to clamped value.</summary>
		/// <param name="_int">Integer that will be clamped.</param>
		/// <param name="_min">Minimum value clamped.</param>
		/// <param name="_max">Maximum value clamped.</param>
		/// <returns>Integer clamped (as int).</returns>
		public static int ClampSet(ref int _int, int _min, int _max)
		{
			return _int = Mathf.Clamp(_int, _min, _max);
		}

		/// <summary>Sets float to clamped value.</summary>
		/// <param name="_float">Float that will be clamped.</param>
		/// <param name="_min">Minimum value clamped.</param>
		/// <param name="_max">Maximum value clamped.</param>
		/// <returns>Float clamped (as float).</returns>
		public static float ClampSet(ref float _float, float _min, float _max)
		{
			return _float = Mathf.Clamp(_float, _min, _max);
		}

		/// <summary>Converts normalized float value to byte.</summary>
		/// <param name="_b">Byte value to convert.</param>
		/// <param name="_f">Float value that converts the Byte.</param>
		/// <returns>Converted Byte.</returns>
		public static byte FloatToColor32Byte(out byte _b, float _f)
		{
			return _b = (byte)(_f >= 1.0f ? 255 : (_f <= 0.0f ? 0 : (byte)Mathf.Floor(_f * 256.0f)));
		}

		/// <summary>Gets 360 system angle between 2 points.</summary>
		/// <param name="_fromPoint">Point from where the angle starts.</param>
		/// <param name="_toPoint">Point the origin point is pointing towards.</param>
		/// <param name="_coordinatesMode">Coordinates Mode.</param>
		/// <returns>360 range angle (as float).</returns>
		public static float Get360Angle(Vector3 _fromPoint, Vector3 _toPoint, CoordinatesModes _coordinatesMode)
		{
			Vector2 direction = Vector2.zero;

			switch(_coordinatesMode)
			{
				case CoordinatesModes.XY:
				direction = new Vector2((_fromPoint.x - _toPoint.x), (_fromPoint.y - _toPoint.y));
				break;

				case CoordinatesModes.YX:
				direction = new Vector2((_fromPoint.y - _toPoint.y), (_fromPoint.x - _toPoint.x));
				break;

				case CoordinatesModes.XZ:
				direction = new Vector2((_fromPoint.x - _toPoint.x), (_fromPoint.z - _toPoint.z));
				break;

				case CoordinatesModes.ZY:
				direction = new Vector2((_fromPoint.z - _toPoint.z), (_fromPoint.y - _toPoint.y));
				break;

				case CoordinatesModes.YZ:
				direction = new Vector2((_fromPoint.y - _toPoint.y), (_fromPoint.z - _toPoint.z));
				break;

				case CoordinatesModes.ZX:
				direction = new Vector2((_fromPoint.z - _toPoint.z), (_fromPoint.x - _toPoint.x));
				break;
			}

			return direction.y < 0f || direction.x < 0f &&direction.y < 0f ? (Mathf.Atan2(direction.y, direction.x) + (Mathf.PI * 2)) * Mathf.Rad2Deg : Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		}
	}
#endregion

#region VoidlessColor
	public static class VoidlessColor
	{
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
	}
#endregion

#region VoidlessColor32
	public static class VoidlessColor32
	{
		/// <summary>Sets Color32 Alpha.</summary>
		/// <param name="_color">The Color32 that will have its Alpha modified.</param>
		/// <param name="_alpha">Updated Color32 Alpha Component.</param>
		/// <returns>New modified Color32.</returns>
		public static Color32 SetAlpha(this Color32 _color, byte _alpha)
		{
			return _color = new Color32(_color.r, _color.g, _color.b, _alpha);
		}
	}
#endregion

#region VoidlessString:
	public static class VoidlessString
	{
		/// <summary>Sets string to Camel Case format.</summary>
		/// <param name="_text">Text to format to Camel Case.</param>
		/// <returns>Formated text.</returns>
		public static string ToCamelCase(this string _text)
		{
			return _text.Replace(_text[0], char.ToLower(_text[0]));
		}
	}
#endregion

#region VoidlessVector3
	public static class VoidlessVector3
	{
		/// <summary>Gets a regular Vector3 [with all components equally valued].</summary>
		/// <param name="_regularValue">Regular value for all Vector3 components.</param>
		/// <returns>Regular Vector3.</returns>
		public static Vector3 RegularVector3(float _regularValue)
		{
			return new Vector3(_regularValue, _regularValue, _regularValue);
		}

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
		public static Vector3 SetXAndY(this Vector3 _vector, float _x, float _y)
		{
			return _vector = new Vector3(_x, _y, _vector.z);
		}

		/// <summary>Sets Vector3 X and Z.</summary>
		/// <param name="_vector">The Vector3 that will have its X and Z modified.</param>
		/// <param name="_x">Updated Vector3 X Component.</param>
		/// <param name="_z">Updated Vector3 Z Component.</param>
		public static Vector3 SetXAndZ(this Vector3 _vector, float _x, float _z)
		{
			return _vector = new Vector3(_x, _vector.y, _z);
		}

		/// <summary>Sets Vector3 Y and Z.</summary>
		/// <param name="_vector">The Vector3 that will have its Y and Z modified.</param>
		/// <param name="_y">Updated Vector3 Y Component.</param>
		/// <param name="_z">Updated Vector3 Z Component.</param>
		public static Vector3 SetYAndZ(this Vector3 _vector, float _y, float _z)
		{
			return _vector = new Vector3(_vector.x, _y, _z);
		}

		/// <summary>Adds value to Vector3 X component.</summary>
		/// <param name="_vector">The Vector3 that will have its X subtracted by value.</param>
		/// <param name="_addedX">Added value to Vector3 X Component.</param>
		/// <returns>Vector with subtracted X component by value.</summary>
		public static Vector3 AddToX(this Vector3 _vector, float _addedX)
		{
			return _vector = new Vector3((_vector.x + _addedX), _vector.y, _vector.z);
		}

		/// <summary>Adds value to Vector3 Y component.</summary>
		/// <param name="_vector">The Vector3 that will have its Y subtracted by value.</param>
		/// <param name="_addedY">Added value to Vector3 Y Component.</param>
		/// <returns>Vector with subtracted Y component by value.</summary>
		public static Vector3 AddToY(this Vector3 _vector, float _addedY)
		{
			return _vector = new Vector3(_vector.x, (_vector.y + _addedY), _vector.z);
		}

		/// <summary>Adds value to Vector3 Z component.</summary>
		/// <param name="_vector">The Vector3 that will have its Z subtracted by value.</param>
		/// <param name="_addedZ">Added value to Vector3 Z Component.</param>
		/// <returns>Vector with subtracted Z component by value.</summary>
		public static Vector3 AddToZ(this Vector3 _vector, float _addedZ)
		{
			return _vector = new Vector3(_vector.x, _vector.y, (_vector.z + _addedZ));
		}

		/// <summary>Adds value to Vector3 X and Y components.</summary>
		/// <param name="_vector">The Vector3 that will have its X and Y subtracted by values.</param>
		/// <param name="_addedX">Added value to Vector3 X Component.</param>
		/// <param name="_addedY">Added value to Vector3 Y Component.</param>
		/// <returns>Vector with subtracted X and Y components by values.</summary>
		public static Vector3 AddToXAndY(this Vector3 _vector, float _addedX, float _addedY)
		{
			return _vector = new Vector3((_vector.x + _addedX), (_vector.y + _addedY), _vector.z);
		}

		/// <summary>Adds value to Vector3 X and Z components.</summary>
		/// <param name="_vector">The Vector3 that will have its X and Z subtracted by values.</param>
		/// <param name="_addedX">Added value to Vector3 X Component.</param>
		/// <param name="_addedZ">Added value to Vector3 Z Component.</param>
		/// <returns>Vector with subtracted X and Z components by values.</summary>
		public static Vector3 AddToXAndZ(this Vector3 _vector, float _addedX, float _addedZ)
		{
			return _vector = new Vector3((_vector.x + _addedX), _vector.y, (_vector.z + _addedZ));
		}

		/// <summary>Adds value to Vector3 Y and Z components.</summary>
		/// <param name="_vector">The Vector3 that will have its Y and Z subtracted by values.</param>
		/// <param name="_addedY">Added value to Vector3 Y Component.</param>
		/// <param name="_addedZ">Added value to Vector3 Z Component.</param>
		/// <returns>Vector with subtracted Y and Z components by values.</summary>
		public static Vector3 AddToYAndZ(this Vector3 _vector, float _addedY, float _addedZ)
		{
			return _vector = new Vector3(_vector.x, (_vector.y + _addedY), (_vector.z + _addedZ));
		}

		/// <summary>Inverts Vector3's X component.</summary>
		/// <param name="_vector">The Vector3 that will have its X inverted.</param>
		/// <returns>Vector3 with X component inverted.</returns>
		public static Vector3 InvertX(this Vector3 _vector)
		{
			return _vector = new Vector3(-_vector.x, _vector.y, _vector.z);
		}
	}
#endregion

#region VoidlessVector2:
	public static class VoidlessVector2
	{
		/// <summary>Converts Vector3 to Vector2 [Ignores Z].</summary>
		/// <param name="_vector3">Vector3 that will be converted to Vector2.</param>
		/// <returns>Converted Vector2.</returns>
		public static Vector2 ToVector2(this Vector3 _vector3)
		{
			return new Vector2(_vector3.x, _vector3.y);
		}

		/// <summary>Sets Vector2 X.</summary>
		/// <param name="_vector">The Vector2 that will have its X modified.</param>
		/// <param name="_x">Updated Vector2 X Component.</param>
		public static Vector2 SetX(this Vector2 _vector, float _x)
		{
			return _vector = new Vector2(_x, _vector.y);
		}

		/// <summary>Sets Vector2 Y.</summary>
		/// <param name="_vector">The Vector2 that will have its Y modified.</param>
		/// <param name="_x">Updated Vector2 Y Component.</param>
		public static Vector2 SetY(this Vector2 _vector, float _y)
		{
			return _vector = new Vector2(_vector.x, _y);
		}
	}
#endregion

#region VoidlessRectTransform
	public static class VoidlessRectTransform
	{
		/*/// <summary>Sets Local Scale X Component equal to value.</summary>
		/// <param name="_rectTransform">RectTransform that will have its Local Scale X Componentn modified.</param>
		/// <param name="_newX">New X Value.</param>
		/// <returns>RectTransform's Local Scale
		public static Vector3 SetLocalScaleX()
		{

		}*/
	}
#endregion

#region VoidlessList:
	public static class VoidlessList
	{
		/*/// <summary>Checks if all elements on list accomplishes all conditions.</summary>
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
		}*/
	}
#endregion

#region VoidlessDictionary:
	public static class VoidlessDictionary
	{
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
	}
#endregion

#region VoidlessExtensionMethods
	public static class VoidlessExtensionMethods
	{
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

		/// <summay>Sets Quaternion.Euler Y component.</summary>
		/// <param name="_quaternion">Queternion that will have its eulerAnglles.y modified.</param>
		/// <param name="_y">Ne Y component value.</param>
		/// <returns>Quaternion with eulerAngles.y modified.</returns>
		public static Quaternion SetY(this Quaternion _quaternion, float _y)
		{
			return Quaternion.Euler(_quaternion.eulerAngles.x, _y, _quaternion.eulerAngles.z);
		}

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
		public static void DestroyAllElements<T>(this List<T> _list) where T : UnityEngine.Object
		{
			foreach(T UnityObject in _list)
			{
				if(Application.isPlaying) UnityEngine.Object.Destroy(UnityObject);
				else if(Application.isEditor) UnityEngine.Object.DestroyImmediate(UnityObject);
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

		/// <summary>Gets the Square Magnitudes between List of Vector3 and Target Vector3.</summary>
		/// <param name="List">the List of Vector3 from where the Square Magnitudes will be measured.</param>
		/// <param name="_targetPoint">The Vector3 from where the List of Vector3 will measure the Square Distances.</param>
		/// <returns>List of Square Magnitudes (float).</returns>
		public static List<float> GetSquareMagnitudesFromVectors(this List <Vector3> _list, Vector3 _targetPoint)
		{
			List<float> newList = new List<float>();

			foreach(Vector3 point in _list)
			{
				newList.Add(point.GetDirectionTowards(_targetPoint).sqrMagnitude);
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

		/// <summary>Sets Active collection of GameObjects to the desired bool.</summary>
		/// <param name="_active">Determines whether all GameObjects will be or not active.</param>
		public static void SetAllActive(this List<GameObject> _gameObjects, bool _active)
		{
			for(int i = 0; i < _gameObjects.Count; i++)
			{
				_gameObjects[i].SetActive(_active);
			}
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
		public static bool Has<T>(this GameObject _object) where T : UnityEngine.Object
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
		public static List<Transform> GetChildsWith<T>(this Transform _transform) where T : UnityEngine.Object
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
				if(Application.isPlaying) UnityEngine.Object.Destroy(child.gameObject);
				else if(Application.isEditor) UnityEngine.Object.DestroyImmediate(child.gameObject);
			}
		}

		/// <summary>Destroys all childs from parent with T component.</summary>
		/// <param name="_transform">The Transform that owns the childs.</param>
		public static void KillAllChildsWith<T>(this Transform _transform) where T : UnityEngine.Object
		{
			foreach(Transform child in _transform)
			{
				if(child.gameObject.GetComponent<T>() != null)
				{
					if(Application.isPlaying) UnityEngine.Object.Destroy(child.gameObject);
					else if(Application.isEditor) UnityEngine.Object.DestroyImmediate(child.gameObject);
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
		public static List<T> GetComponentsFromChilds<T>(this Transform _transform) where T : UnityEngine.Object
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
		public static List<GameObject> AdoptChildsWith<T>(this Transform _transform, Transform _formerParent) where T : UnityEngine.Object
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
		public static List<GameObject> CastComponentListAsGameObject<T>(this List<T> _list) where T : UnityEngine.Object
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
		public static List<T> GetComponentsFromGameObjects<T>(this List<GameObject> _list) where T : UnityEngine.Object
		{
			List<T> newList =  new List<T>();

			foreach(GameObject UnityObject in _list)
			{
				if(UnityObject.Has<T>()) newList.Add(UnityObject.GetComponent<T>());
			}

			return newList;
		}
	}
#endregion

#region VoidlessTexture:
	public class VoidlessTexture
	{
		/// <summary>Converts Sprite to Texture2D.</summary>
		/// <param name="_sprite">Sprite to copy pixels from.</param>
		/// <returns>Texture2D with Sprite's pixels.</returns>
		public static Texture2D ToTexture(Sprite _sprite)
		{
			Texture2D newTexture = new Texture2D((int)_sprite.rect.width, (int)_sprite.rect.height);

			Color[] pixels = _sprite.texture.GetPixels
			( 
				(int)_sprite.textureRect.x, 
                (int)_sprite.textureRect.y, 
                (int)_sprite.textureRect.width, 
                (int)_sprite.textureRect.height
            );

            newTexture.SetPixels(pixels);
            newTexture.Apply();
            return newTexture;
		}
	}
#endregion

#region VoidlessCoroutine:
	public static class VoidlessCoroutine
	{
		/// <summary>Calls CoroutineController.Instance's WaitSeconds own method.</summary>
		/// <param name="_monoBehaviour">MonoBehaviour caller reference to check if it's still alive at each ticking.</param>
		/// <param name="_seconds">Seconds to wait.</param>
		/// <param name="onWaitEnds">Action called when the wait ends.</param>
		/// <returns>Behaviour with WaitSecondsCoroutine reference on the constructor [from Instance's method].</returns>
		public static Behavior WaitSeconds(this MonoBehaviour _monoBehaviour, float _seconds, Action onWaitEnds)
		{
			return CoroutineController.Instance.WaitSeconds(_monoBehaviour, _seconds, onWaitEnds);
		}

		/// <summary>Calls CoroutineController.Instance's WaitUntil own method.</summary>
		/// <param name="_monoBehaviour">MonoBehaviour caller reference to check if it's still alive at each ticking.</param>
		/// <param name="_seconds">Seconds to wait.</param>
		/// <param name="onConditionGiven">Action called when the condition is given.</param>
		/// <returns>Behaviour with WaitUntilCoroutine reference on the constructor [from Instance's method].</returns>
		public static Behavior WaitUntil(this MonoBehaviour _monoBehaviour, System.Func<bool> _condition, Action onConditionGiven)
		{
			return CoroutineController.Instance.WaitUntil(_monoBehaviour, _condition, onConditionGiven);
		}
	}
#endregion

#region VoidlessReflection:
	public static class VoidlessReflection
	{
		/// <summary>Gets a collection of specified signature methods from MonoBehaviour.</summary>
		/// <param name="_mono">MonoBehaviour to extract methods from.</param>
		/// <param name="_returnType">Return type methods criteria.</param>
		/// <param name="_paramTypes">Type of Parameters that the Method signature must have criteria [null if no parameters expected].</param>
		/// <param name="_flags">Methods binding flags.</param>
		/// <returns>List of MethodInfos with the established criteria.</returns>
		public static List<MethodInfo> GetMethods(this MonoBehaviour _mono, Type _returnType, BindingFlags _flags, params Type[] _paramTypes)
		{
			return _mono.GetType().GetMethods(_flags).Where
			(
				method => method.ReturnType == _returnType
			).Select
			(
				method => new
				{
					method, Params = method.GetParameters()
				}
			).Where
			(x =>
				{
					return _paramTypes == null ?
					x.Params.Length == 0 : 
					x.Params.Length == _paramTypes.Length && x.Params
					.Select(p => p.ParameterType).ToArray().IsEqualTo(_paramTypes);
				}
			).Select
			(
				x => x.method
			).ToList();
		}

		/// <summary>[Overload Method] Gets a collection of specified signature methods from MonoBehaviour. Does not care about the specifics of parameters on Method's signature</summary>
		/// <param name="_mono">MonoBehaviour to extract methods from.</param>
		/// <param name="_returnType">Return type methods criteria.</param>
		/// <param name="_flags">Methods binding flags.</param>
		/// <returns>List of MethodInfos with the established criteria.</returns>
		public static List<MethodInfo> GetMethods(this MonoBehaviour _mono, Type _returnType, BindingFlags _flags)
		{
			return _mono.GetType().GetMethods(_flags).Where
			(
				method => method.ReturnType == _returnType
			).ToList();
		}

		/// <summary>[Overload Method] Gets a collection of specified signature methods from MonoBehaviour. Does not care about the specifics of parameters on Method's signature</summary>
		/// <param name="_mono">MonoBehaviour to extract methods from.</param>
		/// <param name="_returnType">Return type methods criteria.</param>
		/// <param name="_flags">Methods binding flags.</param>
		/// <returns>List of MethodInfos with the established criteria.</returns>
		public static List<MethodInfo> GetMethods(this GameObject _gameObject, Type _returnType, BindingFlags _flags)
		{
			MonoBehaviour[] monos = _gameObject.GetComponents<MonoBehaviour>();
			List<MethodInfo> methodsInfo = new List<MethodInfo>();

			foreach(MonoBehaviour mono in monos)
			{
				methodsInfo.AddRange(mono.GetMethods(_returnType, _flags));	
			}

			return methodsInfo;
		}

		/// <summary>Gets a collection of specified signature methods from GameObject.</summary>
		/// <param name="_mono">GameObject to extract methods from.</param>
		/// <param name="_returnType">Return type methods criteria.</param>
		/// <param name="_paramTypes">Type of Parameters that the Method signature must have criteria [null if no parameters expected].</param>
		/// <param name="_flags">Methods binding flags.</param>
		/// <returns>List of MethodInfos with the established criteria.</returns>
		public static List<MethodInfo> GetMethods(this GameObject _gameObject, Type _returnType, BindingFlags _flags, params Type[] _paramTypes)
		{
			MonoBehaviour[] monos = _gameObject.GetComponents<MonoBehaviour>();
			List<MethodInfo> methodsInfo = new List<MethodInfo>();

			foreach(MonoBehaviour mono in monos)
			{
				methodsInfo.AddRange(mono.GetMethods(_returnType, _flags, _paramTypes));	
			}

			return methodsInfo;
		}

		/// \TODO Move this Extension Method to another VoidlessExtension class that is specific for either Lists or IList
		/// <summary>Checks if IList equals another IList.</summary>
		/// <param name="_list">IList to compare.</param>
		/// <param name="_otherList">Other IList to compare this IList.</param>
		/// <returns>If IList equals the other IList.</returns>
		public static bool IsEqualTo<T>(this IList<T> _list, IList<T> _otherList)
		{
			if(_list.Count != _otherList.Count) return false;
			else
			{
				for(int i = 0; i < _list.Count; i++)
				{
					if(!_list[i].Equals(_otherList[i])) return false;	
				}
			}

			return true;
		}
	}
#endregion

#region VoidlessEditorWindow:
	public static class VoidlessEditorWindow
	{
		/// \TODO XML Documentation of this method...
		/*public static Rect GetVerticalLayoutDisplacement(this Rect _rect, ref float _currentLayoutY, float _verticalDisplacement, Vector2 _rectOffset)
		{
			return new Rect
			{
				(_rect.x + _rectOffset.x),
				((_rect.y + _rectOffset.y) + (_currentLayoutY += _verticalDisplacement)),
				(_rect.width - (_rectOffset.x * 2f)),
				_rectOffset.y;
			}
		}*/

		/// <summary>Creates most adequate object Field on EditorGUI Layout.</summary>
		public static object CreateEditorGUIField(this Rect _fieldRect, string _label, Type _objectType, object _methodArgument)
		{
			//for(int i = 0; i < _methodParameter.Length; i++)
			{
				//Type t = _methodParameter.ParameterType;

				if(_objectType == typeof(UnityEngine.Object))
				{
					_methodArgument = EditorGUI.ObjectField(_fieldRect, _label, (UnityEngine.Object)(object)_methodArgument, _objectType, true) as UnityEngine.Object;
				}
				else if(_objectType == typeof(UnityEngine.GameObject))
				{
					return _methodArgument = EditorGUI.ObjectField(_fieldRect, _label, (UnityEngine.GameObject)(object)_methodArgument, _objectType, true) as UnityEngine.GameObject;
				}
				else if(_objectType == typeof(System.Enum))
				{
					//Type enumType = System.Enum.GetUnderlyingType(_objectType);
					//_methodArgument = EditorGUI.EnumPopup(_fieldRect, _label, (enumType)(object)_objectType);
					//_methodArgument = _methodArgument.CreateEnumPopUp(_objectType, _fieldRect, _label);
				}
				else if(_objectType == typeof(string))
				{

				}
				else if(_objectType == typeof(int))
				{

				}

				return _methodArgument;
			}
		}

		/// <summary>Abstract Layer that creates EditorGUI object Field relative to the data Type needed.</summary>
		/// <param name="_object">Object variable to evaluate.</param>
		/// <param name="_type">Type of the new EditorGUI Field.</param>
		/// <param name="_layoutPosition">Field's Layout position.</param>
		/// <param name="_label">Field's Label.</param>
		/// <returns>Most proper EditorGUI's Field [null if there is none].</returns>
		public static object CreatePropertyField<T>(this object _object, Type _type, Rect _layoutPosition, string _label) where T : System.Type
		{
			if(_type == typeof(UnityEngine.Object))
			{
				//return _object.CreateObjectField<>(_type.GetType(), _layoutPosition, _label);

				///Should run CreateObjectField.
			}
			else if(_type == typeof(System.Enum))
			{
				
			}

			return _object;
		}

		/// <summary>Creates EditorGUI object Field.</summary>
		/// <param name="_object">Object variable to evaluate.</param>
		/// <param name="_type">Type of the new EditorGUI Field.</param>
		/// <param name="_layoutPosition">Field's Layout position.</param>
		/// <param name="_label">Field's Label.</param>
		/// <returns>Most proper EditorGUI's Field [null if there is none].</returns>
		public static object CreateObjectField<T>(this object _object, T _type, Rect _layoutPosition, string _label) where T : UnityEngine.Object
		{
			return _object = EditorGUI.ObjectField(_layoutPosition, _label, (T)_object, typeof(T), true) as T;
		}

		/// <summary>Creates EditorGUI Enum Pop-Up Field.</summary>
		/// <param name="_object">Object variable to evaluate.</param>
		/// <param name="_type">Type of the new EditorGUI Field.</param>
		/// <param name="_layoutPosition">Field's Layout position.</param>
		/// <param name="_label">Field's Label.</param>
		/// <returns>Most proper EditorGUI's Field [null if there is none].</returns>
		public static object CreateEnumPopUp(this object _object, System.Enum _type, Rect _layoutPosition, string _label)
		{
			return _object = EditorGUI.EnumPopup(_layoutPosition, _label, _type);			
		}

		/// <summary>Creates EditorGUI Pop-Up Field.</summary>
		/// <param name="_object">Object variable to evaluate.</param>
		/// <param name="_layoutPosition">Field's Layout position.</param>
		/// <param name="_label">Field's Label.</param>
		/// <param name="_displayedOptions">Options to display [Either by a direct array or a converted one by params].</param>
		/// <returns>Index of the current selected Pop-Up's array element.</returns>
		public static int CreatePopUp(this object _object, Rect _layoutPosition, string _label, params string[] _displayedOptions)
		{
			return EditorGUI.Popup(_layoutPosition, _label, (int)_object, _displayedOptions);
		}
	}
#endregion

#region VoidlessXMLSerializer
	public class VoidlessXMLSerializer
	{
		/// <summary>Serialized Object on Path given.</summary>
		/// <param name="_item">Object to serialize.</param>
		/// <param name="_path">Path to save the Object.</param>
		/// <param name="onSerializationEnds">Optional Action called after the serialization ends.</param>
		public static void Serialize(object _item, string _path, Action onSerializationEnds)
		{
			XmlSerializer serializer = new XmlSerializer(_item.GetType());
			StreamWriter writer = new StreamWriter(_path);
			serializer.Serialize(writer.BaseStream, _item);
			writer.Close();

			if(onSerializationEnds != null) onSerializationEnds();
		}

		/// <summary>Deserialized [Loads] Object on given Path.</summary>
		/// <param name="_path">Object's Path.</param>
		/// <param name="onDeserializationEnds">Optional Action called just before deserializing Object.</param>
		/// <returns>Deserialized [Loaded] Object.</returns>
		public static T Deserialize<T>(string _path, Action onDeserializationEnds)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			StreamReader reader = new StreamReader(_path);
			T deserialized = (T)serializer.Deserialize(reader.BaseStream);
			reader.Close();

			if(onDeserializationEnds != null) onDeserializationEnds();

			return deserialized;
		}
	}
#endregion

}
