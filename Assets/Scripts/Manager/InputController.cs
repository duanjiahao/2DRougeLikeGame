using System;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public void Update() {

        if (isControlling) {
            if (currentAction != null) {
                currentAction.Invoke();
            }
            return;
        }

        if (Input.GetKey(KeyCode.UpArrow)) {
            Debug.Log("Up");
            isControlling = true;
            currentAction = Up;
            currentAction.Invoke();
            return;
        }

        if (Input.GetKey(KeyCode.DownArrow)) {
            Debug.Log("Down");
            isControlling = true;
            currentAction = Down;
            currentAction.Invoke();
            return;
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
            Debug.Log("Left");
            isControlling = true;
            currentAction = Left;
            currentAction.Invoke();
            return;
        }

        if (Input.GetKey(KeyCode.RightArrow)) {
            Debug.Log("Right");
            isControlling = true;
            currentAction = Right;
            currentAction.Invoke();
            return;
        }

        currentAction = null;
    }

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
}
