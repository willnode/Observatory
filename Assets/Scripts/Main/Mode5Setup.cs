using UnityEngine;

using UnityEngine.UI;

public class Mode5Setup : ModeBase
{

    public GameObject mainPanel;
    public RectTransform[] panels;
    [Space]
    public Toggle togMute;
    public Slider togScale;

    SceneKickstarter kicker;
    bool onLoad = false;
    int lastActive = -1;

    void Start ()
    {
    }

    void OnEnable ()
    {
        kicker = GetComponent<SceneKickstarter> ();
        kicker.cam.userInteraction = false;
        LoadTogglesData();
        mainPanel.SetActive(true);
        lastActive = -1;
        SwitchPanel (0);
    }

    void OnDisable ()
    {
        mainPanel.SetActive(false);
        GetComponent<Mode0Navigator>().enabled = true;
        kicker.cam.userInteraction = true;
        kicker.SaveSettings ();
    }

    public void LoadTogglesData ()
    {
        onLoad = true;
        togMute.isOn = kicker.mute;
        togScale.value = ToTogScale (kicker.zoomFactor);
        onLoad = false;
    }

    void OnEscapeHit ()
    {   
        if (enabled)
            Save();
    }

    public void Refresh ()
    {
        if(onLoad)
            return;
        kicker.mute = togMute.isOn;
        kicker.zoomFactor = FromTogScale (togScale.value);
        kicker.Refresh ();
    }

    public void Save ()
    {
        enabled = false;
    }

    public void SwitchPanel (int idx)
    {
        if (lastActive == idx)
            return;
        if (lastActive == -1) {
            for (int i = 0; i < panels.Length; i++) {
                panels [i].gameObject.SetActive (i == idx);
            }
            lastActive = idx;
        } else {
            var tO = panels[lastActive];
            var tN = panels[idx];
            tO.pivot = new Vector2(lastActive < idx ? 0 : 1, 0.5f);
            tN.pivot = new Vector2(lastActive < idx ? 1 : 0, 0.5f);
            tN.gameObject.SetActive(true);
            StartCoroutine(0.5f, (t) => {
                t = 1 - t;
                t = t * t * t * t;
                tN.localScale = new Vector3(1-t, 1, 1);
                tO.localScale = new Vector3(t, 1, 1);
            });
            Invoke((i) => {
                tO.gameObject.SetActive(false);
                lastActive = idx;
            }, .6f);
        }
    }

    float ToTogScale (float v)
    {
        return v * 5f - 2f;
    }

    float FromTogScale (float v)
    {
        return (v + 2f) / 5f;
    }
    
    public void OpenLink(string link) {
        Application.OpenURL(link);
    }
}
