using UnityEngine;
using UnityEngine.UI;
public class Mode2ZoomInto : ModeBase
{
    SolarOrbit curMoon;
    //public GameObject lookupPanel;
    public Slider zoomSlider;

    public void SwitchTo (SolarOrbit now)
    {
        curMoon = now;
        enabled = true;
        GC<Mode0Navigator> ().slot.Fade (SolarSlot.FadeMode.Hidden);  
    }

    void OnEnable ()
    {
        //   lookupPanel.SetActive (true);
        cam.SetPlanetTarget (curMoon.transform);
        cam.SetCamDistance (curMoon, false);
        cam.lockTarget = curMoon.transform.parent;
        cam.camRot.x = CamController.VYAngle (Vector3.zero, curMoon.transform.position) - 90;
    }

    void OnDisable ()
    {
        cam.lockTarget = null;
        //  if (lookupPanel)
        //    lookupPanel.SetActive (false);
    }

    void Update ()
    {
        if (Input.anyKey)
            zoomSlider.normalizedValue += SelectorController.GetAxis (KeyCode.PageUp, KeyCode.PageDown) * 0.5f;
    }

    void OnEscapeHit ()
    {   
        if (!enabled)
            return;
        enabled = false;
        GetComponent<Mode1Lookup> ().SwitchTo ();
        var nav = GetComponent<Mode0Navigator> ();
        nav.slots [nav.currentSlot].Fade (SolarSlot.FadeMode.Lookup);
    }

}

