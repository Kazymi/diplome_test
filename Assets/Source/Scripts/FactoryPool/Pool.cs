using System.Collections.Generic;

public class Pool<T> : IPool<T> where T : IPooledObject
{
    private readonly IFactory<T> _factory;
    private readonly List<T> _pooledObjects = new List<T>();

    public Pool(IFactory<T> factory, int amountObject)
    {
        _factory = factory;

        for (var i = 0; i < amountObject; i++)
        {
            _pooledObjects.Add(NewPoolObject());
        }
    }

    public T Pull()
    {
        if (_pooledObjects.Count == 0)
        {
            _pooledObjects.Add(NewPoolObject());
        }

        var returnValue = _pooledObjects[0];
        returnValue.Initialize();
        _pooledObjects.Remove(returnValue);
        return returnValue;
    }

    public void Push(IPooledObject pooledObject)
    {
        _pooledObjects.Add((T) pooledObject);
    }

    private T NewPoolObject()
    {
        var returnValue = _factory.CreatePoolObject();
        returnValue.SetParentPool(this);
        return returnValue;
    }
}