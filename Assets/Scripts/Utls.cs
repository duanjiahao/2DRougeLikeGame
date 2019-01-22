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
}
