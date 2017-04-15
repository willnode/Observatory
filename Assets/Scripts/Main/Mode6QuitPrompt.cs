using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class Mode6QuitPrompt : ModeBase
{

    public Grayscale effect;
    public GameObject mainPanel;
    float lastTimeScale;
    Vector3 lastCam;
    float loopY;
    void OnEnable ()
    {
        effect.enabled = true;
        effect.gradInterpol = 0;
        StartCoroutine (Fade2Prompt ());
        lastCam = cam.camRot;
        cam.userInteraction = false;
        mainPanel.SetActive(true);
        //GetComponent<SelectorController>().ActivateSubMenuMode(false);
        loopY = 100f;
    }

    void Update ()
    {
        loopY += Time.deltaTime * 10f;
        cam.camRot.x += 30f * Time.deltaTime; 
        cam.camRot.x = Mathf.Repeat (cam.camRot.x, 360);
        cam.camRot.y = Mathf.PingPong(loopY, 140f) - 70f;
        cam.camRot.z = lastCam.z * Mathf.Sqrt((cam.camRot.y + 90f) / 90f);
    }

   void OnEscapeHit ()
    {   
        if (enabled)
            Confirm(false);
    }


    void OnDisable ()
    {
        SolarOrbit.timeScale = lastTimeScale;
        effect.enabled = false;
        cam.camRot = lastCam;
        cam.userInteraction = true;
        GetComponent<Mode0Navigator> ().enabled = true;
        mainPanel.SetActive(false);
    }

    public void Confirm (bool quit)
    {
        if (quit)
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
        else
            enabled = false;
    }

    IEnumerator Fade2Prompt ()
    {
        lastTimeScale = SolarOrbit.timeScale;
        float refSpeed = 0, refSpeed2 = 0;
        do {
            SolarOrbit.timeScale = Mathf.SmoothDamp (SolarOrbit.timeScale, 0, ref refSpeed, .5f);
            effect.gradInterpol = Mathf.SmoothDamp (effect.gradInterpol, 1, ref refSpeed2, .3f);
            yield return null;
        } while (SolarOrbit.timeScale > 0.01f);
        SolarOrbit.timeScale = 0;
        effect.gradInterpol = 1;
    }


}
