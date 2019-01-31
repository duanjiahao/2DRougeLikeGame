using System;
using UnityEngine;
using System.Collections.Generic;

public enum CharacterDirection {
    UP = 1,
    DOWN = 2,
    LEFT = 3,
    RIGHT = 4,
}

public abstract class BaseCharacter {

    public Queue<Func<bool>> MoveActionQueue;
    public Queue<Func<bool>> OtherActionQueue;

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
                SceneManager.StartGame.statusPanel.UpdateStatusInfo();
            }
        }
    }

    public bool IsHero { get; protected set; }

    public bool IsActing {
        get {
            return MoveActionQueue.Count + OtherActionQueue.Count > 0;
        }
    }

    public int MaxLife { get; set; }

    private int _curLife;
    public int CurLife{
        get {
            return _curLife;
        } set {
            _curLife = value;
            if (IsHero) {
                SceneManager.StartGame.statusPanel.UpdateStatusInfo();
            } else{
                (this as Monster).UpdateLife();
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
                SceneManager.StartGame.statusPanel.UpdateStatusInfo();
            }
        }
    }

    public int Damage { get { return Atk * Lvl; } }

    public Position currentPosition;

    public CharacterDirection currentDirction;

    public string prefabPath;

    public GameObject go;

    public abstract void Init();

    public abstract bool MoveLeft();
    public abstract bool MoveRight();
    public abstract bool MoveUp();
    public abstract bool MoveDown();
    public abstract void ChangeDirction(CharacterDirection dirction);
    public abstract bool Attack();
    public abstract bool BeAttack();
    public abstract void Death();
}
