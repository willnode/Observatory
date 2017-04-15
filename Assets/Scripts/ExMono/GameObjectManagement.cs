


using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// GM is shortcut for GameObject Managements (for getting tag, script singleton, spawn and despawn)
/// </summary> 
public static class GM {
    
    /// <summary>
    /// Get the component, or add it instead in case if it null
    /// </summary>
    public static T GetOrAddComponent<T>(this GameObject game) where T : Component
    {
        // Somebody said this is much faster 40% than usual <T>
        var t = (T)game.GetComponent(typeof(T));
        return t ?? (T)game.AddComponent(typeof(T));
    }
    
    /// <summary>
    /// Extension short-hand for GetOrAddComponent (Always Returns ExMono)
    /// </summary>
    public static MonoBehaviourEX GC(this GameObject game) {
        return GetOrAddComponent<MonoBehaviourEX>(game);
    }

    /// <summary>
    /// Extension short-hand for GetComponent
    /// </summary>
    public static T GC<T>(this GameObject game) where T : Component {
        return  (T)game.GetComponent(typeof(T));
    }
    
    /// <summary>
    /// Extension short-hand for GetOrAddComponent (Always Returns ExMono)
    /// </summary>
    public static MonoBehaviourEX GC(this Component game) {
        return GetOrAddComponent<MonoBehaviourEX>(game.gameObject);
    }
    
    /// <summary>
    /// Extension short-hand for GetComponent
    /// </summary>
    public static T GC<T>(this Component game) where T : Component {
        return  (T)game.GetComponent(typeof(T));
 }

    static Dictionary<string, GameObject> tagCatalog = new Dictionary<string, GameObject>();
    
    static Dictionary<Type, Component> mainCatalog = new Dictionary<Type, Component>();
    
    static Dictionary<string, Dictionary<Type, Component>> monoTaggedCatalog = new Dictionary<string, Dictionary<Type, Component>>();

    /// <summary>
    /// Get GameObject with Specific tag
    /// </summary>
    public static GameObject Get (string tag) {
        if (tagCatalog.ContainsKey(tag)) {
            var v = tagCatalog[tag];
            if (v)
                return v;
        }
        return tagCatalog[tag] = GameObject.FindWithTag(tag);
    }

    public static T Get<T>(string tag) where T : Component
    {
        return Get(tag).GetComponent<T>();
    }
    
    /// <summary>
    /// Get Main component (one script for whole game)
    /// </summary>
    public static T Main<T> () where T : Component
    {
        var t = typeof(T);
        return (T)(mainCatalog.GetValueOrDefault(t) ?? (mainCatalog[t] = (Component)GameObject.FindObjectOfType(t)));
    }
    
    public static T Main<T> (string gameTag) where T : Component
    {
        var t = typeof(T);
        var dic = monoTaggedCatalog.GetValueOrDefault(gameTag);
        if (dic == null)
            dic = monoTaggedCatalog[gameTag] = new Dictionary<Type, Component>();
        var obj = dic.GetValueOrDefault(t);
        if (!obj) {
            var gS = GameObject.FindGameObjectsWithTag(gameTag);
            Component c = null;
            for (int i = 0; i < gS.Length; i++)
            {
                if (c = gS[i].GetComponent(t)) {
                    obj = dic[t] = c;
                    break;
                }
            }
        }
        return (T)obj;
    }
    
    public static GameObject player {
        get {
            return Get("Player");
        }
    }
    
    public static GameObject gameController {
        get {
            return Get("GameController");
        }
    }

    static Dictionary<GameObject, Stack<GameObject>> prefabCatalog = new Dictionary<GameObject, Stack<GameObject>>();
    static Dictionary<int, GameObject> prefabReleased = new Dictionary<int, GameObject>();
    
    public static GameObject Spawn (GameObject prefab) {
    #if UNITY_EDITOR
        if (!prefab || prefab.activeInHierarchy)
            throw new NotSupportedException("GM.Spawn() only accepts Prefab to be Instantianted");
    #endif
        var stk = prefabCatalog.GetValueOrDefault(prefab);
        if (stk == null)
            stk = prefabCatalog[prefab] = new Stack<GameObject>();
        if (stk.Count == 0) {
            var obj = GameObject.Instantiate(prefab);
            prefabReleased[(obj.GetInstanceID())] = prefab;
            return obj;
        }
        else {
            var obj = stk.Pop();
            obj.SetActive(true);
            obj.SendMessage("Start", null, SendMessageOptions.DontRequireReceiver);
            obj.hideFlags = HideFlags.None;
            return obj;
        }
    }
    
