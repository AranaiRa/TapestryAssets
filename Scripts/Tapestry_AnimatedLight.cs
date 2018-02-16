using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_AnimatedLight : Tapestry_Activatable {

    public GameObject
        particleSystemContainer;
    public ParticleSystem
        psys_in,
        psys_active,
        psys_out;
    public Light lightSource;
    public GameObject emissiveStatics;
    public bool
        lightIntensityJitter = true,
        lightPositionJitter = true,
        emissionJitter = true,
        timedLight = false,
        useInPsys   = true,
        useHoldPsys = true,
        useOutPsys  = true;
    public float
        lightIntensityBase = 1.0f,
        lightIntensityJitterAmount = 0.1f,
        lightJitterSpeed = 0.2f,
        positionJitterAmount = 0.1f,
        positionJitterSpeed = 0.2f,
        emissionMax = 1.0f,
        emissionMin = 0.8f,
        emissionJitterSpeed = 0.2f,
        transitionSpeed = 1.0f,
        timedDuration = 4.0f;
    public Vector2Int
        timedOn,
        timedOff;
    public Color
        emissionColor = Color.white;

    private bool
        isOn = true,
        isTurningOn  = false,
        isTurningOff = false;
    private float
        intensityJitterTime = 0f,
        intensityJitterTargetTime = 0f,
        intensityJitterTarget = 0f,
        intensityJitterLast = 0f,
        positionJitterTime = 0f,
        positionJitterTargetTime = 0f,
        emissionJitterTime = 0f,
        emissionJitterTargetTime = 0f,
        transitionTime = 0f;
    private Vector3
        lightPositionBase = Vector3.zero,
        lightPositionJitterTarget = Vector3.zero,
        lightPositionJitterLast = Vector3.zero;
    private Color
        emissionJitterTarget = Color.white,
        emissionJitterLast = Color.white;
    private Material mat;

    private void Reset()
    {
        intensityJitterLast = lightIntensityBase;

        bool
            hasParticleContainer = false,
            hasLight = false,
            hasEmissiveStatics = false;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "T_Particles")
            {
                hasParticleContainer = true;
                particleSystemContainer = transform.GetChild(i).gameObject;
                for (int j = 0; j < particleSystemContainer.transform.childCount; j++)
                {
                    if (transform.GetChild(j).name == "T_PS_In")
                    {
                        psys_in = particleSystemContainer.transform.GetChild(i).GetComponent<ParticleSystem>();
                    }
                    if (transform.GetChild(j).name == "T_PS_Active")
                    {
                        psys_active = particleSystemContainer.transform.GetChild(i).GetComponent<ParticleSystem>();
                    }
                    if (transform.GetChild(j).name == "T_PS_Out")
                    {
                        psys_out = particleSystemContainer.transform.GetChild(i).GetComponent<ParticleSystem>();
                    }
                }
            }
            if (transform.GetChild(i).name == "T_Light")
            {
                hasLight = true;
                lightSource = transform.GetChild(i).gameObject.GetComponent<Light>();
                if(lightSource == null)
                    lightSource = transform.GetChild(i).gameObject.AddComponent<Light>();
            }
            if (transform.GetChild(i).name == "T_EmissiveMesh")
            {
                hasEmissiveStatics = true;
                emissiveStatics = transform.GetChild(i).gameObject;
            }
        }

        if (!hasParticleContainer)
        {
            particleSystemContainer = new GameObject();
            particleSystemContainer.transform.SetParent(transform);
            particleSystemContainer.name = "T_Particles";

            psys_in = new GameObject().AddComponent<ParticleSystem>();
            psys_in.transform.SetParent(particleSystemContainer.transform);
            psys_in.name = "T_PS_In";

            psys_active = new GameObject().AddComponent<ParticleSystem>();
            psys_active.transform.SetParent(particleSystemContainer.transform);
            psys_active.name = "T_PS_Active";

            psys_out = new GameObject().AddComponent<ParticleSystem>();
            psys_out.transform.SetParent(particleSystemContainer.transform);
            psys_out.name = "T_PS_Out";
        }

        if(!hasLight)
        {
            lightSource = new GameObject().AddComponent<Light>();
            lightSource.transform.SetParent(transform);
            lightSource.name = "T_Light";
        }

        if (!hasEmissiveStatics)
        {
            emissiveStatics = new GameObject();
            emissiveStatics.transform.SetParent(transform);
            emissiveStatics.name = "T_EmissiveStatics";
        }
    }

    void Start()
    {
        intensityJitterLast = lightIntensityBase;
        lightPositionBase = lightSource.transform.localPosition;

        mat = emissiveStatics.GetComponentInChildren<MeshRenderer>().material;

        emissionJitterLast = emissionColor;
        emissionJitterTarget = emissionColor;

        psys_in.Stop();
        psys_out.Stop();
    }

    private void OnDestroy()
    {
        //clean up material instance created by emission jitter
        Destroy(mat);
    }

    // Update is called once per frame
    void Update () {
        if (isTurningOn)
        {
            transitionTime += Time.deltaTime;
            if (transitionTime >= transitionSpeed)
                transitionTime = transitionSpeed;

            float mix = transitionTime / transitionSpeed;

            lightSource.intensity = Mathf.Lerp(0, lightIntensityBase, mix);
            lightSource.transform.localPosition = Vector3.Lerp(lightSource.transform.localPosition, lightPositionBase, mix);

            emissiveStatics.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            emissiveStatics.GetComponent<Renderer>().material.SetColor("_EmissionColor",
                Color.Lerp(Color.black, emissionColor, mix)
                );

            if (transitionTime == transitionSpeed)
            {
                isTurningOn = false;
                isOn = true;
            }
        }
        else if (isTurningOff)
        {
            transitionTime += Time.deltaTime;
            if (transitionTime >= transitionSpeed)
                transitionTime = transitionSpeed;

            float mix = transitionTime / transitionSpeed;

            lightSource.intensity = Mathf.Lerp(lightIntensityBase, 0, mix);
            lightSource.transform.localPosition = Vector3.Lerp(lightPositionBase, lightSource.transform.localPosition, mix);

            emissiveStatics.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            emissiveStatics.GetComponent<Renderer>().material.SetColor("_EmissionColor",
                Color.Lerp(emissionColor, Color.black, mix)
                );

            if (transitionTime == transitionSpeed)
            {
                isTurningOff = false;
                isOn = false;
            }
        }
        else if(isOn)
        {
            if (lightIntensityJitter)
            {
                if (intensityJitterTime >= intensityJitterTargetTime)
                {
                    intensityJitterLast = lightSource.intensity;
                    intensityJitterTarget = lightIntensityBase + Random.Range(-lightIntensityJitterAmount, lightIntensityJitterAmount);
                    intensityJitterTime = 0;
                    intensityJitterTargetTime = Random.Range(lightJitterSpeed * 0.4f, lightJitterSpeed);
                }
                intensityJitterTime += Time.deltaTime * Tapestry_WorldClock.globalTimeFactor;
                float prog = intensityJitterTime / intensityJitterTargetTime;
                prog = Mathf.Clamp(prog, 0, 1);

                lightSource.intensity = Mathf.Lerp(intensityJitterLast, intensityJitterTarget, prog);
            }
            if (lightPositionJitter)
            {
                if (positionJitterTime >= positionJitterTargetTime)
                {
                    lightPositionJitterLast = lightSource.transform.localPosition;
                    Vector3 dir = new Vector3(
                            Random.Range(-1, 1),
                            Random.Range(-1, 1),
                            Random.Range(-1, 1)).normalized;
                    dir.x *= lightSource.transform.localScale.x;
                    dir.y *= lightSource.transform.localScale.y;
                    dir.z *= lightSource.transform.localScale.z;
                    lightPositionJitterTarget = lightPositionBase + dir * positionJitterAmount;
                    positionJitterTime = 0;
                    positionJitterTargetTime = Random.Range(lightJitterSpeed * 0.4f, lightJitterSpeed);
                }
                positionJitterTime += Time.deltaTime * Tapestry_WorldClock.globalTimeFactor;
                float prog = positionJitterTime / positionJitterTargetTime;
                prog = Mathf.Clamp(prog, 0, 1);

                lightSource.transform.localPosition = Vector3.Lerp(lightPositionJitterLast, lightPositionJitterTarget, prog);
            }
            if (emissionJitter)
            {
                mat = emissiveStatics.GetComponentInChildren<MeshRenderer>().material;

                if (mat.HasProperty("_EmissionColor"))
                {
                    if (emissionJitterTime >= emissionJitterTargetTime)
                    {
                        emissionJitterLast = emissionJitterTarget;
                        emissionJitterTarget = emissionColor * Random.Range(emissionMin, emissionMax);
                        emissionJitterTime = 0;
                        emissionJitterTargetTime = Random.Range(emissionJitterSpeed * 0.4f, emissionJitterSpeed);
                    }
                    emissionJitterTime += Time.deltaTime * Tapestry_WorldClock.globalTimeFactor;
                    float prog = emissionJitterTime / emissionJitterTargetTime;
                    prog = Mathf.Clamp(prog, 0, 1);

                    emissiveStatics.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                    emissiveStatics.GetComponent<Renderer>().material.SetColor("_EmissionColor",
                        Color.Lerp(emissionJitterLast, emissionJitterTarget, prog)
                        );
                }
            }
            if (timedLight)
            {
                Vector2Int clockTime = Tapestry_WorldClock.GetFormattedTime();
                if((clockTime.x == timedOff.x) && (clockTime.y == timedOff.y))
                {
                    TurnOff();
                }
            }
        }
        else
        {
            if (timedLight)
            {
                Vector2Int clockTime = Tapestry_WorldClock.GetFormattedTime();
                if ((clockTime.x == timedOn.x) && (clockTime.y == timedOn.y))
                {
                    TurnOn();
                }
            }
        }
    }

    public void TurnOn(bool instant = false)
    {
        if (!instant)
        {
            transitionTime = 0;
            isTurningOn = true;
            isTurningOff = false;

            psys_in.Play();
            psys_active.Play();
            psys_out.Stop();
        }
        else
        {
            
        }
    }

    public void TurnOff(bool instant = false)
    {
        if (!instant)
        {
            transitionTime = 0;
            isTurningOn = false;
            isTurningOff = true;

            psys_in.Stop();
            psys_active.Stop();
            psys_out.Play();
        }
        else
        {

        }
    }
}
