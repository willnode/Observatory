using UnityEngine;
using System.Collections.Generic;
using System;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
#endif

[CreateAssetMenu]
public class DataDetail : ScriptableObject
{
    public MetadataDetail metadata;

    public PlanetDetail[] planets;

    public PlanetDetail GetPlanet (string name)
    {
        for (int i = 0; i < planets.Length; i++) {
            if (planets [i].id == name)
                return planets [i];
        }
        return null;
    }

    public PlanetDetail GetPlanet (PlanetData data)
    {
        return GetPlanet (data.id);
    }

    #if UNITY_EDITOR
    public void Fill (AtomDetail dt, PlanetDetail p, ref int plugged, ref int added, ref int possible)
    {
        var path = Application.dataPath + "/Resources/" + p.id;
        if (!Directory.Exists (path))
            return;
        
        var s = Directory.GetFiles (path);
        var sL = new List<string> (s);
        for (int i = sL.Count - 1; i >= 0; i--) {
            if (sL [i].Contains (".meta"))
                sL.RemoveAt (i);
        }
        s = sL.ToArray ();
        added += s.Length;
        foreach (var c in dt.images) {
            if (!string.IsNullOrEmpty (c.path)) {
                plugged++;
                continue;
            }
            if (string.IsNullOrEmpty (c.wikiLink)) {
                Debug.LogWarning ("NOT VALID: " + p.name + "/" + dt.head);
   
                continue;
            }
            possible++;
            string sp;
            if (LocateImage (c.wikiLink, p.id, s, out sp)) {
                c.path = sp;
                plugged++;
            }
            
        }
    }

    bool LocateImage (string wiki, string id, string[] files, out string image)
    {
        var path = Application.dataPath + "/Resources/" + id;
        var ss = Path.GetFileNameWithoutExtension (wiki);
        var o = files.FirstOrDefault (x => x.Contains (ss));
        if (o == default(string)) {
            Debug.LogErrorFormat ("NOT FOUND: {0}/{1}\n\nhttp://en.wikipedia.org/wiki/File:{2}\n", id, ss, wiki);
            image = null;
            return false;
        }
        o = "Assets/Resources/" + id + o.Replace (path + "\\", "/");
        var img = AssetDatabase.LoadAssetAtPath<Sprite> (o);
        if (!img) {
            var mT = (TextureImporter)TextureImporter.GetAtPath (o);
            if (mT) {
                mT.textureType = TextureImporterType.Default;
                mT.mipmapEnabled = false;
                mT.maxTextureSize = 1024;
                mT.npotScale = TextureImporterNPOTScale.None;
                mT.spriteImportMode = SpriteImportMode.Single;
                mT.crunchedCompression = true;
                mT.compressionQuality = 40;
                mT.SaveAndReimport ();
                img = AssetDatabase.LoadAssetAtPath<Sprite> (o);
            }
        }
        if (img) {
            image = GetFileNameWithoutExtension (o).Replace ("Assets/Resources/", string.Empty);
            if (!Resources.Load<Sprite> (image))
                Debug.LogWarning ("UUHHH..OHHH.PATHERRRRO???  " + path);
            return true;
        }
        image = string.Empty;
        return false;
    }

    public static string GetFileNameWithoutExtension (string path)
    {
        if (path == null)
            return null;
        int length;
        if ((length = path.LastIndexOf ('.')) == -1)
            return path;
        return path.Substring (0, length);
    }

    [ContextMenu ("Import")]
    public void Import ()
    {
        if (EditorUtility.DisplayDialog ("", "Overwrite?", "YUP"))
            EditorJsonUtility.FromJsonOverwrite (GUIUtility.systemCopyBuffer, this);
        else
            return;
        Debug.ClearDeveloperConsole ();
        int plugged = 0, added = 0, ad = 0, possible = 0;
        int total = 0, qualified = 0, toomuch = 0, charT = 0;
        bool hasAdd = false;
        foreach (var p in planets) {
            bool qualifyWarn = false;
            hasAdd = false;
            foreach (var o in p.points) {
                total++;
                if (o.body.Length > 415)
                    toomuch++;
                else if (o.body.Length >= 315)
                    qualified++;
                else if (!qualifyWarn) {
                    qualifyWarn = true;
                    Debug.LogWarning (p.id + " is yet qualified, at " + o.head);
                }
                charT += Mathf.Min (o.body.Length, 315);
                if (!hasAdd)
                    Fill (o, p, ref plugged, ref added, ref possible);
                else
                    Fill (o, p, ref plugged, ref ad, ref possible);
                hasAdd = true;
            }
        }
        Debug.LogFormat ("Plugged: {0}/{2}, Available: {1}", plugged, added, possible);
        Debug.LogFormat ("Total Points: {0}, Approved {1} ({3}), Completion {2} (-{4})", total, qualified + toomuch, (charT / (total * 315f)).ToString ("P")
            , ((qualified + toomuch) / (float)total).ToString ("P0"), ((total * 315f) - charT));
        EditorUtility.SetDirty (this);
    }

