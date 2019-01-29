using UnityEngine;

public enum CharacterDirection {
    UP = 1,
    DOWN = 2,
    LEFT = 3,
    RIGHT = 4,
}

public abstract class BaseCharacter {

    public int Exp{ get; set; }

    public int NeedExp{ get { return Lvl * 50; } }

    public int CurExp{ get; set; }

    public bool IsHero { get; protected set; }

    public bool isActing;

    public int MaxLife { get; set; }

    public int CurLife{ get; set; }

    public int Atk { get; set; }

    public int Lvl { get; set; }

    public int Damage { get { return Atk * Lvl; } }

    public Position currentPosition;

    public CharacterDirection currentDirction;

    public string prefabPath;

    public GameObject go;

    public abstract void Init();

    public abstract bool Move(CharacterDirection dirction);

    public abstract void ChangeDirction(CharacterDirection dirction);

    public abstract bool Attack();

    public abstract void Death();
}
