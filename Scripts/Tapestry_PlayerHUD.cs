using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tapestry_PlayerHUD : MonoBehaviour {

    public Tapestry_Player player;
    public Text targetName;
    public Text debugelement_time;

    private void Reset()
    {
        
    }

    private void Update()
    {
        if(player.objectInSights != null)
        {
            if (player.objectInSights.GetComponent<Tapestry_Activatable>().isInteractable ||
                player.objectInSights.GetComponent<Tapestry_Activatable>().displayNameWhenUnactivatable)
            {
                targetName.text = player.objectInSights.displayName;
                targetName.gameObject.SetActive(true);
            }
        }
        else
        {
            targetName.gameObject.SetActive(false);
        }

        debugelement_time.text = Tapestry_WorldClock.worldTime + "\n" + Tapestry_WorldClock.GetFormattedTime();
    }
}
