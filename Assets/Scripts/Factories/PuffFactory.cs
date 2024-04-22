
using UnityEngine;

public class PuffFactory : PrefabFactory
{
    [SerializeField] float delay = 0.25f;
    
    protected override void DoGet(GameObject element)
    {
        base.DoGet(element);
        
        Coroutines.StartCoroutine(
            Coroutines.Delay(delay, 
                () => Release(element)));
    }

}
