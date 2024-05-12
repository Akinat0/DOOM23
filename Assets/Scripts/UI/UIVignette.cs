using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIVignette : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] AnimationCurve flareCurve;

    double flareTime = double.MinValue;
    
    public void Flare(Color color)
    {
        image.color = color;
        flareTime = Time.timeAsDouble;
        Refresh();
    }

    void Update()
    {
        Refresh();
    }

    void Refresh()
    {
        float animationTime = (float)(Time.timeAsDouble - flareTime);
        float alpha = flareCurve.Evaluate(animationTime);
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
}
