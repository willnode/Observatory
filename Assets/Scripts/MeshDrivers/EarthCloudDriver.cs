using UnityEngine;


// Driving Cloud Texture of Earth
public class EarthCloudDriver : MonoBehaviour
{
    public Material mat;
    public string textureName = "_CloudMap";
    float offset;
    public float speed;
    // Update is called once per frame
    void Update ()
    {
        offset += SolarOrbit.timeScale * Time.deltaTime * speed;
        offset = Mathf.Repeat (offset, 1f);
        mat.SetTextureOffset (textureName, Vector2.left * offset);
    }
}

