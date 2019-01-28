using System;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType {
    Up = 1,
    Down = 2,
    Left = 3,
    Right = 4,
    Attack = 5,
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

    private StartGame startGame;

    private bool isControlling;

    private Action currentAction;

    private Dictionary<ActionType, Action> actionMap;

    public void Init(StartGame startGame) {
        this.startGame = startGame;
        isControlling = false;
        currentAction = null;
        actionMap = new Dictionary<ActionType, Action>();
        actionMap.Add(ActionType.Up, Up);
        actionMap.Add(ActionType.Down, Down);
        actionMap.Add(ActionType.Left, Left);
        actionMap.Add(ActionType.Right, Right);
        actionMap.Add(ActionType.Attack, Attack);
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
        if (isControlling || !actionMap.ContainsKey(actionType)) {
            return;
        }

        foreach (BaseCharacter character in CharacterManager.Singleton.Characters) {
            character.isActing = true;
        }

        isControlling = true;
        currentAction = actionMap[actionType];
        currentAction.Invoke();
    }

    /// <summary>
    /// 接收点击事件
    /// </summary>
    private void AddEvent() {
        startGame.left.OnPress.AddListener(OnLeftClick);
        startGame.right.OnPress.AddListener(OnRightClick);
        startGame.up.OnPress.AddListener(OnUpClick);
        startGame.down.OnPress.AddListener(OnDownClick);
        startGame.attack.OnPress.AddListener(OnAttackClick);
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
    private void Right() {
        bool isAllCompelte = true;
        foreach (BaseCharacter character in CharacterManager.Singleton.Characters) {
            isAllCompelte = character.Move(CharacterDirection.RIGHT) && isAllCompelte;
        }

        if (isAllCompelte) {
            currentAction = null;
            isControlling = false;
        }
    }

    private void Left() {
        bool isAllCompelte = true;
        foreach (BaseCharacter character in CharacterManager.Singleton.Characters) {
            isAllCompelte = character.Move(CharacterDirection.LEFT) && isAllCompelte;
        }
        if (isAllCompelte) {
            currentAction = null;
            isControlling = false;
        }
    }

    private void Down() {
        bool isAllCompelte = true;
        foreach (BaseCharacter character in CharacterManager.Singleton.Characters) {
            isAllCompelte = character.Move(CharacterDirection.DOWN) && isAllCompelte;
        }

        if (isAllCompelte) {
            currentAction = null;
            isControlling = false;
        }
    }

    private void Up() {
        bool isAllCompelte = true;
        foreach (BaseCharacter character in CharacterManager.Singleton.Characters) {
            isAllCompelte = character.Move(CharacterDirection.UP) && isAllCompelte;
        }
        if (isAllCompelte) {
            currentAction = null;
            isControlling = false;
        }
    }

    private void Attack() {
        bool isAllCompelte = true;
        foreach (BaseCharacter character in CharacterManager.Singleton.Characters) {
            isAllCompelte = character.Attack() && isAllCompelte;
        }
        if (isAllCompelte) {
            currentAction = null;
            isControlling = false;
        }
    }
    #endregion
}
