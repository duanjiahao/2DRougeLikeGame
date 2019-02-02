using System;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType {
    Up = 1,
    Down = 2,
    Left = 3,
    Right = 4,
    Attack = 5,
    BeAttack = 6,
}

public class InputController {

    private static InputController inputController;
    public static InputController Singleton {
        get {
            if (inputController == null) {
                inputController = new InputController();
            }
            return inputController;
        }
    }

    private bool isControlling;

    private Action currentAction;

    public void Init() {
        isControlling = false;
        currentAction = null;
        AddEvent();
    }

    public void Update() {

        if (isControlling) {
            if (currentAction != null) {
                currentAction.Invoke();
            }
            return;
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
            DispatchAction(ActionType.Up);
            return;
        }

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
            DispatchAction(ActionType.Down);
            return;
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
            DispatchAction(ActionType.Left);
            return;
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
            DispatchAction(ActionType.Right);
            return;
        }

        if (Input.GetKey(KeyCode.K)) {
            DispatchAction(ActionType.Attack);
            return;
        }

        currentAction = null;
    }

    public void DispatchAction(ActionType actionType) {
        if (isControlling) {
            return;
        }

        foreach (Monster monster in CharacterManager.Singleton.Monsters) {
            monster.WhatToDo(Utils.GetHeroPosition(actionType));
        }

        CharacterManager.Singleton.Hero.ClearQueue();
        CharacterManager.Singleton.Hero.SetQueueByActionType(actionType);

        isControlling = true;
        currentAction = Action;
        currentAction.Invoke();
    }

    /// <summary>
    /// 接收点击事件
    /// </summary>
    private void AddEvent() {
        SceneManager.StartGame.left.OnPress.AddListener(OnLeftClick);
        SceneManager.StartGame.right.OnPress.AddListener(OnRightClick);
        SceneManager.StartGame.up.OnPress.AddListener(OnUpClick);
        SceneManager.StartGame.down.OnPress.AddListener(OnDownClick);
        SceneManager.StartGame.attack.OnPress.AddListener(OnAttackClick);

        SceneManager.StartGame.upDir.onClick.AddListener(OnUpDirClick);
        SceneManager.StartGame.downDir.onClick.AddListener(OnDownDirClick);
        SceneManager.StartGame.leftDir.onClick.AddListener(OnLeftDirClick);
        SceneManager.StartGame.rightDir.onClick.AddListener(OnRightDirClick);
        SceneManager.StartGame.pauseBtn.onClick.AddListener(OnPauseDirClick);
    }

    private void OnPauseDirClick() {
        PauseWindow pauseWindow = WindowManager.Singleton.ShowWindow<PauseWindow>(UIWindowType.PAUSE_WINDOW);
        pauseWindow.SetContent();
    }

    private void OnRightDirClick() {
        CharacterManager.Singleton.Hero.ChangeDirction(CharacterDirection.RIGHT);
    }

    private void OnLeftDirClick() {
        CharacterManager.Singleton.Hero.ChangeDirction(CharacterDirection.LEFT);
    }

    private void OnDownDirClick() {
        CharacterManager.Singleton.Hero.ChangeDirction(CharacterDirection.DOWN);
    }

    private void OnUpDirClick() {
        CharacterManager.Singleton.Hero.ChangeDirction(CharacterDirection.UP);
    }

    private void OnDownClick() {
        InputController.Singleton.DispatchAction(ActionType.Down);
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

    private void OnAttackClick() {
        InputController.Singleton.DispatchAction(ActionType.Attack);
    }

    #region Action Event 
    private void Action() {
        bool isAllCompelte = true;

        if (CharacterManager.Singleton.Hero == null) {
            return;
        }

        if (CharacterManager.Singleton.Hero.MoveActionQueue.Count > 0) {
            if (CharacterManager.Singleton.Hero.MoveActionQueue.Peek().Invoke()) {
                CharacterManager.Singleton.Hero.MoveActionQueue.Dequeue();
            } else {
                isAllCompelte = false;
            }
        }

        if (CharacterManager.Singleton.Hero.OtherActionQueue.Count > 0) {
            if (CharacterManager.Singleton.Hero.OtherActionQueue.Peek().Invoke()) {
                CharacterManager.Singleton.Hero.OtherActionQueue.Dequeue();
            }
            return;
        }

        if (isAllCompelte) {
            foreach (Monster monster in CharacterManager.Singleton.Monsters) {
                if (monster.OtherActionQueue.Count > 0) {
                    if (monster.OtherActionQueue.Peek().Invoke()) {
                        monster.OtherActionQueue.Dequeue();
                    }
                    return;
                }
            }
        }


        foreach (Monster monster in CharacterManager.Singleton.Monsters) {
            if (monster.MoveActionQueue.Count > 0) {
                if (monster.MoveActionQueue.Peek().Invoke()) {
                    monster.MoveActionQueue.Dequeue();
                }
                else {
                    isAllCompelte = false;
                }
            } 
        }

        if (isAllCompelte) {
            currentAction = null;
            isControlling = false;
        }
    }
    #endregion
}
