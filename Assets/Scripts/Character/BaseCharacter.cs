using UnityEngine;

public enum CharacterDirction {
    UP = 1,
    DOWN = 2,
    LEFT = 3,
    RIGHT = 4,
}

public abstract class BaseCharacter {

    public bool isActing;

    public Position currentPosition;

    public CharacterDirction currentDirction;

    public string prefabPath;

    public GameObject go;

    public abstract void Init();

    public abstract bool Move(CharacterDirction dirction);

    public abstract void ChangeDirction(CharacterDirction dirction);

    public abstract bool Attack();
}
