using UnityEngine;
using System.Collections;

public class BackgroundScaler : MonoBehaviour {

    public Camera uiCamera;

    // Use this for initialization
    void Start() {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        float cameraHeight = uiCamera.orthographicSize * 2f;
        float cameraWigth = cameraHeight * uiCamera.aspect;
        Vector2 spriteSize = spriteRenderer.bounds.size;
        if (cameraWigth / spriteSize.x > cameraHeight / spriteSize.y) {
            transform.localScale *= cameraWigth / spriteSize.x;
        } else {
            transform.localScale *= cameraHeight / spriteSize.y;
        }
    }
}
