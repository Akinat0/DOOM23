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
        element.SetActive(true);
    }
    
    protected virtual void DoRelease(GameObject element)
    {
        element.SetActive(false);
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



    public GameObject Get() => Get(null, Vector3.zero, Quaternion.identity);
    public GameObject Get(Transform parent) => Get(parent, Vector3.zero, Quaternion.identity);
    public GameObject Get(Transform parent, Vector3 position) => Get(parent, position, Quaternion.identity);
    public GameObject Get(Vector3 position) => Get(null, position, Quaternion.identity);
    public GameObject Get(Vector3 position, Quaternion rotation) => Get(null, position, rotation);
    
    public GameObject Get(Transform parent, Vector3 position, Quaternion rotation)
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

        element.transform.parent = parent;
        element.transform.localPosition = position;
        element.transform.rotation = rotation;
        
        return element;
    }

    public TComp Get<TComp>() where TComp : Component => Get().GetComponent<TComp>();
    public TComp Get<TComp>(Transform parent) where TComp : Component => Get(parent).GetComponent<TComp>();
    public TComp Get<TComp>(Transform parent, Vector3 pos) where TComp : Component => Get(parent, pos).GetComponent<TComp>();
    public TComp Get<TComp>(Transform parent, Vector3 pos, Quaternion rotation) where TComp : Component => Get(parent, pos, rotation).GetComponent<TComp>();
    public TComp Get<TComp>(Vector3 pos) where TComp : Component => Get(pos).GetComponent<TComp>();
    public TComp Get<TComp>(Vector3 pos, Quaternion rotation) where TComp : Component => Get(pos, rotation).GetComponent<TComp>();


    public void Release(Component component) => Release(component.gameObject);
    public void Release(GameObject element)
    {
        if (Stack.Count > 0 && Stack.Contains(element))
        {
            Debug.LogError("Trying to release an object that has already been released to the pool.");
            return;
        }

        DoRelease(element);
        
        if (maxSize <= 0 || CountInactive < maxSize)
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