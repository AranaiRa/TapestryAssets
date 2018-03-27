using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tapestry_UI_HUD : MonoBehaviour {

    public Tapestry_Player player;
    public Text targetName;
    public Text debugelement_time;
    public Image InventoryPanel;
    public Tapestry_UI_Fader
        activateIndicator, pushIndicator, liftIndicator;

    private void Start()
    {
        
    }

    private void Reset()
    {
        
    }

    private void Update()
    {
        if (player.objectInSights != null)
        {
            if (player.objectInSights.GetComponent<Tapestry_Activatable>().isInteractable)
            {
                targetName.text = player.objectInSights.displayName;
                targetName.gameObject.SetActive(true);
                if(activateIndicator.readyToFadeIn)
                    activateIndicator.FadeIn();
            }
            else if (player.objectInSights.GetComponent<Tapestry_Activatable>().displayNameWhenUnactivatable)
            {
                targetName.text = player.objectInSights.displayName;
                targetName.gameObject.SetActive(true);
            }
            if (player.objectInSights.GetComponent<Tapestry_Activatable>().isPushable)
            {
                if(pushIndicator.readyToFadeIn)
                    pushIndicator.FadeIn();
            }
            if (player.objectInSights.GetComponent<Tapestry_Activatable>().isLiftable)
            {
                if(liftIndicator.readyToFadeIn)
                    liftIndicator.FadeIn();
            }
        }
        else
        {
            targetName.gameObject.SetActive(false);
            if(activateIndicator.readyToFadeOut)
                activateIndicator.FadeOut();
            if(pushIndicator.readyToFadeOut)
                pushIndicator.FadeOut();
            if(liftIndicator.readyToFadeOut)
                liftIndicator.FadeOut();
        }

        string h = Tapestry_WorldClock.worldTime.Hour.ToString();
        string m = Tapestry_WorldClock.worldTime.Minute.ToString();
        if (m.Length == 1)
            m = "0" + m;
        string s = Tapestry_WorldClock.worldTime.Second.ToString();
        if (s.Length == 1)
            s = "0" + s;
        debugelement_time.text = h+":"+m+":"+s;
    }
}
