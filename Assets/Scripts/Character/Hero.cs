using UnityEngine;

public class Hero : BaseCharacter {

    private Animator animator;

    public Hero(Transform container) {
        currentPosition = DungeonManager.Singleton.CurrentDungeon.StartPoint;
        prefabPath = "Hero";
        go = Utils.DrawCharacter(this, container);
        animator = go.GetComponentInChildren<Animator>();
    }

    public override bool Move(CharacterDirction dirction) {
        switch (dirction) {
            case CharacterDirction.DOWN:
                return Down();
            case CharacterDirction.UP:
                return Up();
            case CharacterDirction.LEFT:
                return Left();
            case CharacterDirction.RIGHT:
                return Right();
            default:
                break;
        }
        return false;
    }

    private bool calculateComplete;
    private bool hasSetOnce;
    private float time;
    public override void Init() {
        hasSetOnce = false;
        calculateComplete = false;
        animator.ResetTrigger("Up");
        animator.ResetTrigger("Down");
        animator.ResetTrigger("Left");
        animator.ResetTrigger("Right");
    }

    private Vector3 endPosition;
    private bool canMove;
    public bool Down() {
        if (calculateComplete) {
            return true;
        }

        if (!hasSetOnce) {
            canMove = DungeonManager.Singleton.CurrentDungeon.CanMove(CharacterDirction.DOWN, currentPosition);
            animator.SetTrigger("Down");
            endPosition = go.transform.localPosition + Vector3.down * Utils.TILE_SIZE;
            hasSetOnce = true;
        }

        if (canMove) {
            go.transform.localPosition += Vector3.down * Time.deltaTime * 2f * Utils.TILE_SIZE;
            if (go.transform.localPosition.y < endPosition.y) {
                go.transform.localPosition = endPosition;
                currentPosition = currentPosition.Bottom();
                calculateComplete = true;
                return true;
            } else {
                return false;
            }
        }

        return true;
    }

    public bool Up() {
        if (calculateComplete) {
            return true;
        }

        if (!hasSetOnce) {
            canMove = DungeonManager.Singleton.CurrentDungeon.CanMove(CharacterDirction.UP, currentPosition);
            animator.SetTrigger("Up");
            endPosition = go.transform.localPosition + Vector3.up * Utils.TILE_SIZE;
            hasSetOnce = true;
        }

        if (canMove) {
            go.transform.localPosition += Vector3.up * Time.deltaTime * 2f * Utils.TILE_SIZE;
            if (go.transform.localPosition.y > endPosition.y) {
                go.transform.localPosition = endPosition;
                currentPosition = currentPosition.Top();
                calculateComplete = true;
                return true;
            } else {
                return false;
            }
        }

        return true;
    }

    public bool Left() {
        if (calculateComplete) {
            return true;
        }

        if (!hasSetOnce) {
            canMove = DungeonManager.Singleton.CurrentDungeon.CanMove(CharacterDirction.LEFT, currentPosition);
            animator.SetTrigger("Left");
            endPosition = go.transform.localPosition + Vector3.left * Utils.TILE_SIZE;
            hasSetOnce = true;
        }

        if (canMove) {
            go.transform.localPosition += Vector3.left * Time.deltaTime * 2f * Utils.TILE_SIZE;
            if (go.transform.localPosition.x < endPosition.x) {
                go.transform.localPosition = endPosition;
                currentPosition = currentPosition.Left();
                calculateComplete = true;
                return true;
            } else {
                return false;
            }
        }

        return true;
    }

    public bool Right() {
        if (calculateComplete) {
            return true;
        }

        if (!hasSetOnce) {
            canMove = DungeonManager.Singleton.CurrentDungeon.CanMove(CharacterDirction.RIGHT, currentPosition);
            animator.SetTrigger("Right");
            endPosition = go.transform.localPosition + Vector3.right * Utils.TILE_SIZE;
            hasSetOnce = true;
        }

        if (canMove) {
            go.transform.localPosition += Vector3.right * Time.deltaTime * 2f * Utils.TILE_SIZE;
            if (go.transform.localPosition.x > endPosition.x) {
                go.transform.localPosition = endPosition;
                currentPosition = currentPosition.Right();
                calculateComplete = true;
                return true;
            } else {
                return false;
            }
        }

        return true;
    }
}
