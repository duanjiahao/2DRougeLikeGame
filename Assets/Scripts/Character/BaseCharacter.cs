using UnityEngine;

public enum CharacterDirection {
    UP = 1,
    DOWN = 2,
    LEFT = 3,
    RIGHT = 4,
}

public abstract class BaseCharacter {

    public int Exp { get; set; }

    public int NeedExp{ get { return Lvl * 50; } }

    private int _curExp;
    public int CurExp{
        get {
            return _curExp;
        }
        set {
            _curExp = value;
            if (IsHero) {
                CharacterManager.Singleton.startGame.statusPanel.UpdateStatusInfo();
            }
        }
    }

    public bool IsHero { get; protected set; }

    public bool isActing;

    public int MaxLife { get; set; }

    private int _curLife;
    public int CurLife{
        get {
            return _curLife;
        } set {
            _curLife = value;
            if (IsHero) {
                CharacterManager.Singleton.startGame.statusPanel.UpdateStatusInfo();
            }
        }
    }

    public int Atk { get; set; }

    private int _lvl;
    public int Lvl { 
        get {
            return _lvl;
        } set {
            _lvl = value;
            if (IsHero) {
                CharacterManager.Singleton.startGame.statusPanel.UpdateStatusInfo();
            }
        }
    }

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
