
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public static class InputUtility
{
    
    public static V GetValueOrDefault<K, V>(this Dictionary<K, V> dic, K key)
    {
        V ret;
        bool found = dic.TryGetValue(key, out ret);
        if (found)
            return ret;
        return default(V);
    }
    
    public static Rect AddBorder (this Rect r, RectOffset offset) {
        return offset.Add(r);
    }
    
    public static Vector2 GetMinimum (this LayoutElement LE) {
        return new Vector2(LE.minWidth, LE.minHeight);
    }
    
    public static Vector2 GetPrefered (this LayoutElement LE) {
        return new Vector2(LE.preferredWidth, LE.preferredHeight);
    }
    
    public static void SetMinimum (this LayoutElement LE, Vector2 newSize) {
        LE.minWidth = newSize.x;
        LE.minHeight = newSize.y;
    }
    
    public static void SetPrefered (this LayoutElement LE, Vector2 newSize) {
        LE.preferredWidth = newSize.x;
        LE.preferredHeight = newSize.y;
    }
    
    /// Get ratio between X (width) / Y (height)
    public static float GetAspect (this Vector2 V2) {
        return V2.x / V2.y;
    }
    
    public static float GetNearestDistanceWith (this Ray ray, Ray other) {
        
        Vector3 u = ray.GetPoint(1) - ray.GetPoint(0);
        Vector3 v = other.GetPoint(1) - other.GetPoint(0);
        Vector3 w = ray.GetPoint(0) - other.GetPoint(0);
        float a = Vector3.Dot(u, u);         // always >= 0
        float b = Vector3.Dot(u, v);
        float c = Vector3.Dot(v, v);         // always >= 0
        float d = Vector3.Dot(u, w);
        float e = Vector3.Dot(v, w);
        float D = a * c - b * b;        // always >= 0
        float sc, sN, sD = D;       // sc = sN / sD, default sD = D >= 0
        float tc, tN, tD = D;       // tc = tN / tD, default tD = D >= 0

        // compute the line parameters of the two closest points
        if (D < 1e-5f)
        { // the lines are almost parallel
            sN = 0f;         // force using point P0 on segment S1
            sD = 1f;         // to prevent possible division by 0f later
            tN = e;
            tD = c;
        }
        else
        {                 // get the closest points on the infinite lines
            sN = (b * e - c * d);
            tN = (a * e - b * d);
       }

       
        // finally do the division to get sc and tc
        sc = (Mathf.Abs(sN) < 1e-4f ? 0f : sN / sD);
        tc = (Mathf.Abs(tN) < 1e-4f ? 0f : tN / tD);

        // get the difference of the two closest points
        Vector3 dP = w + (sc * u) - (tc * v);  // =  S1(sc) - S2(tc)

        return (dP).magnitude;   // return the closest distance
    }
    
    /// <summary> Get Consistent Input between mouse and touch </summary>
    public static bool GetPointerInput (ref Vector2 position, out Vector2 delta, float keyboardAxis)
    {
        var res = Vector2.zero;
        if (Input.mousePresent && Input.GetMouseButton (0)) {
            if (Input.GetMouseButtonDown (0))
                res = Vector2.zero;
            else if (Input.GetMouseButtonUp (0))
                res = Vector2.zero;
            else
                res = (Vector2)Input.mousePosition - position;
            position = Input.mousePosition;
        } else if (Input.touchCount > 0) {
            var t = Input.GetTouch (0);
            if (t.phase == TouchPhase.Began)
                res = Vector2.zero;
            else if (t.phase == TouchPhase.Canceled || t.phase == TouchPhase.Ended)
                res = Vector2.zero;
            else
                res = t.position - position;
            position = t.position;
        } else {
            res.x -= keyboardAxis * Input.GetAxis ("Horizontal");
            res.y += keyboardAxis * Input.GetAxis ("Vertical");
        }

        delta = res;
        delta.x *= -2;
        return res != Vector2.zero;
    }
    
}

public static class MathUtility {
    public static int Mod (int a, int b) {
        var r = a % b;
        return r < 0 ? r + b : r;
    }
}