using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_EffectZone : MonoBehaviour {
    
    public Tapestry_Effect effect;
    public bool 
        removeEffectOnTriggerLeave = true,
        applyByKeyword;
    public Tapestry_KeywordRegistry keywords;

    private void Reset()
    {
        effect = (Tapestry_Effect)ScriptableObject.CreateInstance("Tapestry_Effect");
        keywords = (Tapestry_KeywordRegistry)ScriptableObject.CreateInstance("Tapestry_KeywordRegistry");
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (ReferenceEquals(keywords, null))
            keywords = (Tapestry_KeywordRegistry)ScriptableObject.CreateInstance("Tapestry_KeywordRegistry");

        Tapestry_Actor a = other.GetComponentInParent<Tapestry_Actor>();
        if (keywords.Count == 0)
        {
            if (a != null)
                a.AddEffect(effect.Clone());
        }
        else if (applyByKeyword)
        {
            if (a != null && !ReferenceEquals(a.keywords, null))
            {
                if (a.keywords.ContainsOne(keywords))
                    a.AddEffect(effect.Clone());
            }
        }
        else
        {
            if (a != null && !ReferenceEquals(a.keywords, null))
            {
                if (!a.keywords.ContainsAll(keywords))
                    a.AddEffect(effect.Clone());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (removeEffectOnTriggerLeave)
        {
            if (effect.duration != Tapestry_EffectBuilder_Duration.Instant)
            {
                Tapestry_Actor a = other.GetComponentInParent<Tapestry_Actor>();
                if (a != null)
                    a.RemoveEffect(effect);
            }
        }
    }
}
