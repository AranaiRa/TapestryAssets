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

    private float
        healthLastFrame = 1.0f, staminaLastFrame = 1.0f,
        timeSinceHealthChange = Tapestry_Config.GaugeFadeDelay, timeSinceStaminaChange = Tapestry_Config.GaugeFadeDelay;
    private bool
        healthIsVisible = false,
        staminaIsVisible = false;

    private void Start()
    {
        healthIsVisible = true;
    }

    private void Reset()
    {
        
    }

    private void Update()
    {
        if(Tapestry_WorldClock.IsPaused)
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

        if(healthIsVisible)
        {
            if(healthLastFrame != 1.0f && gaugeHealth.fillAmount == 1.0f)
            {
                timeSinceHealthChange = 3.0f;
            }
        }
        else
        {
            if(healthLastFrame != gaugeHealth.fillAmount)
            {
                healthIsVisible = true;
                gaugeHealth.CrossFadeAlpha(1, Tapestry_Config.GaugeFadeTime, true);
            }
        }
        if (staminaIsVisible)
        {
            if (staminaLastFrame != 1.0f && gaugeStamina.fillAmount == 1.0f)
            {
                timeSinceStaminaChange = 3.0f;
            }
        }
        else
        {
            if (staminaLastFrame != gaugeStamina.fillAmount)
            {
                staminaIsVisible = true;
                gaugeStamina.CrossFadeAlpha(1, Tapestry_Config.GaugeFadeTime, true);
            }
        }

        //end of frame
        timeSinceHealthChange -= Time.deltaTime;
        timeSinceStaminaChange -= Time.deltaTime;
        if (timeSinceHealthChange <= 0)
        {
            timeSinceHealthChange = 0;
            if (gaugeHealth.fillAmount == 1.0f)
            {
                healthIsVisible = false;
                gaugeHealth.CrossFadeAlpha(0, Tapestry_Config.GaugeFadeTime, true);
            }
        }
        if (timeSinceStaminaChange <= 0)
        {
            timeSinceStaminaChange = 0;
            if (gaugeStamina.fillAmount == 1.0f)
            {
                staminaIsVisible = false;
                gaugeStamina.CrossFadeAlpha(0, Tapestry_Config.GaugeFadeTime, true);
            }
        }

        healthLastFrame = gaugeHealth.fillAmount;
        staminaLastFrame = gaugeStamina.fillAmount;
    }
}
