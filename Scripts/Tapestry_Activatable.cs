using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tapestry_Activatable : MonoBehaviour {

    public string 
        displayName = "";
    public bool 
        isInteractable = true,
        isPushable = false,
        isLiftable = false,
        displayNameWhenUnactivatable = false;
    public List<string> keywords;
    public AudioSource
        emitter;

    protected virtual void Reset()
    {
        bool
            hasEmitter = false;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "T_Emitter")
            {
                hasEmitter = true;
                emitter = transform.GetChild(i).gameObject.GetComponent<AudioSource>();
                if (emitter == null)
                    emitter = transform.GetChild(i).gameObject.AddComponent<AudioSource>();
            }
        }

        if (!hasEmitter)
        {
            GameObject go = new GameObject();
            go.transform.SetParent(transform);
            go.name = "T_Emitter";
            go.AddComponent<AudioSource>();
            go.transform.localPosition = Vector3.zero;
            emitter = go.GetComponent<AudioSource>();
            emitter.maxDistance = Tapestry_Config.DefaultSoundRadius;
        }

        emitter.playOnAwake = false;
    }

    public virtual void Activate(Tapestry_Entity activatingEntity)
    {

    }

    public virtual void Push(Tapestry_Entity activatingEntity)
    {

    }

    public virtual void Lift(Tapestry_Entity activatingEntity)
    {

    }

    public virtual void Hover()
    {

    }
}
