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
    public Transform WindowContainer;
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

    public enum RestartStep {
        UnloadingResource = 1,
        RemovingCharacters = 2,
        StartingNewDungeon = 3,
        GeneratingNewHero = 4,
        GeneratingMonsters = 5,
        DrawingLittleMap = 6,
        AllComplete = 7,
    }

    [HideInInspector]
    public RestartStep currentStep;

    public void RestartAll() {
        StartCoroutine("Restart");
    }

    IEnumerator Restart() {
        needStop = true;
        currentStep = RestartStep.UnloadingResource;
        yield return null;
        ResourceManager.Singleton.UnloadAllResource();
        yield return null;
        currentStep = RestartStep.RemovingCharacters;
        CharacterManager.Singleton.RemoveAllCharacter();
        yield return null;
        currentStep = RestartStep.StartingNewDungeon;
        DungeonManager.Singleton.StartNewDungeon();
        yield return null;
        currentStep = RestartStep.GeneratingNewHero;
        CharacterManager.Singleton.GenerateHero();
        yield return null;
        currentStep = RestartStep.GeneratingMonsters;
        CharacterManager.Singleton.GenerateMonsters();
        yield return null;
        currentStep = RestartStep.DrawingLittleMap;
        LittleMapManager.Singleton.DrawLittleMap();
        yield return null;
        currentStep = RestartStep.AllComplete;
        needStop = false;
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
        WindowManager.Singleton.Init(WindowContainer);

        //记录耗时
        Debug.LogWarning(Time.realtimeSinceStartup - currentTime);
    }

    private bool needStop;
    // Update is called once per frame
    void Update() {
        if (!needStop) {
            InputController.Singleton.Update();
            CharacterManager.Singleton.Update();
            LittleMapManager.Singleton.Update();
            SceneManager.Singleton.Update();
        }
       
        WindowManager.Singleton.Update();
	}

    private void LateUpdate() {
        SceneManager.Singleton.LateUpdate();
    }
}
