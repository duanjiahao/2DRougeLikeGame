using System;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType {
    Up = 1,
    Donw = 2,
    Left = 3,
    Right = 4,
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

    private Dictionary<ActionType, Action> actionMap;

    public void Init() {
        isControlling = false;
        currentAction = null;
        actionMap = new Dictionary<ActionType, Action>();
        actionMap.Add(ActionType.Up, Up);
        actionMap.Add(ActionType.Donw, Down);
        actionMap.Add(ActionType.Left, Left);
        actionMap.Add(ActionType.Right, Right);
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
            DispatchAction(ActionType.Donw);
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

        currentAction = null;
    }

    public void DispatchAction(ActionType actionType) {
        if (isControlling || !actionMap.ContainsKey(actionType)) {
            return;
        }

        isControlling = true;
        currentAction = actionMap[actionType];
        currentAction.Invoke();
    }


    #region Action Event 
    private bool setOnce = false;
    private void Right() {
        if (!setOnce) {
            foreach (BaseCharacter character in CharacterManager.Singleton.Characters) {
                character.Init();
            }
            setOnce = true;
        }

        bool isAllCompelte = true;
        foreach (BaseCharacter character in CharacterManager.Singleton.Characters) {
            isAllCompelte = isAllCompelte && character.Move(CharacterDirction.RIGHT);
        }
        if (isAllCompelte) {
            setOnce = false;
            currentAction = null;
            isControlling = false;
        }
    }

    private void Left() {
        if (!setOnce) {
            foreach (BaseCharacter character in CharacterManager.Singleton.Characters) {
                character.Init();
            }
            setOnce = true;
        }

        bool isAllCompelte = true;
        foreach (BaseCharacter character in CharacterManager.Singleton.Characters) {
            isAllCompelte = isAllCompelte && character.Move(CharacterDirction.LEFT);
        }
        if (isAllCompelte) {
            setOnce = false;
            currentAction = null;
            isControlling = false;
        }
    }

    private void Down() {
        if (!setOnce) {
            foreach (BaseCharacter character in CharacterManager.Singleton.Characters) {
                character.Init();
            }
            setOnce = true;
        }

        bool isAllCompelte = true;
        foreach (BaseCharacter character in CharacterManager.Singleton.Characters) {
            isAllCompelte = isAllCompelte && character.Move(CharacterDirction.DOWN);
        }

        if (isAllCompelte) {
            setOnce = false;
            currentAction = null;
            isControlling = false;
        }
    }

    private void Up() {
        if (!setOnce) {
            foreach (BaseCharacter character in CharacterManager.Singleton.Characters) {
                character.Init();
            }
            setOnce = true;
        }

        bool isAllCompelte = true;
        foreach (BaseCharacter character in CharacterManager.Singleton.Characters) {
            isAllCompelte = isAllCompelte && character.Move(CharacterDirction.UP);
        }
        if (isAllCompelte) {
            setOnce = false;
            currentAction = null;
            isControlling = false;
        }
    }
    #endregion
}
