using UnityEngine;
using System;
using System.Collections.Generic;

public class Character : BaseCharacter {

    public enum ActitonType {
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4,
        Attack = 5,
    }

    public ActionType actionType;
    protected Animator animator;

    protected Dictionary<ActionType, Action> onceActionMap; 
    protected Dictionary<ActionType, Func<bool>> updateActionMap; 

    public override void Init() {
        onceActionMap = new Dictionary<ActionType, Action>();
        updateActionMap = new Dictionary<ActionType, Func<bool>>();

        onceActionMap.Add(ActionType.Up, UpInit);
        onceActionMap.Add(ActionType.Down, DownInit);
        onceActionMap.Add(ActionType.Left, LeftInit);
        onceActionMap.Add(ActionType.Right, RightInit);
        onceActionMap.Add(ActionType.Attack, AttackInit);

        updateActionMap.Add(ActionType.Up, UpUpdate);
        updateActionMap.Add(ActionType.Down, DownUpdate);
        updateActionMap.Add(ActionType.Left, LeftUpdate);
        updateActionMap.Add(ActionType.Right, RightUpdate);
        updateActionMap.Add(ActionType.Attack, AttackUpdate);
    }

    public override bool Move(CharacterDirction dirction) {
        switch (dirction) {
            case CharacterDirction.DOWN:
                actionType = ActionType.Down;
                break;
            case CharacterDirction.UP:
                actionType = ActionType.Up;
                break;
            case CharacterDirction.LEFT:
                actionType = ActionType.Left;
                break;
            case CharacterDirction.RIGHT:
                actionType = ActionType.Right;
                break;
            default:
                return true;
        }

        return Update();
    }

    public override bool Attack() {
        actionType = ActionType.Attack;
        return Update();
    }

    public override void ChangeDirction(CharacterDirction dirction) {
        Utils.SetTriggerByDirction(animator, dirction);
    }

    private bool hasSetOnce;
    protected virtual bool Update() {
        if (!isActing) {
            return true;
        }

        if (!hasSetOnce && onceActionMap.ContainsKey(actionType)) {
            ResetAllState();
            onceActionMap[actionType].Invoke();
            hasSetOnce = true;
        }

        if (updateActionMap.ContainsKey(actionType) && updateActionMap[actionType].Invoke()) {
            isActing = false;
            hasSetOnce = false;
            return true;
        } else {
            return false;
        }
    }

    private void ResetAllState() {
        animator.ResetTrigger("Up");
        animator.ResetTrigger("Down");
        animator.ResetTrigger("Left");
        animator.ResetTrigger("Right");
        animator.ResetTrigger("Attack");
        animator.SetFloat("AttackTime", 0f);
    }

    private Vector3 endPosition;
    private bool canMove;
    protected virtual void DownInit() {
        canMove = DungeonManager.Singleton.CurrentDungeon.CanMove(CharacterDirction.DOWN, currentPosition);
        animator.SetTrigger("Down");
        currentDirction = CharacterDirction.DOWN;
        endPosition = go.transform.localPosition + Vector3.down * Utils.TILE_SIZE;
    }

    protected virtual bool DownUpdate() {
        if (canMove) {
            go.transform.localPosition += Vector3.down * Time.smoothDeltaTime * 2f * Utils.TILE_SIZE;
            if (go.transform.localPosition.y <= endPosition.y) {
                go.transform.localPosition = endPosition;
                currentPosition = currentPosition.Bottom();
                return true;
            } else {
                return false;
            }
        }

        return true;
    }

    protected virtual void UpInit() {
        canMove = DungeonManager.Singleton.CurrentDungeon.CanMove(CharacterDirction.UP, currentPosition);
        currentDirction = CharacterDirction.UP;
        animator.SetTrigger("Up");
        endPosition = go.transform.localPosition + Vector3.up * Utils.TILE_SIZE;
    }

    protected virtual bool UpUpdate() {
        if (canMove) {
            go.transform.localPosition += Vector3.up * Time.smoothDeltaTime * 2f * Utils.TILE_SIZE;
            if (go.transform.localPosition.y >= endPosition.y) {
                go.transform.localPosition = endPosition;
                currentPosition = currentPosition.Top();
                return true;
            } else {
                return false;
            }
        }
        return true;
    }

    protected virtual void LeftInit() {
        canMove = DungeonManager.Singleton.CurrentDungeon.CanMove(CharacterDirction.LEFT, currentPosition);
        animator.SetTrigger("Left");
        currentDirction = CharacterDirction.LEFT;
        endPosition = go.transform.localPosition + Vector3.left * Utils.TILE_SIZE;
    }

    protected virtual bool LeftUpdate() {
        if (canMove) {
            go.transform.localPosition += Vector3.left * Time.smoothDeltaTime * 2f * Utils.TILE_SIZE;
            if (go.transform.localPosition.x <= endPosition.x) {
                go.transform.localPosition = endPosition;
                currentPosition = currentPosition.Left();
                return true;
            } else {
                return false;
            }
        }
        return true;
    }

    protected virtual void RightInit() {
        canMove = DungeonManager.Singleton.CurrentDungeon.CanMove(CharacterDirction.RIGHT, currentPosition);
        animator.SetTrigger("Right");
        currentDirction = CharacterDirction.RIGHT;
        endPosition = go.transform.localPosition + Vector3.right * Utils.TILE_SIZE;
    }

    protected virtual bool RightUpdate() {
        if (canMove) {
            go.transform.localPosition += Vector3.right * Time.smoothDeltaTime * 2f * Utils.TILE_SIZE;
            if (go.transform.localPosition.x >= endPosition.x) {
                go.transform.localPosition = endPosition;
                currentPosition = currentPosition.Right();
                return true;
            } else {
                return false;
            }
        }
        return true;
    }

    private float time;
    protected virtual void AttackInit() {
        time = 0f;
        animator.SetTrigger("Attack");
    }

    protected virtual bool AttackUpdate() {
        time += Time.smoothDeltaTime;
        if (time > 1f) {
            animator.SetFloat("AttackTime", 2f);
            return true;
        }
        return false;
    }
}
