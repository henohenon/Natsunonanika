using System;
using LitMotion;
using R3;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasGroup))]
    public class UIPopController: MonoBehaviour
    {
        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;
        
        private void Awake()
        {
            if(_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
            if(_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>();
        }

        private MotionHandle motion;

        public void Popup()
        {
            SetActive(true);
            motion.TryComplete();
            motion = LMotion.Create(Vector2.zero, Vector2.one, 0.2f).Bind(x =>
            {
                _rectTransform.localScale = x;
            });
        }
        public void Popout()
        {
            if(_canvasGroup == null) Awake();
            _canvasGroup.blocksRaycasts = false;
            motion.TryComplete();
            motion = LMotion.Create(Vector2.one, Vector2.zero, 0.2f).WithOnComplete(() =>
            {
                SetActive(false);
            }).Bind(x =>
            {
                _rectTransform.localScale = x;
            });
        }

        public void SetActive(bool active)
        {
            //gameObject.SetActive(active);
            _rectTransform.localScale = active ? Vector2.one : Vector2.zero;
            if(_canvasGroup == null) Awake();
            _canvasGroup.blocksRaycasts = active;
        }

        private void OnDisable()
        {
            motion.TryComplete();
        }
    }
}