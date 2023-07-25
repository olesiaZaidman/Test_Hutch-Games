using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    private Light dirlight;

    void Start()
    {
        CreateLightInScene();
    }
   
    void CreateLightInScene()
    {
        dirlight = new GameObject("Light").AddComponent<Light>();
        dirlight.type = LightType.Directional;
      // SetLightColor(dirlight, Color.green);
    }

    void SetLightColor(Light light, Color color)
    {
        if (light != null)
        {
              light.color = color;
        }
    }
}
