using UnityEngine;
using System;
using System.Collections;

public class SolarSlot : MonoBehaviour
{
    public SolarOrbitRenderer[] orbits;
    public RectTransform[] buttons;
    public RectTransforms[] satelliteButtons;
    public int satelliteHighlightedIdx = -1;
    public string caption;
    public float camDistance;

    public void Fade (FadeMode mode)
    {
        StartCoroutine (DoFading (mode));
    }

    public void Fade (FadeMode mode, GameObject planetLookedUp)
    {
        for (int i = 0; i < orbits.Length; i++) {
            if (!orbits[i] || orbits [i].gameObject == planetLookedUp) {
                satelliteHighlightedIdx = i;
                Fade (mode);
                return;
            }
        }
        Debug.Log ("EHEMMMMMMMMMMMMMMM");
        Fade (mode);
    }

    public enum FadeMode
    {
        Visible = 0,
        Inactive = 1,
        Lookup = 2,
        Hidden = 3
    }

    public void SetUp ()
    {
        for (int i = 0; i < buttons.Length; i++) {
            if (buttons [i])
                buttons [i].localScale = Vector3.zero;
        }
        for (int i = 0; i < orbits.Length; i++) {
            if (orbits [i])
                orbits [i].SetAlpha (0.2f, 0f);
        }
    }

    IEnumerator DoFading (FadeMode mode)
    {
        float m = Time.time, i = 0;

        float planetFrom = mode == FadeMode.Visible ? 0.2f : (mode == FadeMode.Inactive ? 1 : (mode == FadeMode.Lookup ? 1 : 0));
        float planetTo = mode == FadeMode.Visible ? 1f : (mode == FadeMode.Inactive ? 0.2f : (mode == FadeMode.Lookup ? 0.1f : 0));
        float moonFrom = mode == FadeMode.Visible ? 0f : (mode == FadeMode.Inactive ? 0.2f : (mode == FadeMode.Lookup ? 0.2f : 0.8f));
        float moonTo = mode == FadeMode.Visible ? 0.2f : (mode == FadeMode.Inactive ? 0f : (mode == FadeMode.Lookup ? 0.8f : 0.2f));
        float planetPinFrom = mode == FadeMode.Inactive ? 1 : 0;
        float planetPinTo = mode == FadeMode.Visible ? 1 : 0;
        float satellitePinFrom = mode == FadeMode.Hidden ? 1 : 0;
        float satellitePinTo = mode == FadeMode.Lookup ? 1 : 0;

        while (Time.time < m + 0.5f) {
            yield return null;

            i = Mathf.Clamp01 ((Time.time - m) / 0.5f);

            for (int j = 0; j < orbits.Length; j++) {
                if (orbits [j])
                    orbits [j].SetAlpha (Mathf.Lerp (planetFrom, planetTo, i), Mathf.Lerp (moonFrom, moonTo, i));
                buttons [j].localScale = Vector3.one * Mathf.Lerp (planetPinFrom, planetPinTo, i);
                if (satellitePinTo == 0)
                    for (int k = 0; k < satelliteButtons [j].Length; k++) {
                        satelliteButtons [j] [k].localScale = Vector3.one * Mathf.Lerp (satellitePinFrom, satellitePinTo, i);
                    }
            }
            if (satelliteHighlightedIdx >= 0 && satellitePinTo > 0)
                for (int k = 0; k < satelliteButtons [satelliteHighlightedIdx].Length; k++) {
                    satelliteButtons [satelliteHighlightedIdx] [k].localScale = Vector3.one * Mathf.Lerp (satellitePinFrom, satellitePinTo, i);
                }
        }
    }



    public void Return ()
    {
        for (int j = 0; j < orbits.Length; j++) {
            buttons [j].localScale = Vector3.one;
        }
    }
}

[Serializable]
public class RectTransforms
{
    public RectTransform[] values;
    public RectTransform this[int v]
    {
    get { return values[v]; }
        set { values[v] = value; }
    }

    public int Length
    {
        get { return values.Length; }
    }

    public RectTransforms(int count)
    {
        values = new RectTransform[count];
    }
}
