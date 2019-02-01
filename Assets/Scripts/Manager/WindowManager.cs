using UnityEngine;
using System.Collections.Generic;

public class WindowManager {
    private static WindowManager windowManager;
    public static WindowManager Singleton {
        get {
            if (windowManager == null) {
                windowManager = new WindowManager();
            }
            return windowManager;
        }
    }

    private Transform container;

    private Dictionary<string, Window> windowMap;
    private List<Window> windowList;

    public void Init(Transform container) {
        this.container = container;
        windowMap = new Dictionary<string, Window>();
        windowList = new List<Window>();
    }

    public void ClearAllWindow() {
        windowMap.Clear();
    }

    public T ShowWindow<T>(string path) where T : BaseWindow, new() {
        if (windowMap.ContainsKey(path)) {
            windowMap[path].Show();
            return windowMap[path] as T;
        }

        GameObject go = ResourceManager.Singleton.Instantiate(path, container, false);
        T w = new T();
        w.GameObject = go;
        w.InitUI();
        windowMap.Add(path, w);
        windowList.Add(w);
        return w;
    }

    public void Update() {
        for (int i = 0; i < windowList.Count; i++) {
            BaseWindow window = windowList[i] as BaseWindow;
            if (window.GameObject.activeSelf && window is IUpdateWindow) {
                (window as IUpdateWindow).Update();
            }
        }
    }
}
