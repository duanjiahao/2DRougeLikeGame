using UnityEngine;
using System;
using System.Collections.Generic;

public class Monster : Character {

    private Position sightArea = new Position(3, 3);

    public Monster(Transform container, Position position) {
        currentPosition = position;
        prefabPath = "Hero";
        go = Utils.DrawCharacter(this, container);
        animator = go.GetComponentInChildren<Animator>();
        currentDirction = CharacterDirction.DOWN;
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

        Position gap = heroPositon - currentPosition;
        if (Mathf.Abs(gap.row) > Mathf.Abs(gap.col)) {
            toDoAction = gap.row > 0 ? ActionType.Up : ActionType.Down;
        } else {
            toDoAction = gap.col > 0 ? ActionType.Right : ActionType.Left;
        }
    }

    private void AllInit() {
        // 这里的actionType表示的是英雄的动作
        if (actionType == ActionType.Attack) {
            if (currentPosition == Utils.GetPositonByDirction(CharacterManager.Singleton.Hero.currentPosition, CharacterManager.Singleton.Hero.currentDirction)) { 
                ChangeDirction(Utils.GetDirction(currentPosition, CharacterManager.Singleton.Hero.currentPosition));
                // 受到攻击

            }

            WhatToDo(CharacterManager.Singleton.Hero.currentPosition);
            if (toDoAction != ActionType.Attack) {
                toDoOnceActionMap[toDoAction].Invoke();
            } else {
                hasSetAttackInit = false;
            }
            return;
        }

        WhatToDo(Utils.GetPositonByDirction(CharacterManager.Singleton.Hero.currentPosition, (CharacterDirction)actionType));
        if (toDoAction != ActionType.Attack) {
            toDoOnceActionMap[toDoAction].Invoke();
        } else {
            hasSetAttackInit = false;
        }
    }

    private bool hasSetAttackInit = false;
    private bool AllUpdate() {
        if (toDoAction == ActionType.Attack) {
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
}
