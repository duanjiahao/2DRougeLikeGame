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

    private static List<BaseCharacter> characterList;

    private Action currentAction = null;

    public void Init() {
        characterList = new List<BaseCharacter>();
    }

    public void AddCharacter(BaseCharacter character) {
        characterList.Add(character);
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
            return;
        }

        if (Input.GetKey(KeyCode.DownArrow)) {
            Debug.Log("Down");
            isControlling = true;
            currentAction = Down;
            return;
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
            Debug.Log("Left");
            isControlling = true;
            currentAction = Left;
            return;
        }

        if (Input.GetKey(KeyCode.RightArrow)) {
            Debug.Log("Right");
            isControlling = true;
            currentAction = Right;
            return;
        }

        currentAction = null;
    }

    private bool setOnce = false;
    private void Right() {
        if (!setOnce) {
            foreach (BaseCharacter character in characterList) {
                character.Init();
            }
            setOnce = true;
        }

        bool isAllCompelte = true;
        foreach (BaseCharacter character in characterList) {
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
            foreach (BaseCharacter character in characterList) {
                character.Init();
            }
            setOnce = true;
        }

        bool isAllCompelte = true;
        foreach (BaseCharacter character in characterList) {
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
            foreach (BaseCharacter character in characterList) {
                character.Init();
            }
            setOnce = true;
        }

        bool isAllCompelte = true;
        foreach (BaseCharacter character in characterList) {
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
            foreach (BaseCharacter character in characterList) {
                character.Init();
            }
            setOnce = true;
        }

        bool isAllCompelte = true;
        foreach (BaseCharacter character in characterList) {
            isAllCompelte = isAllCompelte && character.Move(CharacterDirction.UP);
        }
        if (isAllCompelte) {
            setOnce = false;
            currentAction = null;
            isControlling = false;
        }
    }
}
