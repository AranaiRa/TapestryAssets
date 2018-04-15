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
        if (isOn && touching.Count == 0 && !isSwitchingOff)
            SwitchOff();
        else if (!isOn && touching.Count > 0 && !isSwitchingOn)
            SwitchOn();
    }

    public override void Activate(Tapestry_Entity activatingEntity)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(keywords.Count == 0)
        {
            touching.Add(other);

            if (!isOn && touching.Count > 0)
                SwitchOn();
        }
        else if(!fireOnlyOnce || (fireOnlyOnce && !hasFired))
        {
            Tapestry_Activatable a = other.gameObject.GetComponentInParent<Tapestry_Activatable>();

            if (a != null)
            {
                if (a.keywords.ContainsOne(keywords))
                {
                    if (!isOn)
                        SwitchOn();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (keywords.Count == 0)
        {
            touching.Remove(other);

            if (isOn && touching.Count == 0)
                SwitchOff();
        }
        else if (!fireOnlyOnce || (fireOnlyOnce && !hasFired))
        {
            if (touching.Contains(other))
                touching.Remove(other);

            if (isOn && touching.Count == 0)
                SwitchOff();
        }
    }
}
