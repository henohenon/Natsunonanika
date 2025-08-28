using LitMotion;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Button))]
public class BigButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button _button;
    private RectTransform _rectTransform;
    private AudioSource _audioSource;

    private MotionHandle _handle;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _button = GetComponent<Button>();
        _rectTransform = GetComponent<RectTransform>();
        _audioSource = GetComponent<AudioSource>();
        _button.onClick.AddListener(() =>
        {
            _handle.TryComplete();
            _handle = LMotion.Create(_rectTransform.localScale, Vector3.one * 0.75f, 0.2f).Bind(x =>
            {
                _rectTransform.localScale = x;
            });
            _audioSource.Play();
        });
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _handle.TryComplete();
        _handle = LMotion.Create(_rectTransform.localScale, Vector3.one * 1.1f, 0.2f).Bind(x =>
        {
            _rectTransform.localScale = x;
        });
        Debug.Log("ホバーしたよ");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("ホバー外れたよ");
        _handle = LMotion.Create(Vector3.one * 1.1f, Vector3.one, 0.2f).Bind(x =>
        {
            _rectTransform.localScale = x;
        });
        Debug.Log("ホバーしたよ");
    }
}
