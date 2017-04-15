using UnityEngine;

using UnityEngine.UI;

public class Mode3Gallery : ModeBase
{
    public GameObject galleryPanel;
    public Transform galleryParent;
    public GameObject galleryPrefab;
    public ScrollRect galleryRect;
    public ToggleGroup galleryGroup;
    public Text galleryText;
    public Text contentTitle;
    public Text contentBody;

    PlanetDetail currentPlanet;

    bool keyReceivable = false;

    public void SwitchTo (SolarOrbit now)
    {
        enabled = true;
        //PaintStats (stat.GetPlanet (now.id));
        RespawnGalleries(currentPlanet = detail.GetPlanet(now.id));
    }
    
    void Awake ()
    {
        galleryPanel.SetActive(false);
    }
    
    void OnEnable ()
    {
        galleryPanel.SetActive(true);
        // Slide animate the gallery
        keyReceivable = false;
        StartCoroutine(.5f, (t) => {
            t = 1 - t;
            t = t * t * t * t;
            galleryPanel.transform.localScale 
                = new Vector3(1 - t, 1f, 1f);
            cam.SetCameraRectangleHorizon(0, .6f + t * .4f);
       });
        Invoke((i) => keyReceivable = true, 0.6f);
    }

    void OnDisable ()
    {
        if (galleryPanel)
        {
            StartCoroutine(.5f, (t) => {
            t = 1 - t; 
            t = t * t * t * t;
            galleryPanel.transform.localScale 
                = new Vector3(t, 1f, 1f);
            cam.SetCameraRectangleHorizon(0, .6f + (1-t) * .4f);
           });
            Invoke((i) => galleryPanel.SetActive(false), 0.6f);
        }
    }

    void OnEscapeHit ()
    {   
        if (!enabled)
            return;
        enabled = false;
        GC<Mode1Lookup> ().enabled = true;
    }

    void RespawnGalleries (PlanetDetail data) {
        GM.DespawnAllChildren(galleryParent);
        for (int i = 0; i < data.points.Length; i++)
        {
            var p = data.points[i];
            for (int j = 0; j < p.images.Length; j++)
            {
                var m = p.images[j];
                var gal = GM.Spawn<GalleryButton>(galleryPrefab, galleryParent);
                gal.Init(m.path, p.head, p.body, m.caption, galleryGroup);
            }
        }
    }
    
    static Vector3[] worldCorners = new Vector3[4];
    
    public void RefreshTextDesc () {
        //  var y = galleryRect.verticalNormalizedPosition;
        //  var offset = galleryRect.GC<RectTransform>().rect.height * 1f;
        var b = GetScreenCoordinates(galleryPanel.GC<RectTransform>());

        var screenP = new Vector2(b.center.x, Mathf.Lerp(b.yMin, b.yMax, 0.75f));
        for (int i = 0; i < galleryParent.childCount; i++)
        {
            var r = GetScreenCoordinates(galleryParent.GetChild(i).GC<RectTransform>());
            r.xMin -= 10000;
            r.xMax += 10000;
            // if (i == 2)
            //        Debug.LogFormat("{0}-{1}: {2}:: {3}", r.min, r.max, screenP, r.Contains(screenP));
            //  var off = -offset - r.anchoredPosition.y;
            if (r.Contains(screenP)) {
                var gal = galleryParent.GetChild(i).GC<GalleryButton>();
                gal.toggle.isOn = true;
                SetGalleryText(i);
           //       Debug.Log(screenP);
          break;
            }
        }
    }
    
    static Rect GetScreenCoordinates(RectTransform uiElement)
     {
       uiElement.GetWorldCorners(worldCorners);
       var result = new Rect(
                     worldCorners[0].x,
                     worldCorners[0].y,
                     worldCorners[2].x - worldCorners[0].x,
                     worldCorners[2].y - worldCorners[0].y);
        //   var canvas = uiElement.GetComponentInParent<Canvas>();
        return result;// new Rect(result.center *;
     }

    int selGallery = 0;
    public void SetGalleryText (int idx) {
        selGallery = idx;
        UpdateGalleryText();
    }

    public void SetGalleryText (GalleryButton but) {
        selGallery = but.transform.GetSiblingIndex();
        UpdateGalleryText();
    }

    void UpdateGalleryText () {
        var gal = galleryParent.GetChild(selGallery).GC<GalleryButton>();
        galleryText.text = gal.detail;
        contentTitle.text = gal.head;
        contentBody.text = gal.body;
    }
    
    
    public void HandleScroll (float vAxis)
    {
       galleryRect.content.anchoredPosition += Vector2.up * vAxis * -galleryRect.scrollSensitivity;
    }
    
    void Update () {
        if (Input.anyKey && keyReceivable) {
        var v = Input.GetAxis("Vertical");
            if (v != 0f) {
                HandleScroll(v);
               RefreshTextDesc(); 
            }
         
            if (Input.GetKey(KeyCode.Alpha1))
            {
                OnEscapeHit();
            }
            if (Input.GetKey(KeyCode.Alpha2))
            {
                GC<Mode4MoreInfo> ().SetUp (currentPlanet.points, string.Format ("{0} itu...", currentPlanet.name));
                enabled = false;
            }
        }
    }
    
    
    

    /*void PaintStats (PlanetData data)
    {
        var ph = data.physical;
        statTxt [0].text = cStr (ph.radius) + " km";
        statTxt [1].text = cStr (ph.mass, ph.mass_) + " kg";
        statTxt [2].text = cStr (ph.surface, ph.surface_) + " km²";
        statTxt [3].text = cStr (ph.volume, ph.volume_) + " km³";
        statTxt [4].text = cStr (ph.density) + " g/cm³";
        statTxt [5].text = cStr (ph.gravity) + " m/s²";
        statTxt [6].text = cStr (ph.escape) + " m/s";
        statTxt [7].text = cStr (ph.temp) + " K";

        var po = data.orbital;
        statTxt [8].text = cStr (po.rotation) + " Hari";
        statTxt [9].text = cStr (po.periode) + " Tahun";
        statTxt [10].text = cStr (po.speed) + " km/s";
        statTxt [11].text = cStr (po.average, po.distance_) + " km";
        statTxt [12].text = cStr (po.aphelion, po.distance_) + " km";
        statTxt [13].text = cStr (po.perihelion, po.distance_) + " km";
        statTxt [14].text = cStr (po.tilt) + "⁰";
        statTxt [15].text = cStr (po.inclination) + "⁰";
        statTxt [16].text = cStr (po.eccentricity);

    }

    const string supers = "⁰¹²³⁴⁵⁶⁷⁸⁹";

    static string cStr (float w)
    {
        if (w == (float)((int)w))
            return w.ToString ("#,#0");
        else
            return w.ToString ("#,#0.00");
    }

    static string cStr (float w, int e)
    {
        string ss = " x 10";
        var s = e.ToString ();
        for (int i = 0; i < s.Length; i++) {
            ss += supers [(int)char.GetNumericValue (s [i])];
        }
        return cStr (w) + ss;
    }

*/

}

