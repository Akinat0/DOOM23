using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIVignette : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] AnimationCurve flareCurve;

    IEnumerator currentRoutine; 
    
    public void Flare(Color color)
    {
        if(currentRoutine != null)
            StopCoroutine(currentRoutine);

        StartCoroutine(currentRoutine = FlareRoutine(color));
    }

    IEnumerator FlareRoutine(Color color)
    {
        image.color = color;
        float time = 0;

        float duration = flareCurve.keys[flareCurve.length - 1].time;

        while (time < duration)
        {
            yield return null;
            time += Time.deltaTime;
            color.a = flareCurve.Evaluate(time);
            image.color = color;
        }
        
        color.a = flareCurve.Evaluate(duration);
        image.color = color;
        currentRoutine = null;
    }
}
