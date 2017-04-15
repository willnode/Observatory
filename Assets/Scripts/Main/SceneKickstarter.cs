using UnityEngine;
using UnityEngine.UI;

public class SceneKickstarter : MonoBehaviourEX
{

    public StatData data;
    public DataDetail detail;
    public CamController cam;
    [Space]
    public GameObject controllers;
    public SolarOrbit[] orbiter;
    public GameObject pinPrefab;
    public Transform pinParent;
    [Space]
   // public bool realScales = false;
    public bool mute = false;
    public float zoomFactor = 0.8f;
    // Use this for initialization
    
    void Awake ()
    {
        var comp = GetComponents<ModeBase>();
        var selector = GC<SelectorController>();
        var logger = GM.Main<Text>("Logger");
        foreach (var c in comp)
        {
            c.cam = cam;
            c.stat = data;
            c.detail = detail;
            c.selector = selector;
            c.logger = logger;
        }
    }
    void Start ()
    {
        LoadSettings();
        Refresh();

        var navigator = controllers.GC<Mode0Navigator> ();
        var lookup = controllers.GC<Mode1Lookup> ();
        //Fill params
        navigator.slots = new SolarSlot[detail.metadata.SlotCaptions.Length];
        navigator.timeTxt = detail.metadata.TimeScaleNames;
        cam.onReal = true;
       
       SolarSlot currentSlot = null;
        int currentSlotIdx = -1, currentSlotRem = 0;
    
        //Persiapkan setiap parameter orbit
        for (int i = 0; i < orbiter.Length; i++) {
            var o = orbiter [i];
            var p = data.GetPlanet (o.id);
            o.Par = new float[] {
                p.orbital.perihelion * Mathf.Pow (10, 
                    p.orbital.distance_ - 6),
                (p.orbital.tilt),
                (p.orbital.rotation) * 24f,
                (p.orbital.periode) * 8760f,
                (p.orbital.inclination),
                (p.orbital.eccentricity),
                (p.orbital.long_ascend),
                 p.physical.radius / 1000000f,
                //(p.physical.radius) / 2000f,
                0
            };
            o.Par [8] = o.Par [7] * 1.5f;
           /* if (p.id == "sun" && !realScales) {
                o.Par [7] /= 20f;
                o.Par [8] /= 20f;
            }*/
            for (int j = 0; j < o.moons.Length; j++) {
                var m = o.moons [j];
                var pm = p.GetMoon (m.id);
                m.Par = new float[] {
                    pm.perihelion * Mathf.Pow (10, pm.distance_ - 6),
                    // : pm.perihelion * Mathf.Pow (10, (pm.distance_
                   // - 4)) / 2) + o.Par [7] * (realScales ? 1 : 2),
                    (pm.inclination),
                    (pm.periode) * 24,
                    (pm.periode) * 24,
                    (pm.inclination),
                    (pm.eccentricity),
                    0,
                    (pm.radius) / (1000000f)
                };
                o.Par [8] = Mathf.Max (o.Par [8], m.Par [0]);
                m.mesh.localEulerAngles = Vector3.left * pm.inclination;
                m.mesh.localScale = Vector3.one * m.Par [7]; 
            }
            o.mesh.localScale = Vector3.one * (o.Par [7]); 
            o.mesh.localEulerAngles = Vector3.left * p.orbital.tilt;
            var pin = Instantiate (pinPrefab).GC<PinButton> ();
            var rend = o.GetComponent<SolarOrbitRenderer>();
            AllocateSlot (ref currentSlot, ref currentSlotIdx, ref currentSlotRem, navigator, lookup,
                (RectTransform)pin.transform, rend);
            pin.SetUp (navigator, o, p.physical.scheme, detail.GetPlanet (p).name);
            pin.target = o.transform;
            if (rend)
                rend.SetScheme(p.physical.scheme);
        }
        currentSlot.SetUp ();
        navigator.SwitchSlot (true);
    
    }

    public void LoadSettings ()
    {
       // realScales = PlayerPrefs.GetInt ("s_RealScale", 1) == 1;
        mute = PlayerPrefs.GetInt ("s_Mute", 0) == 1;
        zoomFactor = PlayerPrefs.GetFloat ("s_Zoom", 0.8f);
    }

    public void SaveSettings ()
    {
        PlayerPrefs.SetInt ("s_Mute", mute ? 1 : 0);
        PlayerPrefs.SetFloat ("s_Zoom", zoomFactor);

       /* if (PlayerPrefs.GetInt ("s_RealScale", 0) != (realScales ? 1 : 0)) {
            PlayerPrefs.SetInt ("s_RealScale", realScales ? 1 : 0);
            PlayerPrefs.Save();
            Debug.Log("RELOAD");
            UnityEngine.SceneManagement.SceneManager.LoadScene (0);
        } else*/ {
            PlayerPrefs.Save();
            Refresh();
        }
    }

    public void Refresh ()
    {
        GC<Mode0Navigator> ().SetMusicMute(mute);
        GM.Main<CanvasScaler>("UI").scaleFactor = zoomFactor;
        GM.Main<CanvasScaler>("World").scaleFactor = zoomFactor;
    }

    void AllocateSlot (ref SolarSlot slot, ref int idx, ref int seek, Mode0Navigator nav, Mode1Lookup lookup, RectTransform pin, SolarOrbitRenderer rend)
    {
        var meta = detail.metadata;
        if (slot == null || seek >= meta.SlotCounts [idx]) {
            if (slot)
                slot.SetUp ();
            slot = pinParent.gameObject.AddComponent<SolarSlot> ();
            idx++;
            seek = 0;
            var count = meta.SlotCounts [idx];
            slot.buttons = new RectTransform[count];
            slot.orbits = new SolarOrbitRenderer[count];
            slot.caption = meta.SlotCaptions [idx];
            slot.camDistance = meta.SlotDistances [idx];
            slot.satelliteButtons = new RectTransforms[count];
            nav.slots [idx] = slot;
        }
        slot.orbits [seek] = rend;
        slot.buttons [seek] = pin;


        slot.satelliteButtons [seek] = pin4Satellites (rend, lookup);
        pin.SetParent (pinParent, false);
        seek++;
    }

    RectTransforms pin4Satellites (SolarOrbitRenderer rend, Mode1Lookup lookup)
    {
        if (!rend)
            return new RectTransforms (0);
        var s = rend.GC<SolarOrbit> ();
        var r = new RectTransforms (s.moons.Length);
        for (int i = 0; i < r.Length; i++) {
            var pin = Instantiate (pinPrefab).GC<PinButton> ();
            var p = data.GetPlanet (s.id);
            var m = p.GetMoon (s.moons [i].id);
            pin.SetUp (lookup, s.moons [i], m.scheme, detail.GetPlanet (p).GetMoon (m.id).label);
            pin.transform.SetParent (pinParent, false);
            pin.target = s.moons [i].transform;
            r [i] = pin.GC<RectTransform> ();
            r [i].localScale = Vector3.zero;
            s.moons[i].GC<SolarOrbitRenderer>().SetScheme(m.scheme);
        }
        return r;
    }
}

