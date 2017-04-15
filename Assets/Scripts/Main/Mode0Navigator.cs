using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Mode0Navigator : ModeBase
{

    AudioSource music;
    public Slider zoomSlider;

    // Handled by Kickstarter
    [HideInInspector, SerializeField] public SolarSlot[] slots;
    [HideInInspector, SerializeField] public string[] timeTxt;
    [SerializeField] public int currentSlot = 0;
    [SerializeField] public float currentSlotFactor = 0;
    public SolarSlot slot { get { return slots[currentSlot];  } }
    
    public float ProcessedSlotFactor { get { return currentSlot / 5f + currentSlotFactor / 5f; }}
    
    void OnEnable ()
    {
        zoomSlider.value = ProcessedSlotFactor;
        cam = GM.Main<CamController>();
        music = GM.Main<AudioSource>();
        selector = GC<SelectorController> ();
        cam.SetPlanetTarget (null);
        Invoke ((i) => Log(slot.caption), .1f);
    }

    public void ReceivePinEvent (SolarOrbit id)
    {
        enabled = false;
        SetAllSlotMode(SolarSlot.FadeMode.Lookup, SolarSlot.FadeMode.Lookup);
        GetComponent<Mode1Lookup> ().SwitchTo (id);
    }
    
    public void SetAllSlotMode (SolarSlot.FadeMode mode, SolarSlot.FadeMode current) {
        foreach (var s in slots)
        {
            s.Fade(s == slot ? current : mode);
        }
    }

    void SetTime (float tScale, float sDamp, float aPitch)
    {
        cam.speedDamp = sDamp;
        StopAllCoroutines ();
        StartCoroutine (AnimatePitch (aPitch));
        StartCoroutine (AnimateTimeScale (tScale));
    }

    IEnumerator AnimatePitch (float target)
    {
        float refSpeed = 0;
        do {
            music.pitch = Mathf.SmoothDamp (music.pitch, 
                target, ref refSpeed, 1f);
            yield return null;
        } while (Mathf.Abs (target - music.pitch) > 0.01f);
        music.pitch = target;
    }
    
    public void SetMusicMute (bool mute)
    {
        music.mute = mute;
    }

    IEnumerator AnimateTimeScale (float target)
    {
        float refSpeed = 0;
        do {
            SolarOrbit.timeScale = Mathf.SmoothDamp (
                SolarOrbit.timeScale, target, ref refSpeed, .5f);
            yield return null;
        } while (Mathf.Abs (target - 
            SolarOrbit.timeScale) > 0.01f);
        SolarOrbit.timeScale = target;
    }

    public Slider timeSlider;
    
    public void ScaleTime (bool next)
    {
        timeSlider.value = Mathf.Clamp(timeSlider.value + (next ? 1 : -1), 0, 9);
    }
    
    public void ScaleTime (float mode)
    {
        Log(timeTxt [(int)mode], true);

        switch ((int)mode) {
        case 0:
            SetTime (0f, 0.6f, 0f);
            break;
        case 1:
            SetTime (1f / 3600f, 0.5f, 0.75f);
            break;
        case 2:
            SetTime (1f / 60f, 0.45f, 0.9f);
            break;
        case 3:
            SetTime (1f, 0.4f, 1f);
            break;
        case 4:
            SetTime (24f, 0.35f, 1.1f);
            break;
        case 5:
            SetTime (168f, 0.3f, 1.2f);
            break;
        case 6:
            SetTime (720f, 0.25f, 1.3f);
            break;
        case 7:
            SetTime (4320f, 0.2f, 1.4f);
            break;
        case 8:
            SetTime (34560f, 0.15f, 1.5f);
            break;
        case 9:
            SetTime (345600f, 0.1f, 2f);
            break;
        }
    }
    
    void SetCameraDistance ()
    {
        var distMin = slot.camDistance;
        var distMax = currentSlot < 4 ? slots[currentSlot + 1].camDistance : distMin * 10;
        cam.SetCamDistance (Mathf.Lerp(distMin, distMax, currentSlotFactor));
    }

    public void SwitchSlot (bool Next)
    {
        slot.Fade (SolarSlot.FadeMode.Inactive);
        currentSlot = MathUtility.Mod(currentSlot + (Next ? 1 : -1), slots.Length);
        slot.Fade (SolarSlot.FadeMode.Visible);
        Log(slot.caption);
        SetCameraDistance();
        zoomSlider.value = ProcessedSlotFactor;
    }
   
    
    public void SwitchSlot (int now)
    {
        slot.Fade (SolarSlot.FadeMode.Inactive);
        currentSlot = now;
        slot.Fade (SolarSlot.FadeMode.Visible);
        Log(slot.caption);
        SetCameraDistance();
        zoomSlider.value = ProcessedSlotFactor;
    }
    
    public void SwitchSlotNormalized (float value) {
        if (!enabled || slots.Length < 5)
            return;
        value *= 5;
        var floor = Mathf.FloorToInt(value);
        currentSlotFactor = value - floor;
        if (currentSlot != floor) {
            SwitchSlot(Mathf.Min(floor, 4));
        } else
            SetCameraDistance();
    }

    public void SwitchTo ()
    {   
        enabled = true;
        cam.camRot = Vector3.right * 90;
        SetAllSlotMode(SolarSlot.FadeMode.Inactive, SolarSlot.FadeMode.Visible);
//slot.Fade (SolarSlot.FadeMode.Visible);
        SetCameraDistance();
    }

    public void OpenSettings ()
    {
        if (enabled)
        {
            enabled = false;
            GetComponent<Mode5Setup>().enabled = true;
        } else {
            GC<SelectorController>().BackFeedback();
        }
    }

    void OnDisable ()
    {
    }

    void OnEscapeHit ()
    {   
        if (!enabled)
            return;
        enabled = false;
        StartCoroutine(TriggerMode6());
    }

    IEnumerator TriggerMode6 ()
    {
        yield return null;
        GetComponent<Mode6QuitPrompt> ().enabled = true;
    }

    void OnLeftHit ()
    {
        if (enabled)
            SwitchSlot (false);
    }

    void OnRightHit ()
    {
        if (enabled)
            SwitchSlot (true);
    }

    void OnNumberHit (int n)
    {
        if (!enabled)
            return;
        var s = slot.buttons;
        n--;
        if (n >= 0 && s.Length > n)
            StartCoroutine (OnNumberHitYield (s [n].GetComponent<PinButton> ().pinData));
    }

    IEnumerator OnNumberHitYield (SolarOrbit o)
    {
        yield return null;
        ReceivePinEvent (o);
    }
}
