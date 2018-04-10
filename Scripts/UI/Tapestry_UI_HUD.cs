using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tapestry_UI_HUD : MonoBehaviour {

    public Tapestry_Player player;
    public Text targetName;
    //public Text debugelement_time;
    public Tapestry_UI_Fader
        activateIndicator, pushIndicator, liftIndicator;
    public Image gaugeHealth, gaugeStamina;

    private void Start()
    {
        
    }

    private void Reset()
    {
        
    }

    private void Update()
    {
        if(Tapestry_WorldClock.isPaused)
        {
            targetName.gameObject.SetActive(false);
            if (!activateIndicator.readyToFadeIn)
                activateIndicator.FadeOut(true);
            if (!pushIndicator.readyToFadeIn)
                pushIndicator.FadeOut(true);
            if (!liftIndicator.readyToFadeIn)
                liftIndicator.FadeOut(true);
        }
        else if (player.objectInSights != null)
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

        gaugeHealth.fillAmount = (player.health / 1000f);
        gaugeStamina.fillAmount = (player.stamina / 1000f);
    }
}
