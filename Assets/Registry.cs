using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Registry : MonoBehaviour
{

    static public Item[] itemRegistry;
    static public List<string> technicalRegistry = new List<string>();
    void Awake()
    {
        itemRegistry = GetAllInstances<Item>();
        foreach (Item item in itemRegistry) // populate technical registry
        {
            Debug.Log(item.technicalName + "\n" + item.itemDescription);
            if (!technicalRegistry.Contains(item.technicalName))
            {
                technicalRegistry.Add(item.technicalName);
            }
            else
            {
                throw new DuplicateTechnicalNameException();
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }

    public static T[] GetAllInstances<T>() where T : ScriptableObject
    {
        return Resources.LoadAll<T>("SObjects");

    }

    static public Item LookupItem(string name)
    {
        foreach (Item item in itemRegistry)
        {
            if (item.itemName == name)
            {
                return item;
            }
        }
        throw new ItemNotFoundException();
    }

    static public Item LookupItemTechnical(string technicalName)
    {
        foreach (Item item in itemRegistry)
        {
            if (item.technicalName == technicalName)
            {
                return item;
            }
        }
        throw new ItemNotFoundException();
    }

    static public Item LookupItem(int index)
    {
        return itemRegistry[index];
    }
}
public class ItemNotFoundException : Exception
{

}

public class DuplicateTechnicalNameException : Exception
{

}