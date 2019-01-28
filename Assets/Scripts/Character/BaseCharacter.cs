using UnityEngine;

public enum CharacterDirection {
    UP = 1,
    DOWN = 2,
    LEFT = 3,
    RIGHT = 4,
}

public abstract class BaseCharacter {

    public bool isActing;

    public Position currentPosition;

    public CharacterDirection currentDirction;

    public string prefabPath;

    public GameObject go;

    public abstract void Init();

    public abstract bool Move(CharacterDirection dirction);

    public abstract void ChangeDirction(CharacterDirection dirction);

    public abstract bool Attack();
}
