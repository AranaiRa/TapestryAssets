using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tapestry_UI_Fader : MonoBehaviour {

    public bool
        readyToFadeOut = true,
        readyToFadeIn = false;
    bool
        isFadingOut = false,
        isFadingIn = false;
    static float fadeTime = 0.1f;
    float time;

	// Use this for initialization
	void Start () {
        FadeOut(true);
	}
	
	// Update is called once per frame
	void Update () {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            if (isFadingIn)
            {
                isFadingIn = false;
                readyToFadeOut = true;
            }
            if (isFadingOut)
            {
                isFadingOut = false;
                readyToFadeIn = true;
            }
            time = 0;
        }
    }

    public void FadeIn(bool instant = false)
    {
        if (instant)
        {
            readyToFadeOut = true;
            foreach (Image i in gameObject.transform.GetComponentsInChildren<Image>())
            {
                i.CrossFadeAlpha(1, 0, true);
            }
            foreach (Text t in gameObject.transform.GetComponentsInChildren<Text>())
            {
                t.CrossFadeAlpha(1, 0, true);
            }
        }
        else if (!isFadingIn && !isFadingOut)
        {
            readyToFadeIn = false;
            isFadingIn = true;
            time = fadeTime;
            foreach (Image i in gameObject.transform.GetComponentsInChildren<Image>())
            {
                i.CrossFadeAlpha(1, fadeTime, true);
            }
            foreach (Text t in gameObject.transform.GetComponentsInChildren<Text>())
            {
                t.CrossFadeAlpha(1, fadeTime, true);
            }
        }
    }

    public void FadeOut(bool instant = false)
    {
        if (instant)
        {
            readyToFadeIn = true;
            foreach (Image i in gameObject.transform.GetComponentsInChildren<Image>())
            {
                i.CrossFadeAlpha(0, 0, true);
            }
            foreach (Text t in gameObject.transform.GetComponentsInChildren<Text>())
            {
                t.CrossFadeAlpha(0, 0, true);
            }
        }
        else if (!isFadingIn && !isFadingOut)
        {
            readyToFadeOut = false;
            isFadingOut = true;
            time = fadeTime;
            foreach (Image i in gameObject.transform.GetComponentsInChildren<Image>())
            {
                i.CrossFadeAlpha(0, fadeTime, true);
            }
            foreach (Text t in gameObject.transform.GetComponentsInChildren<Text>())
            {
                t.CrossFadeAlpha(0, fadeTime, true);
            }
        }
    }
}
