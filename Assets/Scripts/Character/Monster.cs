using UnityEngine;
using System;
using System.Collections.Generic;

public class Monster : Character {

    private Position sightArea = new Position(4, 4);

    private SpriteRenderer life;

    public Monster(Transform container, Position position) {
        currentPosition = position;
        prefabPath = "Monster";
        go = Utils.DrawCharacter(this, container);
        animator = go.GetComponentInChildren<Animator>();
        life = go.transform.Find("life").GetComponent<SpriteRenderer>();
        currentDirction = CharacterDirection.DOWN;
        IsHero = false;
        CurLife = MaxLife = 50;
        Atk = 5;
        Exp = 50;
    }

    public override void Init() {
        onceActionMap = new Dictionary<ActionType, Action>();
        updateActionMap = new Dictionary<ActionType, Func<bool>>();
        toDoOnceActionMap = new Dictionary<ActionType, Action>();
        toDoUpdateActionMap = new Dictionary<ActionType, Func<bool>>();

        onceActionMap.Add(ActionType.Up, AllInit);
        onceActionMap.Add(ActionType.Down, AllInit);
        onceActionMap.Add(ActionType.Left, AllInit);
        onceActionMap.Add(ActionType.Right, AllInit);
        onceActionMap.Add(ActionType.Attack, AllInit);

        updateActionMap.Add(ActionType.Up, AllUpdate);
        updateActionMap.Add(ActionType.Down, AllUpdate);
        updateActionMap.Add(ActionType.Left, AllUpdate);
        updateActionMap.Add(ActionType.Right, AllUpdate);
        updateActionMap.Add(ActionType.Attack, AllUpdate);

        toDoOnceActionMap.Add(ActionType.Up, UpInit);
        toDoOnceActionMap.Add(ActionType.Down, DownInit);
        toDoOnceActionMap.Add(ActionType.Left, LeftInit);
        toDoOnceActionMap.Add(ActionType.Right, RightInit);
        toDoOnceActionMap.Add(ActionType.Attack, AttackInit);

        toDoUpdateActionMap.Add(ActionType.Up, UpUpdate);
        toDoUpdateActionMap.Add(ActionType.Down, DownUpdate);
        toDoUpdateActionMap.Add(ActionType.Left, LeftUpdate);
        toDoUpdateActionMap.Add(ActionType.Right, RightUpdate);
        toDoUpdateActionMap.Add(ActionType.Attack, AttackUpdate);
    }

    private ActionType toDoAction;
    private Dictionary<ActionType, Action> toDoOnceActionMap;
    private Dictionary<ActionType, Func<bool>> toDoUpdateActionMap;

    private void WhatToDo(Position heroPositon) {
        if (!(heroPositon - currentPosition < sightArea)) {
            toDoAction = (ActionType)UnityEngine.Random.Range(1, 5);
            return;
        }

        if (Utils.IsInAttackRange(currentPosition, heroPositon)) {
            ChangeDirction(Utils.GetDirction(currentPosition, heroPositon));
            toDoAction = ActionType.Attack;
            return;
        }

        toDoAction = (ActionType)DungeonManager.Singleton.CurrentDungeon.GetNextDirection(currentPosition);
    }

    private void AllInit() {
        WhatToDo(CharacterManager.Singleton.Hero.currentPosition);
        if (actionType != ActionType.Attack && toDoAction != ActionType.Attack) {
            toDoOnceActionMap[toDoAction].Invoke();
        } else {
            hasSetAttackInit = false;
        }
    }

    private bool hasSetAttackInit = false;
    private bool AllUpdate() {
        if (toDoAction == ActionType.Attack || actionType == ActionType.Attack) {
            if (CharacterManager.Singleton.Hero.isActing) {
                return false;
            }

            if (!hasSetAttackInit) {
                toDoOnceActionMap[toDoAction].Invoke();
                hasSetAttackInit = true;
            }
        }
        return toDoUpdateActionMap[toDoAction].Invoke();
    }

    public override void Death() {
        ResourceManager.Singleton.Unload(go);
        CharacterManager.Singleton.RemoveCharacter(this);
    }

    public void UpdateLife() {
        Debug.LogWarning("CurLife: " + CurLife + " MaxLife: " + MaxLife);
        if (CurLife == MaxLife) {
            life.gameObject.SetActive(false);
            return;
        }
        life.gameObject.SetActive(true);
        Vector3 screenPos = CharacterManager.Singleton.startGame.mainCamera.WorldToScreenPoint(life.transform.position);

        life.material.SetFloat("_Progress", (float)CurLife / MaxLife);
        life.material.SetFloat("_All", life.size.x * 100);
        life.material.SetFloat("_Xmax", screenPos.x + life.size.x * 100 / 2);
    }
}
