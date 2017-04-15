using UnityEngine;

/// SolarOrbit berfungsi menggerakkan planet/satelit
/// Selama simulasi berlangsung
public class SolarOrbit : MonoBehaviour
{

    public static Vector3 worldOffset;

    /// ID adalah identifikasi sebuah planet/satelit ke
    /// data internal, dimasukkan secara manual
    public string id = "";

    /// Mesh adalah objek yang bertugas menampilkan
    /// bentuk bundar planet, dimasukkan secara manual
    public Transform mesh;

    /// Moons adalah Daftar objek satelit pada setiap
    /// planet, dimasukkan secara manual
    public SolarOrbit[] moons;
    [HideInInspector]
    public bool isMoon;


    /// Par berisi variabel-variabel yang dibutuhkan
    /// untuk menjalankan sebuah simulasi planet.
    /// Urutannya: [0]Perisenter, [1]Kemiringan, [2]Lama Rotasi,
    /// [3]Lama Revolusi, [4]Inklinasi, [5]Eksentritas, 
    /// [6]Longitudinal Askending, dan [7]Radius Planet
    [HideInInspector]
    public float[] Par = new float[8];

    /// Variabel yang digunakan selama simulasi
    private float[] CosSinOmega = new float[2];
    private float k;
    private float time;

    #if UNITY_EDITOR
    //public SolarInfoDetail detail
    #endif

    public void Start ()
    {
        k = 1;
        k = Mathf.PI * 2 / ThetaInt (Par [3]);
        if (id != "moon")
            time = Random.Range (0, Par [3]);
        
        CosSinOmega [0] = Mathf.Cos (Par [6]);
        CosSinOmega [1] = Mathf.Sin (Par [6]);
        if (!mesh)
            mesh = transform;
    }

    public static float timeScale = 1;
    //[Range(0,1500)]
    //public float lastVel;
    void Update ()
    {
        time += Time.deltaTime * timeScale;
        if (time > Par [3])
            time = time % Par [3];
      //      var l = transform.localPosition;
        transform.localPosition = ParametricOrbit (ThetaInt (time));
        //if(Time.deltaTime > 0)
          //  lastVel = (transform.localPosition - l).magnitude / Time.deltaTime;
        if (Par [2] > 0)
            mesh.Rotate (Vector3.down * (360 * Time.deltaTime * timeScale / Par [2]), Space.Self);
    }

    public Vector3 GetPositionAfterTime (float t)
    {
        return ParametricOrbit (ThetaInt (time + t));
    }

    public float ThetaInt (float t)
    {
        float a = t * Mathf.PI * 2 / Par [3];
        var e = Par [5];
        return k * (e * (e * Mathf.Cos (a) + 4) * Mathf.Sin (a) + (e * e + 2) * a) / 2;
    }

    public Vector3 ParametricOrbit (float th)
    {
        //th += Mathf.PI;
        float Cost = Mathf.Cos (th);
        float Sint = Mathf.Sin (th);
        
        float x = (Par [0] * (1 + Par [5])) / (1 + Par [5] * Cost) * Cost;
        float z = (Par [0] * (1 + Par [5])) / (1 + Par [5] * Cost) * Sint;
        
        float xp = CosSinOmega [0] * x - CosSinOmega [1] * z;
        float yp = CosSinOmega [1] * x + CosSinOmega [0] * z;
        
        return Quaternion.Euler (-Par [4], Par[6], 0) * new Vector3 (xp, 0f, yp);
    }
}

