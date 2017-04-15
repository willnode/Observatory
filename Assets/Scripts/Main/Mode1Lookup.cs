using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Mode1Lookup : ModeBase
{
    internal SolarOrbit curPlanet;
    public Slider zoomSlider;
    public Image zoomIndicator;
    public GameObject lookupButtons;
    string planetName;
    float savedZoomValue = 1;


    void OnEnable ()
    {
       planetName = detail.GetPlanet (curPlanet.id).name;
        Log(planetName);
        zoomSlider.value = Mathf.Max(savedZoomValue, .6f);
        cam.SetCamDistance (curPlanet, true);
        cam.userInteraction = true;
        lookupButtons.SetActive(true);
        curPlanet.GC<SolarOrbitRenderer>().enabled = false;
        temporaryZoom = zoomSlider.normalizedValue;
    }

    void OnDisable ()
    {
        savedZoomValue = zoomSlider.value;
        lookupButtons.SetActive(false);
        curPlanet.GC<SolarOrbitRenderer>().enabled = true;
    }

    public void ReceivePinEvent (SolarOrbit id)
    {
        enabled = false;
        GC<Mode2ZoomInto> ().SwitchTo (id);
        planetName = detail.GetPlanet (curPlanet.id).GetMoon (id.id).label;
        Log(planetName);
    }

    public void ReceivePinEventWithIdx (int id)
    {
        if (id >= curPlanet.moons.Length)
            return;
        ReceivePinEvent (curPlanet.moons [id]);
    }

    public void SwitchTo (SolarOrbit now)
    {
        curPlanet = now;
        cam.SetPlanetTarget (curPlanet.transform);
        cam.camRot.x = CamController.VYAngle (Vector3.zero, curPlanet.transform.position);
        enabled = true;
    }

    public void SwitchTo ()
    {
        cam.SetPlanetTarget (curPlanet.transform);
        cam.SetCamDistance (curPlanet, true);
        enabled = true;
    }

    void OnEscapeHit ()
    {   
        if (!enabled)
            return;
        GetComponent<Mode0Navigator> ().SwitchTo ();
        enabled = false;
    }

    void UpdateAdvance ()
    {
        enabled = false;
//        selector.ActivateSubMenuMode (true);
        cam.userInteraction = false;
    }

    void UpdateAdvance (bool userInteract)
    {
        enabled = false;
  //      selector.ActivateSubMenuMode (true);
        cam.userInteraction = userInteract;
    }

    float lastZoomTime = 0;
    float temporaryZoom = 1;
    const string supers = "⁰¹²³⁴⁵⁶⁷⁸⁹";

    void Update ()
    {
        if (Input.anyKey) {
            temporaryZoom += SelectorController.GetAxis (KeyCode.PageUp, KeyCode.PageDown) * 0.4f;
            temporaryZoom = Mathf.Max(temporaryZoom, 0.01f);
            zoomSlider.normalizedValue = temporaryZoom;
            if (temporaryZoom > 1)
                cam.SetCamFOVNormalized(temporaryZoom);
        }
        if (lastZoomTime < 5) {
            lastZoomTime += Time.deltaTime;
            if (lastZoomTime >= 5f)
                Log(planetName);
           else
                NotifySliderChange (false);
        }
    }
    public void NotifySliderChange ()
    {
        if (zoomSlider.normalizedValue < .99f)
            temporaryZoom = zoomSlider.normalizedValue;
        if (enabled || GC<Mode2ZoomInto>().enabled)
            NotifySliderChange(true);
        
    }
    public void NotifySliderChange (bool renew)
    {
        if (cam.onReal) {
            var fov = cam.cam.fieldOfView;
            double dist = cam.camRotZMult * cam.camRot.z - (cam.kMaxDist - 0.011f);
            if (fov < 69.8f) {
                var wideF = Mathf.Tan (fov / 2f * Mathf.Deg2Rad) * dist;
                dist = wideF / CamController.preferedFOV;
            }
            // dist -=  / 2.0;
            string t = (dist * 1000000).ToString ("E1").Replace ("E+00", "×10");
            t = t.Substring (0, t.Length - 1) + supers [(int)char.GetNumericValue (t [t.Length - 1])];
            logger.text = (string.Format ("{0} ({1} km)", planetName, t));
            if (fov < 69.8f) {
                logger.text += string.Format (" ({0}°)", cam.cam.fieldOfView.ToString ("0.0"));
                zoomIndicator.color = Color.yellow;
            } else
                zoomIndicator.color = Color.white;
            if (renew)
                lastZoomTime = 0;
        } 
    }

    void OnNumberHit (int n)
    {
        if (!enabled)
            return;
        StartCoroutine (OnNumberHitYield (--n));
    }

    IEnumerator OnNumberHitYield (int n)
    {
        yield return null;
        if (n == 0)
            AdvanceToMode3_Stat ();
        else if (n == 1)
            AdvanceToMode4_Detail ();
        else
            ReceivePinEventWithIdx (n - 2);
    }


    public void AdvanceToMode3_Stat ()
    {
        UpdateAdvance ();
        GC<Mode3Gallery> ().SwitchTo (curPlanet);
    }


    public void AdvanceToMode4_Detail ()
    {
        UpdateAdvance ();
        var p = detail.GetPlanet (curPlanet.id);
        GC<Mode4MoreInfo> ().SetUp (p.points, string.Format ("{0} itu...", p.name));
    }
    /*
    public void AdvanceToMode5_Satellite ()
    {
        var p = detail.GetPlanet (curPlanet.id);
        GetComponent<Mode4MoreInfo> ().SetUp (p.moons, string.Format ("Satelit {0} diantaranya...", p.name));
        UpdateAdvance ();
    }

    public void AdvanceToMode6_Probes ()
    {
        var p = detail.GetPlanet (curPlanet.id);
        GetComponent<Mode4MoreInfo> ().SetUp (p.probes, string.Format ("{0} pada...", p.name));
        UpdateAdvance ();
    }*/

    /*public void AdvanceToMode7_Detail ()
    {
        GetComponent<Mode7Detail> ().SwitchTo (detail.GetPlanet (curPlanet.id));
        UpdateAdvance ();
    }*/
}

