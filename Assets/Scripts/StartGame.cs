using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StartGame : MonoBehaviour {

    public Camera mainCamera;
    public Camera uiCamera;
    public Transform heroContainer;
    public Transform DungeonContainer;
    public Transform LittleMapContainer;
    public GameObject mask;
    public StatusPanel statusPanel;

    public PressComponent left;
    public PressComponent right;
    public PressComponent up;
    public PressComponent down;
    public PressComponent attack;

    public Button leftDir;
    public Button rightDir;
    public Button upDir;
    public Button downDir;

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
        CharacterManager.Singleton.GenerateMonsters();
        yield return null;
        LittleMapManager.Singleton.DrawLittleMap();
        yield return null;
        needStop = false;
    }

    IEnumerator StartRemoveCharacter(BaseCharacter baseCharacter) {
        yield return null;
        CharacterManager.Singleton.Characters.Remove(baseCharacter);
    }

	// Use this for initialization
	void Start () {
        float currentTime = Time.realtimeSinceStartup;

        SceneManager.Singleton.Init(this);
        ResourceManager.Singleton.Init();
        InputController.Singleton.Init();
        DungeonManager.Singleton.Init(DungeonContainer);
        CharacterManager.Singleton.Init(heroContainer);

        DungeonManager.Singleton.StartNewDungeon();
        CharacterManager.Singleton.GenerateHero();
        CharacterManager.Singleton.GenerateMonsters();

        LittleMapManager.Singleton.Init(LittleMapContainer, uiCamera);
        LittleMapManager.Singleton.DrawLittleMap();

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
        SceneManager.Singleton.Update();
	}

    private void LateUpdate() {
        SceneManager.Singleton.LateUpdate();
    }
}
