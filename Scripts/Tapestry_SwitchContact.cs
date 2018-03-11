using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_SwitchContact : Tapestry_Switch {
    
    private List<Collider> touching = new List<Collider>();

    protected override void Reset()
    {
        base.Reset();
    }

    protected override void Update()
    {

        base.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision has begun. Filters:" + keywords.Count);
        if(keywords.Count == 0)
        {
            touching.Add(other);

            if (!isOn && touching.Count > 0)
            {
                Debug.Log("Ping! I'm on.");
                SwitchOn();
            }
        }
        else
        {
            Tapestry_Activatable a = other.gameObject.GetComponentInParent<Tapestry_Activatable>();
            
            if (a != null)
            {
                bool keywordMatch = false;

                foreach (string kw in a.keywords)
                {
                    if (keywords.Contains(kw))
                    {
                        keywordMatch = true;
                        touching.Add(other);
                        break;
                    }
                }

                if (keywordMatch)
                {
                    if (!isOn)
                        SwitchOn();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Collision has ended.");
        if (keywords.Count == 0)
        {
            touching.Remove(other);

            if (isOn && touching.Count == 0)
            {
                Debug.Log("Zorp. I'm off.");
                SwitchOff();
            }
        }
        else
        {
            if(touching.Contains(other))
            {
                touching.Remove(other);
            }

            if (isOn && touching.Count == 0)
                SwitchOff();
        }
    }
}
