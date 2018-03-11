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
        clockLight = false,
        useInPsys   = false,
        useHoldPsys = false,
        useOutPsys  = false,
        invertTimed = false,
        toggleOnActivate = false;
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
        timedOn = new Vector2Int(18,0),
        timedOff = new Vector2Int(6,0);
    public Color
        emissionColor = Color.white;
    public AudioSource
        emitter;
    public AudioClip
        inSound,
        activeSound,
        outSound;

    [SerializeField]
    private bool
        isOn = true;
    private bool
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
        transitionTime = 0f,
        timeUntilStateChange = -1f;
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
            hasEmissiveStatics = false,
            hasEmitter = false;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "T_Particles")
            {
                hasParticleContainer = true;
                particleSystemContainer = transform.GetChild(i).gameObject;
                for (int j = 0; j < particleSystemContainer.transform.childCount; j++)
                {
                    if (particleSystemContainer.transform.GetChild(j).name == "T_PS_In")
                    {
                        psys_in = particleSystemContainer.transform.GetChild(j).GetComponent<ParticleSystem>();
                    }
                    if (particleSystemContainer.transform.GetChild(j).name == "T_PS_Active")
                    {
                        psys_active = particleSystemContainer.transform.GetChild(j).GetComponent<ParticleSystem>();
                    }
                    if (particleSystemContainer.transform.GetChild(j).name == "T_PS_Out")
                    {
                        psys_out = particleSystemContainer.transform.GetChild(j).GetComponent<ParticleSystem>();
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
            if (transform.GetChild(i).name == "T_Emitter")
            {
                hasEmitter = true;
                emitter = transform.GetChild(i).gameObject.GetComponent<AudioSource>();
                if (emitter == null)
                    transform.GetChild(i).gameObject.AddComponent<AudioSource>();
            }
        }

        if (!hasParticleContainer)
        {
            particleSystemContainer = new GameObject();
            particleSystemContainer.transform.SetParent(transform);
            particleSystemContainer.transform.localPosition = Vector3.zero;
            particleSystemContainer.name = "T_Particles";

            psys_in = new GameObject().AddComponent<ParticleSystem>();
            psys_in.transform.SetParent(particleSystemContainer.transform);
            psys_in.transform.localPosition = Vector3.zero;
            psys_in.name = "T_PS_In";
            psys_in.Stop();

            psys_active = new GameObject().AddComponent<ParticleSystem>();
            psys_active.transform.SetParent(particleSystemContainer.transform);
            psys_active.transform.localPosition = Vector3.zero;
            psys_active.name = "T_PS_Active";
            psys_in.Stop();

            psys_out = new GameObject().AddComponent<ParticleSystem>();
            psys_out.transform.SetParent(particleSystemContainer.transform);
            psys_out.transform.localPosition = Vector3.zero;
            psys_out.name = "T_PS_Out";
            psys_in.Stop();
        }

        if(!hasLight)
        {
            lightSource = new GameObject().AddComponent<Light>();
            lightSource.transform.SetParent(transform);
            lightSource.transform.localPosition = Vector3.zero;
            lightSource.name = "T_Light";
            lightSource.shadows = LightShadows.Soft;
        }

        if (!hasEmissiveStatics)
        {
            emissiveStatics = new GameObject();
            emissiveStatics.transform.SetParent(transform);
            emissiveStatics.name = "T_EmissiveMesh";
            emissiveStatics.transform.localPosition = Vector3.zero;
        }

        if (!hasEmitter)
        {
            GameObject go = new GameObject();
            go.transform.SetParent(transform);
            go.name = "T_Emitter";
            go.AddComponent<AudioSource>();
            go.transform.localPosition = Vector3.zero;
            emitter = go.GetComponent<AudioSource>();
        }

        emitter.playOnAwake = false;
    }

    void Start()
    {
        intensityJitterLast = lightIntensityBase;
        lightPositionBase = lightSource.transform.localPosition;

        mat = emissiveStatics.GetComponentInChildren<MeshRenderer>().material;

        emissionJitterLast = emissionColor;
        emissionJitterTarget = emissionColor;

        if (isOn)
            TurnOn(true);
        else
            TurnOff(true);
    }

    private void OnDestroy()
    {
        //clean up material instance created by emission jitter
        Destroy(mat);
    }

    // Update is called once per frame
    void Update () {
        if (!useInPsys && psys_in.gameObject.activeInHierarchy)
            psys_in.gameObject.SetActive(false);
        else if (useInPsys && !psys_in.gameObject.activeInHierarchy)
            psys_in.gameObject.SetActive(true);

        if (!useHoldPsys && psys_active.gameObject.activeInHierarchy)
            psys_active.gameObject.SetActive(false);
        else if (useHoldPsys && !psys_active.gameObject.activeInHierarchy)
            psys_active.gameObject.SetActive(true);

        if (!useOutPsys && psys_out.gameObject.activeInHierarchy)
            psys_out.gameObject.SetActive(false);
        else if (useOutPsys && !psys_out.gameObject.activeSelf)
            psys_out.gameObject.SetActive(true);

        if (isTurningOn)
        {
            transitionTime += Time.deltaTime * Tapestry_WorldClock.GlobalTimeFactor;
            if (transitionTime >= transitionSpeed)
                transitionTime = transitionSpeed;

            float mix = transitionTime / transitionSpeed;

            lightSource.intensity = Mathf.Lerp(0, lightIntensityBase, mix);
            lightSource.transform.localPosition = Vector3.Lerp(lightSource.transform.localPosition, lightPositionBase, mix);

            if (mat != null)
            {
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor",
                    Color.Lerp(Color.black, emissionColor * emissionMin, mix)
                    );
            }

            if (transitionTime == transitionSpeed)
            {
                isTurningOn = false;
                isOn = true;
                if (activeSound != null)
                {
                    emitter.clip = activeSound;
                    emitter.volume = Tapestry_Config.SoundVolumeMaster * Tapestry_Config.SoundVolumeSFX;
                    emitter.loop = true;
                    emitter.Play();
                }
            }
        }
        else if (isTurningOff)
        {
            transitionTime += Time.deltaTime * Tapestry_WorldClock.GlobalTimeFactor;
            if (transitionTime >= transitionSpeed)
                transitionTime = transitionSpeed;

            float mix = transitionTime / transitionSpeed;

            lightSource.intensity = Mathf.Lerp(lightIntensityBase, 0, mix);
            lightSource.transform.localPosition = Vector3.Lerp(lightPositionBase, lightSource.transform.localPosition, mix);

            if (mat != null)
            {
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor",
                    Color.Lerp(emissionColor * emissionMin, Color.black, mix)
                    );
            }

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
                intensityJitterTime += Time.deltaTime * Tapestry_WorldClock.GlobalTimeFactor;
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
                positionJitterTime += Time.deltaTime * Tapestry_WorldClock.GlobalTimeFactor;
                float prog = positionJitterTime / positionJitterTargetTime;
                prog = Mathf.Clamp(prog, 0, 1);

                lightSource.transform.localPosition = Vector3.Lerp(lightPositionJitterLast, lightPositionJitterTarget, prog);
            }
            if (emissionJitter)
            {
                mat = emissiveStatics.transform.GetChild(0).GetComponent<MeshRenderer>().material;

                if (mat.HasProperty("_EmissionColor"))
                {
                    if (emissionJitterTime >= emissionJitterTargetTime)
                    {
                        emissionJitterLast = emissionJitterTarget;
                        emissionJitterTarget = emissionColor * Random.Range(emissionMin, emissionMax);
                        emissionJitterTime = 0;
                        emissionJitterTargetTime = Random.Range(emissionJitterSpeed * 0.4f, emissionJitterSpeed);
                    }
                    emissionJitterTime += Time.deltaTime * Tapestry_WorldClock.GlobalTimeFactor;
                    float prog = emissionJitterTime / emissionJitterTargetTime;
                    prog = Mathf.Clamp(prog, 0, 1);

                    mat.EnableKeyword("_EMISSION");
                    mat.SetColor("_EmissionColor",
                        Color.Lerp(emissionJitterLast, emissionJitterTarget, prog)
                        );
                }
            }
            if (clockLight)
            {
                bool h = (Tapestry_WorldClock.worldTime.Hour == timedOff.x);
                bool m = (Tapestry_WorldClock.worldTime.Minute == timedOff.y);

                if (h && m)
                {
                    TurnOff();
                }
            }
        }
        else
        {
            if (clockLight)
            {
                bool h = (Tapestry_WorldClock.worldTime.Hour == timedOff.x);
                bool m = (Tapestry_WorldClock.worldTime.Minute == timedOff.y);

                if (h && m)
                {
                    TurnOn();
                }
            }
        }

        if(timeUntilStateChange >= 0)
        {
            timeUntilStateChange -= Time.deltaTime * Tapestry_WorldClock.GlobalTimeFactor;

            if(timeUntilStateChange <= 0)
            {
                if(!invertTimed)
                    TurnOff();
                else
                    TurnOn();
            }
        }
    }

    public override void Activate(Tapestry_Entity activatingEntity)
    {
        if (toggleOnActivate)
        {
            if (!isTurningOn && !isTurningOff)
            {
                if (isOn)
                    TurnOff();
                else
                    TurnOn();
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
            
            if(useInPsys)
                psys_in.Play();
            if(useHoldPsys)
                psys_active.Play();
            if(useOutPsys)
                psys_out.Stop();
            
            if(timedLight && !invertTimed)
            {
                timeUntilStateChange = timedDuration + transitionSpeed;
            }

            if (inSound != null)
            {
                emitter.clip = inSound;
                emitter.volume = Tapestry_Config.SoundVolumeMaster * Tapestry_Config.SoundVolumeSFX;
                emitter.loop = false;
                emitter.Play();
            }
        }
        else
        {
            isOn = true;

            if(useInPsys)
                psys_in.Stop();
            if(useHoldPsys)
                psys_active.Play();
            if(useOutPsys)
                psys_out.Stop();

            lightSource.intensity = lightIntensityBase;

            if (emissiveStatics != null)
            {
                emissiveStatics.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial.EnableKeyword("_EMISSION");
                emissiveStatics.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial.SetColor("_EmissionColor", emissionColor);
            }
        }
    }

    public void TurnOff(bool instant = false)
    {
        if (!instant)
        {
            transitionTime = 0;
            isTurningOn = false;
            isTurningOff = true;

            if (useInPsys)
                psys_in.Stop();
            if (useInPsys)
                psys_active.Stop();
            if (useInPsys)
                psys_out.Play();
            
            if (timedLight && invertTimed)
            {
                timeUntilStateChange = timedDuration + transitionSpeed;
            }


            if (outSound != null)
            {
                emitter.clip = outSound;
                emitter.volume = Tapestry_Config.SoundVolumeMaster * Tapestry_Config.SoundVolumeSFX;
                emitter.loop = false;
                emitter.Play();
            }
        }
        else
        {
            isOn = false;

            if (useInPsys)
                psys_in.Stop();
            if (useHoldPsys)
                psys_active.Stop();
            if (useOutPsys)
                psys_out.Stop();

            lightSource.intensity = 0;

            if (emissiveStatics != null)
            {
                emissiveStatics.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial.EnableKeyword("_EMISSION");
                emissiveStatics.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial.SetColor("_EmissionColor", Color.black);
            }
        }
    }

    public bool GetIsOn()
    {
        return isOn;
    }
}
