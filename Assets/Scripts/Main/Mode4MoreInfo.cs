using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Mode4MoreInfo : ModeBase
{
    public GameObject pointPrefab;
    public Transform pointParent;
    public GameObject pointPanel;
    public ToggleGroup pointGroup;
    

    //    public Transform imagePanel;
    //  public Text headerText;

    [HideInInspector] AtomDetail[] unit;
  //  List<Sprite> images = new List<Sprite> ();
  //  List<string> imgPaths = new List<string> ();
    public ScrollRect scroller;
//    public Image imageThumb;
 //   public Text imageCaption;
//    [HideInInspector] public int totalPoint;
    [HideInInspector] public int idx = 0;
    [HideInInspector] public int idxImage = 0;

    [HideInInspector]
    public List<PointDriver> points = new List<PointDriver> ();


    public void SetUp (AtomDetail[] u, string head)
    {
        enabled = true;
        idx = 0;
        unit = u;
        GM.DespawnAllChildren(pointParent);
        //AllocateHead (unit.Length);
        for (int i = 0; i < unit.Length; i++) {
            var p = GM.Spawn(pointPrefab, pointParent).GC<PointDriver>();
            var un = unit [i];
            points.Add(p);
            p.SetUp(un.head, un.body);
            p.toggle.group = pointGroup;
            /*            if (i >= points.Count)
                            points.Add (((GameObject)Instantiate (pointPrefab, pointPanel, false))
                                .GetComponent<PointDriver> ().SetUp (i, this, pointGroup));
                        else
                            points [i].gameObject.SetActive (true);
                        points [i].SetUp (un.head, un.body);*/
        }
        //for (int i = unit.Length; i < points.Count; i++) {
          //  points [i].gameObject.SetActive (false);
        //}

  //      totalPoint = unit.Length;

        //if (unit.Length > 0)
          //  PointFeedback(0);
      //  headerText.text = head;
       // StartCoroutine (LoadAllImages ());

    }

   /* IEnumerator LoadAllImages ()
    {
        images.Clear ();
        imgPaths.Clear ();
        int i = 0, j = 0;
        while (i < unit.Length) {
            if (unit [i].images == null || unit [i].images.Length == 0) {
                i++;
                continue;
            }
            if (string.IsNullOrEmpty (unit [i].images [j].path)) {
                j++;
                if (j >= unit [i].images.Length) {
                    i++;
                    j = 0;
                }
                continue;
            }
            var l = Resources.LoadAsync<Sprite> (unit [i].images [j].path);
            yield return l;
            images.Add ((Sprite)l.asset);
            imgPaths.Add (unit [i].images [j].path);
            if(idx == i && idxImage == j)
            {
                imageThumb.sprite = (Sprite)l.asset;
                imageCaption.text = unit[i].images[j].caption;
                SwitchDetail(0);
            }
            j++;
            if (j >= unit [i].images.Length) {
                i++;
                j = 0;
            }
        }
    }

    void PointFeedback (int id)
    {
        PointFeedback (id, 0);
    }

    void PointFeedback (int id, int idImage)
    {
        idx = id;
        idxImage = idImage;
        for (int i = 0; i < points.Count; i++) {
            if (i != id)
                points [i].Expand (false);
        }
        points [idx].Expand(true);
        var po = unit [idx].images [idxImage];
        var y = imgPaths.FindIndex (x => x == po.path);
        Sprite img = null;
        if (y >= 0 && y < images.Count)
            img = images [y];
        StartCoroutine (AnimateImage (po.caption, img));
    }

    public void SwitchExpandAll (bool expandAll)
    {
        scroller.GetComponent<RectTransform> ().anchorMax = new Vector2 (expandAll ? 1 : 0.4f, 1);
        //canvas.scaleFactor = expandAll ? 1.1f : 0.8f;
        imagePanel.gameObject.SetActive (!expandAll);
        PointDriver.keepExpand = expandAll;
        for (int i = 0; i < points.Count; i++) {
            points [i].Expand (expandAll ? false : i == idx);
        }
    }*/

    public void SwitchDetail (bool Next)
    {
        idxImage += Next ? 1 : -1;
        if (idxImage < 0) {
            idx--;
            idx = (int)Mathf.Repeat (idx, points.Count);
            idxImage = unit [idx].images.Length - 1;
        } else if (idxImage >= unit [idx].images.Length) {
            idx++;
            idx = (int)Mathf.Repeat (idx, points.Count);
            idxImage = 0;
        }
       // PointFeedback (idx, idxImage);
        ScrollTo ((RectTransform)points [idx].transform);
    }

    public void SwitchDetail (int pos)
    {
        idx = pos;
        idx = (int)Mathf.Repeat (idx, points.Count);
        //PointFeedback (idx);
        ScrollTo ((RectTransform)points [idx].transform);
    }

    void ScrollTo (RectTransform selected)
    {
        // var SC = scroller;
        var CT = scroller.content;
        var SR = scroller.GetComponent<RectTransform> ();
        var ST = selected;

        float selectedPositionY = -(ST.anchoredPosition.y + SR.rect.height / 2);

      StartCoroutine(AnimateScroll(CT, Mathf.Clamp (selectedPositionY
        , 0, CT.rect.height - SR.rect.height)));
    }

    IEnumerator AnimateScroll (RectTransform CT, float target)
    {
        float cur = CT.anchoredPosition.y;
        float speed = 0f;
        do {
           cur = Mathf.SmoothDamp(cur, target, ref speed, 0.1f);
            CT.anchoredPosition = new Vector2 (CT.anchoredPosition.x, cur);
            yield return null;
        } while (Mathf.Abs(target - cur) > 0.1f);
    }
/*
    IEnumerator AnimateImage (string c, Sprite h)
    {
        if (!h || h == imageThumb.sprite)
            yield break;
        var m = Time.time; 
        var hasUpdated = false; 
        while (Time.time < m + 0.4f) {
            yield return null;
            var p = Mathf.Clamp01 ((Time.time - m) / 0.4f);
            imagePanel.eulerAngles = Vector3.up * 180 * Mathf.PingPong (p, 0.5f);
            if (!hasUpdated && p > 0.5f) {
                imageCaption.text = c;
                imageThumb.sprite = h;
                hasUpdated = true;
            }
        }
    }*/

    void Awake ()
    {
        pointPanel.SetActive (false);
    }
    bool keyReceivable = false;
    void OnEnable ()
    {
        pointPanel.SetActive (true);
         // Slide animate the gallery
        keyReceivable = false;
        StartCoroutine(.5f, (t) => {
            t = 1 - t;
            t = (t * t * t * t);
            pointPanel.transform.localScale 
                = new Vector3(1 - t, 1f, 1f);
            cam.SetCameraRectangleHorizon((1-t) * .4f, 1);
        });
        Invoke((i) => keyReceivable = true, 0.6f);
    }

    void OnDisable ()
    {
        if (pointPanel && GO) {
            //points[idx].Expand(false);

            StartCoroutine(.5f, (t) => {
            t = 1 - t; 
            t = (t * t * t * t);
            pointPanel.transform.localScale 
                = new Vector3(t, 1f, 1f);
            cam.SetCameraRectangleHorizon(t * .4f, 1);
            });
            Invoke((i) => pointPanel.SetActive(false), 0.6f);
        
            //images.Clear ();
         //   Resources.UnloadUnusedAssets ();
        }
    }
    

    public void HandleScroll (float vAxis)
    {
        scroller.content.anchoredPosition += Vector2.up * vAxis * -scroller.scrollSensitivity;
    }

    void Update ()
    {
        if (Input.anyKey && keyReceivable)
        {
            
            var v = Input.GetAxisRaw("Vertical");
            if (v != 0f) {
                HandleScroll(v);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                SwitchDetail(false);
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                SwitchDetail(true);
                
            
            if (Input.GetKey(KeyCode.Alpha1))
            {
                GC<Mode3Gallery>().SwitchTo(GC<Mode1Lookup>().curPlanet); //.SetUp(currentPlanet.points, string.Format("{0} itu...", currentPlanet.name));
                enabled = false;
            }
            if (Input.GetKey(KeyCode.Alpha2))
            {
                OnEscapeHit();
            }
        }
    }
    

    
    void OnEscapeHit ()
    {   
        if (!enabled)
            return;
        enabled = false;
        GetComponent<Mode1Lookup> ().enabled = true;
    }

    void OnLeftHit ()
    {
        if (enabled)
            SwitchDetail (false);
    }

    void OnRightHit ()
    {
        if (enabled)
            SwitchDetail (true);
    }
}

