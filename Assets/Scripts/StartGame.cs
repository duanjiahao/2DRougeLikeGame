using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour {

    public Camera UICamera;
    public Transform heroContainer;
    public Transform DungeonContainer;
    public Transform LittleMapContainer;
    public CanvasScaler canvasScaler;
    public GameObject mask;

    public PressComponent left;
    public PressComponent right;
    public PressComponent up;
    public PressComponent down;

    public void RestartAll() {
        StartCoroutine("Restart");
    }

    IEnumerator Restart() {
        needStop = true;
        yield return null;
        ResourceManager.Singleton.UnloadAllResource();
        yield return null;
        CharacterManager.Singleton.RemoveAllCharacter();
        yield return null;
        DungeonManager.Singleton.StartNewDungeon();
        yield return null;
        CharacterManager.Singleton.GenerateHero();
        yield return null;
        LittleMapManager.Singleton.DrawLittleMap();
        yield return null;
        needStop = false;
    }

	// Use this for initialization
	void Start () {
        float currentTime = Time.realtimeSinceStartup;

        ResourceManager.Singleton.Init();
        InputController.Singleton.Init(this);
        DungeonManager.Singleton.Init(DungeonContainer);
        CharacterManager.Singleton.Init(heroContainer, this);

        DungeonManager.Singleton.StartNewDungeon();
        CharacterManager.Singleton.GenerateHero();

        LittleMapManager.Singleton.Init(LittleMapContainer, canvasScaler);
        LittleMapManager.Singleton.DrawLittleMap();

        UICamera.transform.localPosition = new Vector3(DungeonManager.Singleton.CurrentDungeon.StartPoint.col * Utils.TILE_SIZE, DungeonManager.Singleton.CurrentDungeon.StartPoint.row * Utils.TILE_SIZE);

        //记录耗时
        Debug.LogWarning(Time.realtimeSinceStartup - currentTime);
    }

    private bool needStop;
    // Update is called once per frame
    void Update() {
        if (needStop) {
            return;
        }

        InputController.Singleton.Update();
        CharacterManager.Singleton.Update();
        LittleMapManager.Singleton.Update();
	}

    private void LateUpdate() {
        if (CharacterManager.Singleton.Hero != null && CharacterManager.Singleton.Hero.go != null) {
            //Vector3 distance = CharacterManager.Singleton.Hero.go.transform.localPosition - UICamera.transform.localPosition;
            //UICamera.transform.localPosition += distance * Time.smoothDeltaTime * 5f;
            UICamera.transform.localPosition = CharacterManager.Singleton.Hero.go.transform.localPosition;
        }
    }
}
