
using UnityEngine;

public class PuffFactory : PrefabFactory
{
    protected override void DoGet(GameObject element)
    {
        base.DoGet(element);
        element.SetActive(true);
        Coroutines.StartCoroutine(
            Coroutines.Delay(0.25f, 
                () => Release(element)));
    }

    protected override void DoRelease(GameObject element)
    {
        base.DoRelease(element);
        element.SetActive(false);
    }
}
