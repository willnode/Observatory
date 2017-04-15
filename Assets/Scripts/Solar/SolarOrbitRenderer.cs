using UnityEngine;

public class SolarOrbitRenderer : MonoBehaviourEX
{
    public Material mat;
    public Color color;
    SolarOrbit PO;
    SolarOrbitRenderer[] PORMoons;
    int lineLength;
    Vector3[] pos;
    bool isMoon;

    public void SetAlpha (float planet, float moon)
    {
        if (PORMoons != null) {
            for (int i = 0; i < PORMoons.Length; i++) {
                var por = PORMoons [i];
                if (por == this)
                    por.color.a = planet;
                else
                    por.color.a = moon;
            }
        } else
            color.a = planet;
    }
    
    public void SetScheme (Color clr) {
        color = clr;
        color.a = 0.5f;
    }

    void Start ()
    {
            
        PO = GC<SolarOrbit> ();
            
        lineLength = 180;
        isMoon = transform.parent == null ? false : transform.parent.position != Vector3.zero;
        PORMoons = GetComponentsInChildren<SolarOrbitRenderer> ();
        pos = new Vector3[lineLength];
        for (int i = 0; i < lineLength; i++)
            pos [i] = PO.ParametricOrbit (2 * Mathf.PI / (lineLength - 1) * i);
            
    }

    void OnEnable ()
    {
        Camera.onPostRender += DrawGL;
    }

    void OnDisable ()
    {
        Camera.onPostRender -= DrawGL;
    }

    void DrawGL (Camera cam)
    {
        if (isActiveAndEnabled) {
            mat.SetPass (0);
            GL.PushMatrix ();
            GL.MultMatrix (transform.parent.localToWorldMatrix);
            GL.Begin (GL.LINES);
            GL.Color (color);
        
            for (int i = 1; i < pos.Length; i++) {
                GL.Vertex3 (pos [i - 1].x, pos [i - 1].y, pos [i - 1].z);
                GL.Vertex3 (pos [i].x, pos [i].y, pos [i].z);
                if (isMoon)
                    i++;
            }

            GL.End ();
            GL.PopMatrix ();
            
        } 
    }
}


