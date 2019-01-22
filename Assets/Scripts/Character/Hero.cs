using UnityEngine;
using System.Collections;

public class Hero : BaseCharacter {
    public Hero(Position position, Transform container) {
        currentPosition = position;
        prefabPath = "Hero";
        Utils.DrawCharacter(this, container);
    }

    public override bool Move(CharacterDirction dirction) {
        switch (dirction) {
            case CharacterDirction.DOWN:
                Down();
                break;
            case CharacterDirction.UP:
                Up();
                break;
            case CharacterDirction.LEFT:
                Left();
                break;
            case CharacterDirction.RIGHT:
                Right();
                break;
            default:
                break;
        }
        return false;
    }

    private float time = 0f;
    public bool Down() {
        time += Time.deltaTime;
        if (time > 2f) {
            time = 0f;
            return true;
        }
        return false;
    }

    public bool Up() {
        time += Time.deltaTime;
        if (time > 2f) {
            time = 0f;
            return true;
        }
        return false;
    }

    public bool Left() {
        time += Time.deltaTime;
        if (time > 2f) {
            time = 0f;
            return true;
        }
        return false;
    }

    public bool Right() {
        time += Time.deltaTime;
        if (time > 2f) {
            time = 0f;
            return true;
        }
        return false;
    }
}