    [ContextMenu ("Export")]
    public void Export ()
    {
        var s = EditorJsonUtility.ToJson (this, true);
        s = s.Replace ("},\n                    {", "}, {");
        GUIUtility.systemCopyBuffer = s;
    }

    [ContextMenu("Export URLs")]
    public void ExportURLs ()
    {
        StringBuilder s = new StringBuilder(3000);
        foreach (var p in planets) {
            s.AppendLine(p.name + " (" + p.id + ")");
            foreach (var a in p.points) {
                foreach (var i in a.images) {
                    s.AppendLine("\thttp://en.wikipedia.org/wiki/File:" + i.wikiLink);
                }
            }
        }
        GUIUtility.systemCopyBuffer = s.ToString();
    }

    [MenuItem ("Assets/Import Detail")]
    public static void ImportStatic ()
    {
        string[] targetData = AssetDatabase.FindAssets ("t:DataDetail");
        if (targetData.Length > 0) {
            var x = AssetDatabase.LoadAssetAtPath<DataDetail> (AssetDatabase.GUIDToAssetPath (targetData [0]));
            // Selection.activeObject = x;
            x.Import ();
        }
    }
    #endif
}

[Serializable]
public class PlanetDetail
{
    public string id;
    public string name;
    public AtomDetail[] points;
    public MoonDetail[] moons;

    public MoonDetail GetMoon (string id)
    {
        for (int i = 0; i < moons.Length; i++) {
            if (moons [i].id == id)
                return moons [i];
        }
        return null;
    }
}

[Serializable]
public class MetadataDetail
{
    public string[] TimeScaleNames;
    public int[] SlotCounts;
    public float[] SlotDistances;
    public string[] SlotCaptions;
}

[Serializable]
public class MoonDetail
{
    public string id;
    public string label;
}

[Serializable]
public class AtomDetail
{
    //[UnityEngine.Serialization.FormerlySerializedAs ("label")]
    public string head;
    [TextArea(3,9)]
    public string body;
    public ThumbDetail[] images = new ThumbDetail[1];
}

[Serializable]
public class ThumbDetail
{
    [TextArea(1,3)]
    public string path;
    // http://en.wikipedia.org/wiki/File:
    public string wikiLink;
    [TextArea(1,3)]
    public string caption;
 }

/*[Serializable]

[Serializable]
public class ContextDetail
{
    public string head;
    public string caption;
    [Multiline (4)]
    public string body;
    public Sprite image;
    [UnityEngine.Serialization.FormerlySerializedAs ("wikiImage")]
    public string wikiLink;
}

[Serializable]
public class TextDetail
{
    public string name;
    public int thumb = 0;
    [Multiline (4)]
    public string summary;
    public Slide[] images = new Slide[1];
}

*/

/*
    [ContextMenu ("Import")]
    public void Import ()
    {
        planets = new PlanetDetail[0];
        if (EditorUtility.DisplayDialog ("", "Overwrite?", "YUP"))
            EditorJsonUtility.FromJsonOverwrite (GUIUtility.systemCopyBuffer, this);
        int plugged = 0, added = 0, possible = 0;
        foreach (var p in planets) {
            Fill (p.cores, p, ref plugged, ref added, ref possible);
            Fill (p.moons, p, ref plugged, ref added, ref possible);
            Fill (p.probes, p, ref plugged, ref added, ref possible);
            FillDetail (p.details, p, ref plugged, ref added, ref possible);
        }
        Debug.LogFormat ("Plugged: {0}/{2}, Added: {1}", plugged, added, possible);
    }

    public void Fill (UnitDetail[] dt, PlanetDetail p, ref int plugged, ref int added, ref int possible)
    {
        var path = Application.dataPath + "/Textures/Detail/" + p.id;
        if (!Directory.Exists (path)) {
            //Debug.LogWarning (path);
            return;
        }
        var s = Directory.GetFiles (path);
        var sL = new List<string> (s);
        for (int i = sL.Count - 1; i >= 0; i--) {
            if (sL [i].Contains (".meta"))
                sL.RemoveAt (i);
        }
        s = sL.ToArray ();
        foreach (var c in dt) {
            foreach (var i in c.images) {
                if (i.image) {
                    plugged++;
                    continue;
                }
                if (string.IsNullOrEmpty (i.wikiLink))
                    continue;
                possible++;
                Sprite sp;
                if (LocateImage (i.wikiLink, p.id, s, out sp)) {
                    i.image = sp;
                    plugged++;
                    added++;
                }
            }
        }
    }
    */
   