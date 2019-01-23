using UnityEngine;

public enum CharacterDirction {
    UP = 1,
    DOWN = 2,
    LEFT = 3,
    RIGHT = 4,
}

public abstract class BaseCharacter {

    public Position currentPosition;

    public string prefabPath;

    public GameObject go;

    public abstract void Init();

    public abstract bool Move(CharacterDirction dirction);
}
