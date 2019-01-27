using UnityEngine;

public static class Utils {

    public const float TILE_SIZE = 50f;

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

    public static void SetTriggerByDirction(Animator animator, CharacterDirction dirction) {
        switch (dirction) {
            case CharacterDirction.DOWN:
                animator.SetTrigger("Down");
                break;
            case CharacterDirction.UP:
                animator.SetTrigger("Up");
                break;
            case CharacterDirction.LEFT:
                animator.SetTrigger("Left");
                break;
            case CharacterDirction.RIGHT:
                animator.SetTrigger("Right");
                break;
            default:
                break;
        }
    }

    public static CharacterDirction GetDirction(Position myPos, Position youPos) {
        if (myPos.Left() == youPos) {
            return CharacterDirction.LEFT;
        }
        if (myPos.Bottom() == youPos) {
            return CharacterDirction.DOWN;
        }
        if (myPos.Right() == youPos) {
            return CharacterDirction.RIGHT;
        }
        if (myPos.Top() == youPos) {
            return CharacterDirction.UP;
        }
        return CharacterDirction.DOWN;
    }

    public static Position GetPositonByDirction(Position myPos, CharacterDirction dirction) {
        switch (dirction) {
            case CharacterDirction.DOWN:
                return myPos.Bottom();
            case CharacterDirction.UP:
                return myPos.Top();
            case CharacterDirction.LEFT:
                return myPos.Left();
            case CharacterDirction.RIGHT:
                return myPos.Right();
            default:
                return myPos;
        }
    }

    public static bool IsInAttackRange(Position myPosition, Position youPosition) {
        return myPosition.Left() == youPosition || myPosition.Right() == youPosition || myPosition.Top() == youPosition || myPosition.Bottom() == youPosition;
    }
}
