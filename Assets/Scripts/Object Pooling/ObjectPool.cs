using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoidlessUtilities
{
[CreateAssetMenu]
public class ObjectPool : SingletonScriptableObject<ObjectPool>
{
	private Dictionary<int, Queue<IPoolObject>> objectsPool; 	/// <summary>Pool of IPoolObjects.</summary>

	void Awake()
	{
		objectsPool = new Dictionary<int, Queue<IPoolObject>>();
	}

	/// <summary>Creates Pool of Objects [if it doesn't already exist].</summary>
	/// <param name="_object">Prefab implementing IPoolObject interface.</param>
	/// <param name="_poolSize">Size of the Pool.</param>
	public void CreateObjectsPool(IPoolObject _object, int _poolSize)
	{
		int instanceKey = 0; 	/// Instance ID of the Prefab.
		MonoBehaviour mono = null;

		if(_object is MonoBehaviour)
		{
			mono = _object as MonoBehaviour;
			instanceKey = mono.gameObject.GetInstanceID();
		}
		else
		{
			Debug.LogError("[ObjectPool] IPoolObject provided does not extend from MonoBehaviour. Pool's closed.");
			return;
		}

		if(!objectsPool.ContainsKey(instanceKey))
		{
			Queue<IPoolObject> newPoolQueue = new Queue<IPoolObject>();

			for(int i = 0; i < _poolSize; i++)
			{
				IPoolObject newPoolObject = Instantiate((UnityEngine.Object)((object)_object), Vector3.zero, Quaternion.identity) as IPoolObject;	
				newPoolObject.OnObjectCreation();
				newPoolQueue.Enqueue(newPoolObject);
			}

			objectsPool.Add(instanceKey, newPoolQueue);
		}
	}
}
}