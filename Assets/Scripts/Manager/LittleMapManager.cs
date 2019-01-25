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

    Dictionary<Position, Image> checkMap;
    Queue<Position> lightArea;

    public Transform Container {
        get;
        private set;
    }

    public const float MAP_SIZE = 150f;

    private GameObject littleHero;
    private Position sightArea;
    private float PER_SIZE;

    public void Init(Transform littleMapContainer, CanvasScaler canvasScaler) {
        checkMap = new Dictionary<Position, Image>();
        lightArea = new Queue<Position>();
        this.Container = littleMapContainer;
        PER_SIZE = MAP_SIZE / (DungeonManager.Singleton.CurrentDungeon.Size + 2);
        sightArea = new Position(Mathf.FloorToInt(canvasScaler.referenceResolution.y / Utils.TILE_SIZE / 2f), Mathf.FloorToInt(canvasScaler.referenceResolution.x / Utils.TILE_SIZE / 2f));
        lastPositon = CharacterManager.Singleton.Hero.currentPosition;
    }

    public void DrawLittleMap() {
        checkMap.Clear();
        int dungeonSize = DungeonManager.Singleton.CurrentDungeon.Size;
        Dictionary<Position, BaseTile>.Enumerator itor = DungeonManager.Singleton.CurrentDungeon.DungeonMap.GetEnumerator();
        while (itor.MoveNext()) {
            GameObject go = ResourceManager.Singleton.Instantiate(itor.Current.Value.image, Container);
            RectTransform rectTransform = go.GetComponent<RectTransform>();
            rectTransform.sizeDelta = Vector2.one * PER_SIZE;
            rectTransform.anchorMin = Vector2.one;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.pivot = Vector2.one;

            go.transform.localPosition = new Vector3((itor.Current.Key.col - dungeonSize) * PER_SIZE, (itor.Current.Key.row - dungeonSize) * PER_SIZE);
            Image image = go.GetComponentInChildren<Image>();
            image.color = IsInSight(itor.Current.Key) ? Color.white : Color.black;
            checkMap.Add(itor.Current.Key, image);
        }

        Hero hero = CharacterManager.Singleton.Hero;
        littleHero = ResourceManager.Singleton.Instantiate(hero.littleMapImage, Container);
        RectTransform rect = littleHero.GetComponent<RectTransform>();
        rect.sizeDelta = Vector2.one * PER_SIZE;
        rect.anchorMin = Vector2.one;
        rect.anchorMax = Vector2.one;
        rect.pivot = Vector2.one;
        littleHero.transform.localPosition = new Vector3((hero.currentPosition.col - dungeonSize) * PER_SIZE, (hero.currentPosition.row - dungeonSize) * PER_SIZE);
    }

    public void CheckSightArea() {
        if (CharacterManager.Singleton.Hero.currentPosition.col != lastPositon.col) {
            bool isUp = CharacterManager.Singleton.Hero.currentPosition.col > lastPositon.col;
            for (int i = CharacterManager.Singleton.Hero.currentPosition.row - sightArea.row; i <= CharacterManager.Singleton.Hero.currentPosition.row + sightArea.row; i++) {
                Position position = new Position(i, CharacterManager.Singleton.Hero.currentPosition.col + (isUp ? 1 : -1) * sightArea.col);
                if (checkMap.ContainsKey(position)) {
                    checkMap[position].color = Color.white;
                }
            }
        }

        if (CharacterManager.Singleton.Hero.currentPosition.row != lastPositon.row) {
            bool isUp = CharacterManager.Singleton.Hero.currentPosition.row > lastPositon.row;
            for (int i = CharacterManager.Singleton.Hero.currentPosition.col - sightArea.col; i <= CharacterManager.Singleton.Hero.currentPosition.col + sightArea.col; i++) {
                Position position = new Position(CharacterManager.Singleton.Hero.currentPosition.row + (isUp ? 1 : -1) * sightArea.row, i);
                if (checkMap.ContainsKey(position)) {
                    checkMap[position].color = Color.white;
                }
            }
        }
    }

    private bool IsInSight(Position position) {
        Position MaxPositon = CharacterManager.Singleton.Hero.currentPosition + sightArea;
        Position MinPositon = CharacterManager.Singleton.Hero.currentPosition - sightArea;
        return position <= MaxPositon && position >= MinPositon;
    }

    private Position lastPositon;
    public void Update() {
        if (CharacterManager.Singleton.Hero != null && lastPositon != CharacterManager.Singleton.Hero.currentPosition) {
            CheckSightArea();

            if (littleHero != null) {
                Hero hero = CharacterManager.Singleton.Hero;
                littleHero.transform.localPosition = new Vector3((hero.currentPosition.col - DungeonManager.Singleton.CurrentDungeon.Size) * PER_SIZE, (hero.currentPosition.row - DungeonManager.Singleton.CurrentDungeon.Size) * PER_SIZE);
            }
            lastPositon = CharacterManager.Singleton.Hero.currentPosition;
        }
    }
}
