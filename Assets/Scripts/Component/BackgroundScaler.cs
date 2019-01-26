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
        if (cameraWigth > cameraHeight) {
            transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 90f));
            transform.localScale *= cameraWigth / spriteSize.y;
        } else {
            transform.localRotation = Quaternion.identity;
            transform.localScale *= cameraHeight / spriteSize.y;
        }
    }
}
