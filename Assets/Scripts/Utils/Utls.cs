using UnityEngine;

public static class Utils {

    public const float TILE_SIZE = 1f;

    public static GameObject DrawTile(Position pos, BaseTile tile, Transform transform) {
        GameObject go = ResourceManager.Singleton.Instantiate(tile.image, transform);
        FlipGameObject(go, tile.dirction);
        go.transform.localPosition = new Vector2(pos.col * TILE_SIZE, pos.row * TILE_SIZE);
        return go;
    }

    public static GameObject DrawCharacter(BaseCharacter character, Transform transform) {
        GameObject go = ResourceManager.Singleton.Instantiate(character.prefabPath, transform);
        go.transform.localPosition = new Vector2(character.currentPosition.col * TILE_SIZE, character.currentPosition.row * TILE_SIZE);
        return go;
    }

    public static void FlipGameObject(GameObject gameObject, ImageDirction dirction) {
        switch (dirction) {
            case ImageDirction.UP:
                gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
                break;
            case ImageDirction.DOWN:
                gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180f));
                break;
            case ImageDirction.LEFT:
                gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90f));
                break;
            case ImageDirction.RIGHT:
                gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90f));
                break;
            default:
                break;
        }
    }

    public static void SetTriggerByDirction(Animator animator, CharacterDirection dirction) {
        switch (dirction) {
            case CharacterDirection.DOWN:
                animator.SetTrigger("Down");
                break;
            case CharacterDirection.UP:
                animator.SetTrigger("Up");
                break;
            case CharacterDirection.LEFT:
                animator.SetTrigger("Left");
                break;
            case CharacterDirection.RIGHT:
                animator.SetTrigger("Right");
                break;
            default:
                break;
        }
    }

    public static CharacterDirection GetDirction(Position myPos, Position youPos) {
        if (myPos.Left() == youPos) {
            return CharacterDirection.LEFT;
        }
        if (myPos.Bottom() == youPos) {
            return CharacterDirection.DOWN;
        }
        if (myPos.Right() == youPos) {
            return CharacterDirection.RIGHT;
        }
        if (myPos.Top() == youPos) {
            return CharacterDirection.UP;
        }
        return CharacterDirection.DOWN;
    }

    public static Position GetPositonByDirction(Position myPos, CharacterDirection dirction) {
        switch (dirction) {
            case CharacterDirection.DOWN:
                return myPos.Bottom();
            case CharacterDirection.UP:
                return myPos.Top();
            case CharacterDirection.LEFT:
                return myPos.Left();
            case CharacterDirection.RIGHT:
                return myPos.Right();
            default:
                return myPos;
        }
    }

    public static bool IsInAttackRange(Position myPosition, Position youPosition) {
        Position gap = myPosition - youPosition;
        return gap == Position.P01 || gap == Position.P10 || gap == Position.P01 * -1 || gap == Position.P10 * -1;
    }

    public static CharacterDirection InverseDirection(CharacterDirection characterDirction) {
        switch (characterDirction) {
            case CharacterDirection.DOWN:
                return CharacterDirection.UP;
            case CharacterDirection.UP:
                return CharacterDirection.DOWN;
            case CharacterDirection.LEFT:
                return CharacterDirection.RIGHT;
            case CharacterDirection.RIGHT:
                return CharacterDirection.LEFT;
            default:
                return characterDirction;
        }
    }
}
