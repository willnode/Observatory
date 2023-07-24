using UnityEngine;

// Handling Camera Movement
public class CamController : MonoBehaviour
{
    public float SwipeSpeed = 5f;
    // Pos: target to the planet
    internal Vector3 camPos;
    internal Vector3 refPos;
    internal Vector3 curPos;
    // Rot: rotation info
    internal Vector3 camRot;
    internal Vector3 curRot;
    internal Vector3 refRot;
    // FOV: zooming distance
    internal float camFOV = 70f;
    internal float curFOV = 50f;
    internal float refFOV;
    internal Vector2 camInput;
    internal float lockDampTmp = 0;
    internal float camPosTmp = 0;
//    internal float targetDistance = 0;
    public float speedDamp = 0.4f;
    public bool userInteraction = true;
    public bool lockOrientation = true;
    public Transform lockTarget;
    public Transform planetTarget;
    public Transform rootOfPlanet;
    public Transform lightTarget;
    internal Camera cam;

    internal float kMaxDist = 0.011f;

    public void SetCamDistance (float distance)
    {
  //      targetDistance = distance;
        camRot.z = distance;
    }
    
    public void SetCameraRectangle (Rect rect) {
        cam.rect = rect;
    }
    
    public void SetCameraRectangleHorizon (float min = 0, float max = 1) {
        SetCameraRectangle(Rect.MinMaxRect(min, 0, max, 1));
    }

    void UpdateClipping ()
    {
        float distance = curRot.z;
        cam.nearClipPlane = Mathf.Max (0.01f, distance / 1000f);
        float minFar = Mathf.Max (10000, (transform.position + rootOfPlanet.position).magnitude);
        cam.farClipPlane = Mathf.Clamp (distance * 3f, minFar, 1000000);
    }

    //float dt;
    //float down;

    void NormalizeRotation ()
    {
        camRot.x -= Mathf.Floor (camRot.x / 360) * 360;
        curRot.x -= Mathf.Floor (curRot.x / 360) * 360;

        camRot.x = curRot.x + Mathf.DeltaAngle (curRot.x, camRot.x);
    }

    public void SetCamDistance (SolarOrbit orbiter)
    {
        SetCamDistance (orbiter, false);
       
    }

    public void SetCamDistance (SolarOrbit orbiter, bool wholePlanet)
    {
//        var mult = wholePlanet ? 3f : 2f;
        var radius = wholePlanet ? orbiter.Par [8] : orbiter.Par [7];
        SetCamDistance (Mathf.Max (radius * 2.5f, radius + 0.05f));
        kMaxDist = 0.011f + orbiter.Par[7];
    }

    public void SetCamFOV (float fov)
    {
        camFOV = fov;
    }

    [Range(0.2f, 5f)]
    public float fovPow = 3;
    public void SetCamFOVNormalized (float fov)
    {
        if (fov <= 1)
            camFOV = Mathf.Pow (fov, fovPow) * 70f;
        else
            camFOV = 70 + (-Mathf.Pow(1/fov, fovPow)+1) * 100f;
    }

    public void SetUserInteraction (bool enable)
    {
        userInteraction = enable;
    }

    void OnEnable ()
    {
        cam = GetComponent<Camera> ();
    }

    public void SwitchLock (bool orientation)
    {
        lockOrientation = orientation;
    }

    public void SetPlanetTarget (Transform target)
    {
        planetTarget = target;
        if (target)
            curPos = transform.position - target.position;
        else
            curPos = transform.position;
        
        lightTarget.GetComponent<Light>().type = target ? LightType.Directional : LightType.Spot;
    }

    void Start ()
    {
        camRot.x = 359f;
        Application.targetFrameRate = 60;
    }

    static public readonly float preferedFOV = Mathf.Tan (35 * Mathf.Deg2Rad);

    // Returns the actual distance
    float InternalBalanceDistanceFOV (float distance, float fov)
    {
        float fovRad = fov / 2f * Mathf.Deg2Rad;
        float wideAngle = Mathf.Tan (fovRad) * distance;

        //If FOV is 70...
        float dist70 = wideAngle / (preferedFOV);

        if (dist70 < kMaxDist) {
            float tanFov = wideAngle / kMaxDist;
            cam.fieldOfView = Mathf.Atan (tanFov) * Mathf.Rad2Deg * 2;
            return kMaxDist / distance;
         } else {
            cam.fieldOfView = 70;
            if (fov <= 70f)
                return dist70 / distance;
            else
                return wideAngle / preferedFOV / distance;
        }
    }

