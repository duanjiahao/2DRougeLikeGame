using System;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    private bool isControlling;

    private static List<BaseCharacter> characterList;

    private Action currentAction = null;

    // Use this for initialization
    void Start() {
        characterList = new List<BaseCharacter>();
    }

    public static void AddCharacter(BaseCharacter character) {
        characterList.Add(character);
    }

    // Update is called once per frame
    void Update() {

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

    private void Right() {
        bool isAllCompelte = true;
        foreach (BaseCharacter character in characterList) {
            isAllCompelte = isAllCompelte && character.Move(CharacterDirction.RIGHT);
        }
        if (isAllCompelte) {
            currentAction = null;
            isControlling = false;
        }
    }

    private void Left() {
        bool isAllCompelte = true;
        foreach (BaseCharacter character in characterList) {
            isAllCompelte = isAllCompelte && character.Move(CharacterDirction.LEFT);
        }
        if (isAllCompelte) {
            currentAction = null;
            isControlling = false;
        }
    }

    private void Down() {
        bool isAllCompelte = true;
        foreach (BaseCharacter character in characterList) {
            isAllCompelte = isAllCompelte && character.Move(CharacterDirction.DOWN);
        }
        if (isAllCompelte) {
            currentAction = null;
            isControlling = false;
        }
    }

    private void Up() {
        bool isAllCompelte = true;
        foreach (BaseCharacter character in characterList) {
            isAllCompelte = isAllCompelte && character.Move(CharacterDirction.UP);
        }
        if (isAllCompelte) {
            currentAction = null;
            isControlling = false;
        }
    }
}
