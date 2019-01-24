using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class StartGame : MonoBehaviour {

    public Camera UICamera;

    public Transform heroContainer;

    public Transform DungeonContainer;

    public EventTrigger left;
    public EventTrigger right;
    public EventTrigger up;
    public EventTrigger down;

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
        left.triggers[0].callback.AddListener(OnLeftClick);
        right.triggers[0].callback.AddListener(OnRightClick);
        up.triggers[0].callback.AddListener(OnUpClick);
        down.triggers[0].callback.AddListener(OnDownClick);
    }

    private void OnDownClick(BaseEventData eventData) {
        InputController.Singleton.DispatchAction(ActionType.Donw);
    }

    private void OnUpClick(BaseEventData eventData) {
        InputController.Singleton.DispatchAction(ActionType.Up);
    }

    private void OnRightClick(BaseEventData eventData) {
        InputController.Singleton.DispatchAction(ActionType.Right);
    }

    private void OnLeftClick(BaseEventData eventData) {
        InputController.Singleton.DispatchAction(ActionType.Left);
    }
}
