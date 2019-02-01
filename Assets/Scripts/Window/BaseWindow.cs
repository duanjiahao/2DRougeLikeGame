using UnityEngine;

public class BaseWindow : Window {

    public Transform Root { get; set; }
    public GameObject GameObject { get; set; }

    public virtual void InitUI() {
        Root = GameObject.transform;
    }

    public override void Hide() {
        GameObject.SetActive(false);
    }

    public override void Show() {
        GameObject.SetActive(true);
    }
}
