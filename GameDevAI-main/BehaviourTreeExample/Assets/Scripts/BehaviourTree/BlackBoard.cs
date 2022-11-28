using System.Collections.Generic;

public class BlackBoard
{
    private Dictionary<string, object> storedData = new Dictionary<string, object>();

    /// <summary>
    /// Adds an item to the data with _id, if an object with _id already exits updates it.
    /// </summary>
    /// <param name="_id"></param>
    /// <param name="_data"></param>
    public void AddOrUpdate(string _id, object _data)
    {
        if (storedData.ContainsKey(_id))
        {
            storedData[_id] = _data;
        }
        else
        {
            storedData.Add(_id, _data);
        }
    }

    public void RemoveEntry(string _id)
    {
        if (storedData.ContainsKey(_id))
        {
            storedData.Remove(_id);
        }
    }

    public bool Contains(string _id)
    {
        return storedData.ContainsKey(_id);
    }

    /// <summary>
    /// Gets object of type T from the blackboard data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_id"></param>
    /// <returns>Returns the object stored under the id _id, null if no such item exists</returns>
    public T Get<T>(string _id)
    {
        object data;
        storedData.TryGetValue(_id, out data);
        return (T) data;
    }
}

