using UnityEngine;
using UnityEngine.UI;

// Handler for Pin button of each planets
public class PinButton : MonoBehaviourEX
{   
    public Image thumbnail;
    public Text caption;
    public Transform target;
    [Space]
    public Mode0Navigator navigator;
    public Mode1Lookup lookup;
    public SolarOrbit pinData;
    public bool isNavigatorPin;
    	
    void LateUpdate ()
    {
        Vector3 n = Camera.main.WorldToScreenPoint (target.position);
        transform.position = (Vector2)n * (n.z > 0 ? 1 : -1);
    }

    public void SetUp (Mode0Navigator ev, SolarOrbit id, Color clr, string cap)
    {
        navigator = ev;
        isNavigatorPin = true;
        pinData = id;
        thumbnail.color = clr;
        caption.text = cap;
    }

    public void SetUp (Mode1Lookup ev, SolarOrbit id, Color clr, string cap)
    {
        lookup = ev;
        isNavigatorPin = false;
        pinData = id;
        thumbnail.color = clr;
        caption.text = cap;
    }

    public void SetUpInvisible(Mode0Navigator ev, SolarOrbit id)
    {
        navigator = ev;
        isNavigatorPin = true;
        pinData = id;
        GetComponent<RectTransform>().sizeDelta = Vector2.one * 110f;
        caption.enabled = false;
        thumbnail.enabled = false;
    }

    public void SetUpInvisible(Mode1Lookup ev, SolarOrbit id)
    {
        lookup = ev;
        isNavigatorPin = false;
        pinData = id;
        GetComponent<RectTransform>().sizeDelta = Vector2.one * 110f;
        caption.enabled = false;
        thumbnail.enabled = false;
    }

    public void ClickFeedback ()
    {
        if (isNavigatorPin)
            navigator.ReceivePinEvent (pinData);
        else
            lookup.ReceivePinEvent (pinData);
    }

}

