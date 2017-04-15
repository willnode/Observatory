using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Interactive Part for Xplainer
public class ImageRectScaler : MonoBehaviourEX, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform label;
    public RectTransform scroller;
    public ScrollRect scroll;
    public Mode4MoreInfo callback;

    //    RectTransform RP;
    Sprite lastS;
    public bool enlarged;
    float clickTime;
    Vector2 lastPos;

    public void OnPointerClick (PointerEventData ev)
    {
        if (Time.realtimeSinceStartup - clickTime < 0.3f) {
            enlarged = !enlarged;
            lastS = null;
        }
        clickTime = Time.realtimeSinceStartup; 
    }

    public void OnPointerDown (PointerEventData ev)
    {
        if (enlarged)
            return;
        lastPos = ev.position;
    }

    public void OnPointerUp (PointerEventData ev)
    {
        if (enlarged)
            return;
        Vector2 delta = ev.position - lastPos;
        if (Mathf.Abs (delta.x) < Mathf.Abs (delta.y) || delta.sqrMagnitude < 30)
            return;
        callback.Invoke (delta.x > 0 ? "OnLeftHit" : "OnRightHit", 0);
    }
    // Use this for initialization
    void Start ()
    {
        //IM = GetComponent<Image> ();
        //RT = GetComponent<RectTransform> ();
        //FT = GetComponent<AspectRatioFitter> ();
        //      RP = (RectTransform)transform.parent;
    }
    
    // Update is called once per frame
    void Update ()
    {
        if (lastS != IM.sprite) {
            lastS = IM.sprite;
            if (!lastS)
                return;
            scroller.sizeDelta = new Vector2 (scroller.sizeDelta.x, -label.sizeDelta.y);
            if (enlarged) {
                RT.anchorMax = (RT.anchorMin);
                RT.sizeDelta = IM.sprite.rect.size * 2;
                scroll.enabled = true;
            } else {
                RT.anchorMin = new Vector2 (0f, 0f);
                RT.anchorMax = new Vector2 (1f, 1f);
                ARF.aspectMode = AspectRatioFitter.AspectMode.None;
                RT.sizeDelta = Vector2.zero;
                RT.offsetMax = Vector2.zero;
                RT.offsetMin = Vector2.zero;
                scroll.enabled = false;
            }
        }
    }


}

