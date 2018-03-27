using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class tempinventoryscript : MonoBehaviour {

    public Image highlight;
    public Text debug;
    public RectTransform scrollview;

    private void Start()
    {
        Debug.Log(highlight.rectTransform.position.ToString());
        Debug.Log(Screen.height);
    }

    // Update is called once per frame
    void Update () {
        string str;
        Vector3 mouse = Input.mousePosition;
        int resolutionTop = Screen.height - 30;
        int resolutionBottom = 30;
        int resolutionLeft = 30;
        int resolutionRight = Screen.width / 2;
        bool inside = false;
        str = "mx:" + mouse.x + "  my:" + mouse.y;
        if(mouse.x >= resolutionLeft && mouse.x <= resolutionRight)
        {
            if(mouse.y >= resolutionBottom && mouse.y <= resolutionTop)
            {
                str += "\n\n inside window";
                inside = true;
            }
        }
        if(inside)
        {
            highlight.gameObject.SetActive(true);
            float x = highlight.rectTransform.position.x;
            float y = mouse.y;
            if (y > Screen.height - 54)
                y = Screen.height - 54;
            else
            {
                y = Mathf.RoundToInt(mouse.y / 24) * 24;
            }
            highlight.rectTransform.position = new Vector3(x, y, 0);
        }
        else
        {
            highlight.gameObject.SetActive(false);
        }
        debug.text = str;
	}
}
