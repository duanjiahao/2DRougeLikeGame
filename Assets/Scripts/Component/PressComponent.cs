using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;


public class PressComponent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    private bool isPress;

    [FormerlySerializedAs("onPress")]
    [SerializeField]
    private UnityEvent m_onPress = new UnityEvent();

    public UnityEvent OnPress {
        get {
            return m_onPress;
        }
        set {
            m_onPress = value;
        }
    }   

    // Use this for initialization
    void Start() {
        isPress = false;
    }

    // Update is called once per frame
    void Update() {
        if (isPress) {
            if (m_onPress != null) {
                m_onPress.Invoke();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        isPress = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        isPress = false;
    }
}
