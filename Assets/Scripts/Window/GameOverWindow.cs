using UnityEngine;
using UnityEngine.UI;

public class GameOverWindow : BaseWindow, IUpdateWindow {

    private Button nextBtn;
    private Slider slider;

    public override void InitUI() {
        base.InitUI();

        //initUI
        nextBtn = Root.Find("nextBtn").GetComponent<Button>();
        slider = Root.Find("slider").GetComponent<Slider>();

        nextBtn.onClick.AddListener(OnNextBtnClick);
    }

    bool startSlider;
    private void OnNextBtnClick() {
        nextBtn.gameObject.SetActive(false);
        slider.gameObject.SetActive(true);
        startSlider = true;
        SceneManager.StartGame.RestartAll();
    }

    public void SetContent() {
        nextBtn.gameObject.SetActive(true);
        slider.gameObject.SetActive(false);
        slider.value = 0f;
        progress = 0f;
        startSlider = false;
    }

    private float progress;
    public void Update() {
        if (!startSlider) {
            return;
        }

        progress += Time.smoothDeltaTime * 0.5f;

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
