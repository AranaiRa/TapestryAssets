using UnityEngine;

public class Tapestry_Level : MonoBehaviour {

    public bool isTimeFrozen = false;
    public Light sun;
    public Material sky;
    public Color tempColor;
    public Tapestry_UI_Inventory inventoryUI;

    private void Reset()
    {
        sky = RenderSettings.skybox;

        bool hasSun = false;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "T_Sun")
            {
                hasSun = true;
                sun = transform.GetChild(i).gameObject.GetComponent<Light>();
                if (sun == null)
                    transform.GetChild(i).gameObject.AddComponent<Light>();
            }
        }

        if (!hasSun)
        {
            GameObject go = new GameObject();
            go.transform.SetParent(transform);
            go.name = "T_Sun";
            go.transform.localPosition = Vector3.zero;
            go.transform.gameObject.AddComponent<Light>();
            sun = go.GetComponent<Light>();
            sun.color = Tapestry_Config.SunDayColor;
            sun.type = LightType.Directional;
            sun.lightmapBakeType = LightmapBakeType.Realtime;
            sun.intensity = Tapestry_Config.SunDirectLight;
            sun.bounceIntensity = Tapestry_Config.SunIndirectLight;
            sun.shadows = LightShadows.Soft;
        }
}

    // Update is called once per frame
    void Update () {
        if (!isTimeFrozen)
        {
            float dayProg = Tapestry_WorldClock.EvaluateTime(Time.deltaTime * Tapestry_Config.ClockProgressionFactor);
            sun.transform.rotation = Quaternion.Euler(dayProg * 360f - 90f, 0, 0);
        }
        sun.color = Tapestry_WorldClock.EvaluateColor();
        RenderSettings.ambientSkyColor = Tapestry_WorldClock.EvaluateAmbientColor();
    }
}