    public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation) {
        var obj = Spawn(prefab);
        var t = obj.transform;
        t.position = position;
        t.rotation = rotation;
        return obj;
    }
    
    public static GameObject Spawn(GameObject prefab, Transform parent, bool keepWorldSpace = false) {
        var obj = Spawn(prefab);
        if (parent) {
            var t = obj.transform;
            t.SetParent(parent, keepWorldSpace);            
        }
        return obj;
    }
    
    public static T Spawn<T>(GameObject prefab, Transform parent, bool keepWorldSpace = false) where T : Component
    {
        return (T)Spawn(prefab, parent, keepWorldSpace).GetComponent(typeof(T));
    }

    public static void BulkSpawn(List<GameObject> lists, GameObject prefab, Transform parent = null, int count = 1, bool keepWorldSpace = false)
    {
        for (int i = 0; i < count; i++)
        {
            lists.Add(Spawn(prefab, parent, keepWorldSpace));
        }
    }
    
    public static void BulkSpawn<T>(List<T> lists, GameObject prefab, Transform parent = null, int count = 1, bool keepWorldSpace = false) where T : Component
    {
        for (int i = 0; i < count; i++)
        {
            lists.Add(Spawn<T>(prefab, parent, keepWorldSpace));
        }        
    }
    
    public static void Despawn(GameObject gameObj) {
        var id = gameObj.GetInstanceID();
        var prefab = prefabReleased.GetValueOrDefault(id);
        if (!prefab) {
            // Prefab not found. This is happen only in editor... where script goes recompiled in middle of game.
            GameObject.Destroy(gameObj);
        } else {
            gameObj.SetActive(false);
            gameObj.hideFlags = HideFlags.DontSave | HideFlags.HideInHierarchy;
            var t = gameObj.transform;
            t.SetParent(null, false);
            t.position = Vector3.zero;
            t.rotation = Quaternion.identity;
            var stk = prefabCatalog.GetValueOrDefault(prefab);
            if (stk == null)
                stk = prefabCatalog[prefab] = new Stack<GameObject>();
            stk.Push(gameObj);
        }
    }

    public static void DespawnAllChildren(Transform parent, bool alsoDestroyNonSpawnedObjects = true) {
        for (int i = parent.childCount; i-- > 0;)
        {
            var g = parent.GetChild(i).gameObject;
            if (alsoDestroyNonSpawnedObjects || prefabReleased.ContainsKey(g.GetInstanceID()))
                Despawn(g);
        }
    }

    public static void DespawnAllChildren(List<GameObject> lists, bool alsoDestroyNonSpawnedObjects = true, bool alsoRemoveFromTheList = true) {
        var flag = alsoRemoveFromTheList && !alsoRemoveFromTheList;
        for (int i = lists.Count; i-- > 0;)
        {
            var g = lists[i];
            if (alsoDestroyNonSpawnedObjects || prefabReleased.ContainsKey(g.GetInstanceID())) {
                Despawn(g);                
                if (flag)
                    lists.RemoveAt(i);
            }
        }
        if (alsoRemoveFromTheList & alsoRemoveFromTheList)
            lists.Clear();
    }
    
    
    public static void DespawnAllChildren<T>(List<T> lists, bool alsoDestroyNonSpawnedObjects = true, bool alsoRemoveFromTheList = true) where T : Component
    {
        var flag = alsoRemoveFromTheList && !alsoRemoveFromTheList;
        for (int i = lists.Count; i-- > 0;)
        {
            var g = lists[i].gameObject;
            if (alsoDestroyNonSpawnedObjects || prefabReleased.ContainsKey(g.GetInstanceID())) {
                Despawn(g);                
                if (flag)
                    lists.RemoveAt(i);
            }
        }
        if (alsoRemoveFromTheList & alsoRemoveFromTheList)
            lists.Clear();
    }
    

}