    internal float camRotZMult;
    void LateUpdate ()
    {
        bool hasInput = false;
        if (userInteraction) {
            Vector2 d;
            hasInput = GetInput (ref camInput, out d);
            if (hasInput) {
                camRot += (Vector3)d * Time.unscaledDeltaTime * SwipeSpeed;// * Mathf.Max (Mathf.Pow (curFOV / 70f, 0.33f), 0.7f);
                camRot.y = Mathf.Clamp (camRot.y, -89.9f, 89.9f);
            }
        }
        NormalizeRotation ();
        //Apply FOV

        curFOV = Mathf.SmoothDamp(curFOV, planetTarget ? camFOV : 70, ref refFOV, speedDamp);

        camRotZMult = InternalBalanceDistanceFOV (camRot.z, curFOV);

        curRot = Vector3.SmoothDamp (curRot, camRot, ref refRot, speedDamp);

        //Apply Transform
    
        if (planetTarget) {
            curPos += camPos;
            camPos = Vector3.zero;
            curPos = Vector3.SmoothDamp (curPos, Vector3.zero, ref refPos, speedDamp / 2f);
            rootOfPlanet.position = -(rootOfPlanet.InverseTransformPoint (planetTarget.position));
            transform.localPosition = curPos + (Nav2Rot (curRot, camRotZMult));
            transform.localRotation = Quaternion.LookRotation (curPos + -transform.localPosition + planetTarget.position);
            if (lockOrientation && planetTarget) {
                float ldNow = VYAngle (lockTarget ? lockTarget.position : rootOfPlanet.position, planetTarget.position);
                camRot.x = Mathf.Repeat (camRot.x + Mathf.Repeat (ldNow - lockDampTmp, 360), 360);
                lockDampTmp = ldNow;
            }
            camPosTmp = 0;

            lightTarget.LookAt(planetTarget);
        } else {
            var delta = rootOfPlanet.position;
            rootOfPlanet.position = transform.forward * 0.1f;
            curPos -= delta - rootOfPlanet.position;
            curPos = Vector3.SmoothDamp (curPos, Vector2.zero, ref refPos, speedDamp);
            transform.position = curPos + Nav2Rot (curRot);
            transform.LookAt (curPos);
        }
        UpdateClipping ();
    }

    static public float VYAngle (Vector3 pivot, Vector3 v)
    {

        pivot.y = pivot.z;
        v.y = v.z;
        Vector2 d = v - pivot;
        if (d.x > 0)
            return Vector2.Angle (Vector2.up, d);
        else
            return 360 - Vector2.Angle (Vector2.up, d);
    }

    public Vector3 Nav2Rot (Vector3 nav)
    {
        return Quaternion.Euler (nav.y, nav.x, 0) * Vector3.forward * -(nav.z);
    }

    public float navMULT;
    public Vector3 Nav2Rot (Vector3 nav, float distMult)
    {
        navMULT = nav.z * distMult;
        return Quaternion.Euler (nav.y, nav.x, 0) * Vector3.forward * -(nav.z * distMult);
    }

    bool GetInput (ref Vector2 pos, out Vector2 delta)
    {
        Vector2 res = Vector2.zero;
        if (Input.mousePresent && Input.GetMouseButton (0)) {
            if (Input.GetMouseButtonDown (0))
                res = Vector2.zero;
            else if (Input.GetMouseButtonUp (0))
                res = Vector2.zero;
            else
                res = (Vector2)Input.mousePosition - pos;
            pos = Input.mousePosition;
        } else if (Input.touchCount > 0) {
            var t = Input.GetTouch (0);
            if (t.phase == TouchPhase.Began)
                res = Vector2.zero;
            else if (t.phase == TouchPhase.Canceled || t.phase == TouchPhase.Ended)
                res = Vector2.zero;
            else
                res = t.position - pos;
            pos = t.position;
        } else {
            res.x -= SwipeSpeed * Input.GetAxis ("Horizontal");
            res.y += SwipeSpeed * Input.GetAxis ("Vertical");
            if (Input.GetButton("Fire1"))
                res /= 10f;
        }

        delta = res;
        delta.x *= -2;
        return res != Vector2.zero;
    }


    [Space]
    public float minOff = 2f;
    public float maxOff = 1f;
    public LensFlare sunFlare;
    public float strength = 5f;
    public float realFlareModifer = 0.1f;
    internal bool onReal = false;

    void Update ()
    {
        ///Vector3 heading = sunFlare.transform.position - transform.position;
        float dist;
        if (planetTarget)
            dist = (planetTarget.position + rootOfPlanet.position).magnitude;
        else
            dist = transform.position.magnitude;
        if (dist > minOff)
            sunFlare.brightness = strength / Mathf.Pow (dist, .5f);
        else
            sunFlare.brightness = (strength / Mathf.Pow (dist, .5f)) * Mathf.InverseLerp (maxOff, minOff, dist);
        if (onReal)
            sunFlare.brightness /= realFlareModifer;
    }
}

