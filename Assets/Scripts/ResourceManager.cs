using UnityEngine;
using System.Collections.Generic;

public class ResourceManager {

    private static ResourceManager sigleton;
    public static ResourceManager Singleton {
        get {
            if (sigleton == null) {
                sigleton = new ResourceManager();
            }
            return sigleton;
        }
    }

    private Dictionary<string, GameObject> pool;

    public void Init() {
        pool = new Dictionary<string, GameObject>();
    }

    /// <summary>
    /// 卸载ResourceManager
    /// </summary>
    public void UnLoad() {
        Dictionary<string, GameObject>.Enumerator itor = pool.GetEnumerator();
        while (itor.MoveNext()) {
            Unload(itor.Current.Value);
        }
        pool.Clear();
        Resources.UnloadUnusedAssets();
    }

    /// <summary>
    /// 实例一个load好的gameobjcet
    /// </summary>
    /// <returns>The instantiate.</returns>
    /// <param name="prefab">Prefab.</param>
    /// <param name="parent">Parent.</param>
    /// <param name="name">Name.</param>
    public GameObject Instantiate(GameObject prefab, Transform parent, string name = "") {
        if (prefab == null) {
            return null;
        }

        GameObject gameObject = GameObject.Instantiate(prefab, prefab.transform.localPosition, prefab.transform.localRotation, parent) as GameObject;
        gameObject.transform.localScale = prefab.transform.localScale;
        if (!string.IsNullOrEmpty(name)) {
            gameObject.name = name;
        }
        return gameObject;
    }

    public GameObject Instantiate(string path, Transform parent, string name = "") {
        if (pool.ContainsKey(path)) {
            return Instantiate(pool[path], parent, name);
        }

        GameObject prefab = Load(path);
        pool.Add(path, prefab);

        return Instantiate(prefab, parent, name);
    }

    public GameObject Load(string path) {
        return Resources.Load(path, typeof(GameObject)) as GameObject;
    }

    public void Unload(GameObject gameObject) {
        GameObject.Destroy(gameObject);
        gameObject = null;
    }
}
