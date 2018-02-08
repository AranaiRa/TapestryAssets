using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tapestry_PlayerHUD : MonoBehaviour {

    public Tapestry_Player player;
    public Text targetName;

    private void Reset()
    {
        
    }

    private void Update()
    {
        if(player.objectInSights != null)
        {
            targetName.text = player.objectInSights.displayName;
            targetName.gameObject.SetActive(true);
        }
        else
        {
            targetName.gameObject.SetActive(false);
        }
    }
}
