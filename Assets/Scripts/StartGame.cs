using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class StartGame : MonoBehaviour {

    public Camera UICamera;

    public Transform heroContainer;

    public Transform DungeonContainer;

    public PressComponent left;
    public PressComponent right;
    public PressComponent up;
    public PressComponent down;

    public void RestartAll() {
        StartCoroutine("Restart");
    }

    IEnumerator Restart() {
        yield return null;

        ResourceManager.Singleton.UnAllResource();
        DungeonManager.Singleton.StartNewDungeon();
        CharacterManager.Singleton.RemoveAllCharacter();
        CharacterManager.Singleton.GenerateHero();
    }

	// Use this for initialization
	void Start () {
        float currentTime = Time.realtimeSinceStartup;

        ResourceManager.Singleton.Init();
        InputController.Singleton.Init();
        DungeonManager.Singleton.Init(DungeonContainer);
        CharacterManager.Singleton.Init(heroContainer, this);

        DungeonManager.Singleton.StartNewDungeon();
        CharacterManager.Singleton.GenerateHero();

        UICamera.transform.localPosition = new Vector3(DungeonManager.Singleton.CurrentDungeon.StartPoint.col * Utils.TILE_SIZE, DungeonManager.Singleton.CurrentDungeon.StartPoint.row * Utils.TILE_SIZE);

        //记录耗时
        Debug.LogWarning(Time.realtimeSinceStartup - currentTime);
        AddEvent();
    }

    // Update is called once per frame
    void Update() {
        InputController.Singleton.Update();
        CharacterManager.Singleton.Update();

        if (CharacterManager.Singleton.Hero.go != null) {
            UICamera.transform.localPosition = CharacterManager.Singleton.Hero.go.transform.localPosition;
        }
	}

    /// <summary>
    /// 接收点击事件，后边要集成到InputContoller里
    /// </summary>
    private void AddEvent() {
        left.OnPress.AddListener(OnLeftClick);
        right.OnPress.AddListener(OnRightClick);
        up.OnPress.AddListener(OnUpClick);
        down.OnPress.AddListener(OnDownClick);
    }

    private void OnDownClick() {
        InputController.Singleton.DispatchAction(ActionType.Donw);
    }

    private void OnUpClick() {
        InputController.Singleton.DispatchAction(ActionType.Up);
    }

    private void OnRightClick() {
        InputController.Singleton.DispatchAction(ActionType.Right);
    }

    private void OnLeftClick() {
        InputController.Singleton.DispatchAction(ActionType.Left);
    }
}
