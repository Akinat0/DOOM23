using System.Collections.Generic;
using UnityEngine;

public class PrefabFactory : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] int maxSize;

    Stack<GameObject> Stack { get; set; }

    public int CountAll { get; private set; }

    public int CountActive => CountAll - CountInactive;
    public int CountInactive => Stack.Count;


    protected virtual GameObject DoCreate()
    {
        return Instantiate(prefab);
    }
    
    protected virtual void DoGet(GameObject element)
    {
        
    }
    
    protected virtual void DoRelease(GameObject element)
    {
        
    }
    
    protected virtual void DoDestroy(GameObject element)
    {
        Destroy(element);
    }


    public void Awake()
    {
        Stack = new Stack<GameObject>();
    }
    
    void OnDestroy()
    {
        Clear();
    }

    public GameObject Get()
    {
        GameObject element;
        if (Stack.Count == 0)
        {
            element = DoCreate();
            CountAll++;
        }
        else
        {
            element = Stack.Pop();
        }

        DoGet(element);
        return element;
    }

    public TComp Get<TComp>() where TComp : Component => Get().GetComponent<TComp>();

    public void Release(GameObject element)
    {
        if (Stack.Count > 0 && Stack.Contains(element))
        {
            Debug.LogError("Trying to release an object that has already been released to the pool.");
            return;
        }

        DoRelease(element);
        
        if (CountInactive < maxSize)
        {
            Stack.Push(element);
        }
        else
        {
            DoDestroy(element);
        }
    }
    

    public void Clear()
    {
        foreach (GameObject element in Stack)
            DoDestroy(element);

        Stack.Clear();
        CountAll = 0;
    }

    
}