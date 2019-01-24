using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LittleMapManager {

    private static LittleMapManager littleMapManager;
    public static LittleMapManager Singleton {
        get {
            if (littleMapManager == null) {
                littleMapManager = new LittleMapManager();
            }
            return littleMapManager;
        }
    }

    Dictionary<Position, bool> checkMap;
    Queue<Position> lightArea;

    public Transform Container {
        get;
        private set;
    }

    public const float MAP_SIZE = 150f;

    private Vector2 sightArea;
    private float PER_SIZE;

    public void Init(Transform littleMapContainer, CanvasScaler canvasScaler) {
        checkMap = new Dictionary<Position, bool>();
        lightArea = new Queue<Position>();
        this.Container = littleMapContainer;
        PER_SIZE = MAP_SIZE / (DungeonManager.Singleton.CurrentDungeon.Size + 2);
        sightArea = new Vector2(Mathf.FloorToInt(canvasScaler.referenceResolution.x / Utils.TILE_SIZE), Mathf.FloorToInt(canvasScaler.referenceResolution.y / Utils.TILE_SIZE)); 
    }

    public void DrawLittleMap() {
        Dictionary<Position, BaseTile>.Enumerator itor = DungeonManager.Singleton.CurrentDungeon.DungeonMap.GetEnumerator();
        while (itor.MoveNext()) {
            GameObject go = ResourceManager.Singleton.Instantiate(itor.Current.Value.image, Container);
            RectTransform rectTransform = go.GetComponent<RectTransform>();
            rectTransform.sizeDelta = Vector2.one * PER_SIZE;
            rectTransform.anchorMin = Vector2.one;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.pivot = Vector2.one;

            int dungeonSize = DungeonManager.Singleton.CurrentDungeon.Size;
            go.transform.localPosition = new Vector3((itor.Current.Key.col - dungeonSize) * PER_SIZE, (itor.Current.Key.row - dungeonSize) * PER_SIZE);
        }
    }

    public void Update() {
        
    }
}
