using UnityEngine;

using System;

[CreateAssetMenu]
public class StatData : ScriptableObject
{
    public PlanetData[] planets;

    public PlanetData GetPlanet (string name)
    {
        for (int i = 0; i < planets.Length; i++) {
            if (planets [i].id == name)
                return planets [i];
        }
        return null;
    }

#if UNITY_EDITOR
    [ContextMenu("Export")]
    public void Export () {
      GUIUtility.systemCopyBuffer = UnityEditor.EditorJsonUtility.ToJson(this, true);
    }

    [ContextMenu("Import")]
    public void Import () {
        UnityEditor.EditorJsonUtility.FromJsonOverwrite(GUIUtility.systemCopyBuffer, this);
    }
#endif

}

[Serializable]
public class PlanetData
{
    public string id;
//    public string type;

    public PlanetPhysical physical = new PlanetPhysical ();
    public PlanetOrbital orbital = new PlanetOrbital ();
    public MoonData[] moons;
  //  public PlanetCoreData[] core;


    public MoonData GetMoon (string id)
    {
        for (int i = 0; i < moons.Length; i++) {
            if (moons [i].id == id)
                return moons [i];
        }
        return null;
    }
}

[Serializable]
public class PlanetPhysical
{
    [Tooltip ("Unit: km")]
    public float radius;
    public Color scheme;
}

[Serializable]
public class PlanetOrbital
{
    public float rotation;
    public float periode;
    public float perihelion;
    public int distance_;
    public float tilt;
    public float inclination;
    public float eccentricity;
    public float long_ascend;

}

[Serializable]
public class MoonData
{
    public string id;
    public float radius;
    public float periode;
    public float perihelion;
    public int distance_;
    public float inclination;
    public float eccentricity;
    public Color scheme = Color.white;
}

[Serializable]
public class PlanetCoreData
{
    public string symbol;
    public Color color;
    public float value;
}