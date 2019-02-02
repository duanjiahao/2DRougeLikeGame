using UnityEngine;
using UnityEngine.UI;

public class NextLayerWindow : BaseWindow, IUpdateWindow {

    private Slider slider;
    private Text text;

    public override void InitUI() {
        base.InitUI();

        text = Root.Find("text").GetComponent<Text>();
        slider = Root.Find("slider").GetComponent<Slider>();
    }

    public void SetContent() {
        slider.gameObject.SetActive(true);
        slider.value = 0f;
        time = 0f;
        progress = 0f;
        text.text = string.Format("第{0}层", DungeonManager.Singleton.Layer);
        SceneManager.StartGame.RestartAll();
    }

    private float delay = 0.5f;
    private float time;
    private float progress;
    public void Update() {
        time += Time.smoothDeltaTime;
        if (time < delay) {
            return;
        }

        progress += Time.smoothDeltaTime;

        float limit = (float)SceneManager.StartGame.currentStep / (int)StartGame.RestartStep.AllComplete;
        if (progress > limit) {
            progress = limit;
        }

        slider.value = progress;
        if (progress >= .99f) {
            Hide();
        }
    }
}
