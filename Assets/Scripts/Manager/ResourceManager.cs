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

    private Dictionary<string, GameObject> resourcePrefab;

    private Dictionary<int, List<GameObject>> allInstantiateGo;

    public void Init() {
        resourcePrefab = new Dictionary<string, GameObject>();
        allInstantiateGo = new Dictionary<int, List<GameObject>>();
    }

    /// <summary>
    /// 卸载所有Instanstiate的GO
    /// </summary>
    public void UnloadAllResource() {

        Dictionary<int, List<GameObject>>.Enumerator itor = allInstantiateGo.GetEnumerator();
        while (itor.MoveNext()) {
            Unload(itor.Current.Value);
        }

        resourcePrefab.Clear();
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

        if (!allInstantiateGo.ContainsKey(prefab.GetInstanceID())) {
            allInstantiateGo.Add(prefab.GetInstanceID(), new List<GameObject>());
        }
        allInstantiateGo[prefab.GetInstanceID()].Add(gameObject);

        return gameObject;
    }

    public GameObject Instantiate(string path, Transform parent, string name = "") {
        if (resourcePrefab.ContainsKey(path)) {
            return Instantiate(resourcePrefab[path], parent, name);
        }

        GameObject prefab = Load(path);
        resourcePrefab.Add(path, prefab);

        return Instantiate(prefab, parent, name);
    }

    public GameObject Load(string path) {
        return Resources.Load(path, typeof(GameObject)) as GameObject;
    }

    public void Unload(List<GameObject> list) {
        foreach (GameObject go in list) {
            Unload(go);
        }
    }

    public void Unload(GameObject gameObject) {
        GameObject.Destroy(gameObject);
        gameObject = null;
    }
}
