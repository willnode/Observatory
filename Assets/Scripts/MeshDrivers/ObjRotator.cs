using UnityEngine;

// Self-rotating object for asteroids
public class ObjRotator : MonoBehaviour
{
    public float speed;

    void Update ()
    {
        transform.Rotate (Vector3.up * (speed * Time.deltaTime * SolarOrbit.timeScale), Space.Self);
    }
}


