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

    public Character() {
        Lvl = 1;
        CurExp = 0;
    }

    public override void Init() {
        if (IsHero) {
            CharacterManager.Singleton.startGame.statusPanel.UpdateStatusInfo();
        }

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

    public override bool Move(CharacterDirection dirction) {
        switch (dirction) {
            case CharacterDirection.DOWN:
                actionType = ActionType.Down;
                break;
            case CharacterDirection.UP:
                actionType = ActionType.Up;
                break;
            case CharacterDirection.LEFT:
                actionType = ActionType.Left;
                break;
            case CharacterDirection.RIGHT:
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

    public override void ChangeDirction(CharacterDirection dirction) {
        ResetAllState();
        Utils.SetTriggerByDirction(animator, dirction);
        currentDirction = dirction;
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
        canMove = DungeonManager.Singleton.CurrentDungeon.CanMove(CharacterDirection.DOWN, currentPosition);
        animator.SetTrigger("Down");
        currentDirction = CharacterDirection.DOWN;
        endPosition = go.transform.localPosition + Vector3.down * Utils.TILE_SIZE;
        endPosition -= new Vector3(0, 0, 1);
        if (canMove) {
            currentPosition = currentPosition.Bottom();
        }
    }

    protected virtual bool DownUpdate() {
        if (canMove) {
            go.transform.localPosition += Vector3.down * Time.smoothDeltaTime * 2f * Utils.TILE_SIZE;
            if (go.transform.localPosition.y <= endPosition.y) {
                go.transform.localPosition = endPosition;
                return true;
            } else {
                return false;
            }
        }

        return true;
    }

    protected virtual void UpInit() {
        canMove = DungeonManager.Singleton.CurrentDungeon.CanMove(CharacterDirection.UP, currentPosition);
        currentDirction = CharacterDirection.UP;
        animator.SetTrigger("Up");
        endPosition = go.transform.localPosition + Vector3.up * Utils.TILE_SIZE;
        endPosition += new Vector3(0, 0, 1);
        if (canMove) {
            currentPosition = currentPosition.Top();
        }
    }

    protected virtual bool UpUpdate() {
        if (canMove) {
            go.transform.localPosition += Vector3.up * Time.smoothDeltaTime * 2f * Utils.TILE_SIZE;
            if (go.transform.localPosition.y >= endPosition.y) {
                go.transform.localPosition = endPosition;
                return true;
            } else {
                return false;
            }
        }
        return true;
    }

    protected virtual void LeftInit() {
        canMove = DungeonManager.Singleton.CurrentDungeon.CanMove(CharacterDirection.LEFT, currentPosition);
        animator.SetTrigger("Left");
        currentDirction = CharacterDirection.LEFT;
        endPosition = go.transform.localPosition + Vector3.left * Utils.TILE_SIZE;
        if (canMove) {
            currentPosition = currentPosition.Left();
        }
    }

    protected virtual bool LeftUpdate() {
        if (canMove) {
            go.transform.localPosition += Vector3.left * Time.smoothDeltaTime * 2f * Utils.TILE_SIZE;
            if (go.transform.localPosition.x <= endPosition.x) {
                go.transform.localPosition = endPosition;
                return true;
            } else {
                return false;
            }
        }
        return true;
    }

    protected virtual void RightInit() {
        canMove = DungeonManager.Singleton.CurrentDungeon.CanMove(CharacterDirection.RIGHT, currentPosition);
        animator.SetTrigger("Right");
        currentDirction = CharacterDirection.RIGHT;
        endPosition = go.transform.localPosition + Vector3.right * Utils.TILE_SIZE;
        if (canMove) {
            currentPosition = currentPosition.Right();
        }
    }

    protected virtual bool RightUpdate() {
        if (canMove) {
            go.transform.localPosition += Vector3.right * Time.smoothDeltaTime * 2f * Utils.TILE_SIZE;
            if (go.transform.localPosition.x >= endPosition.x) {
                go.transform.localPosition = endPosition;
                return true;
            } else {
                return false;
            }
        }
        return true;
    }

    private float time;
    private BaseCharacter character;
    protected virtual void AttackInit() {
        time = 0f;
        animator.SetTrigger("Attack");
        character = CharacterManager.Singleton.GetCharacterByPositon(Utils.GetPositonByDirction(currentPosition, currentDirction));
        if (character != null) {
            PerformAttack();
        }
    }

    protected virtual bool AttackUpdate() {
        time += Time.smoothDeltaTime;
        if (time > 1f) {
            animator.SetFloat("AttackTime", 2f);
            return true;
        }
        return false;
    }

    public override void Death() {
    }

    private void PerformAttack() {
        character.CurLife -= Damage;
        if (character.CurLife <= 0) {
            character.Death();
            PerformExp();
        }
    }

    private void PerformExp() {
        CurExp += character.Exp;
        if (CurExp >= NeedExp) {
            CurExp -= NeedExp;
            Lvl++;
            Debug.LogFormat("Level Up!!!");
        }
    }
}
