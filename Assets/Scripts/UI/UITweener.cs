using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum UIAnimationTypes {
    Move,
    Scale,
    ScaleX,
    ScaleY,
    Fade
}
//[ExecuteInEditMode]
public class UITweener : MonoBehaviour {

    public GameObject objectToAnimate;

    public UIAnimationTypes animationType;
    public LeanTweenType easeType;
    public float duration;
    public float delay;

    public bool loop;
    public bool pingpong;

    public bool startPositionOffset;
    public Vector3 from;
    public Vector3 to;

    public LTDescr _tweenObject;

    //public bool showOnEnable;
    //public bool workOnDisable;

    public UnityEvent onCompleteEnableCallback;
    public UnityEvent onDisableCallback;

    public void OnEnable() {
        HandleTween();
    }

    public void onCompleteEnable() {
        if (onCompleteEnableCallback != null) {
            onCompleteEnableCallback.Invoke();
        }
    }
    
    public void onDisable() {
        if (onDisableCallback != null) {
            onDisableCallback.Invoke();
        }
    }

    public void HandleTween() {

        if (objectToAnimate == null)
            objectToAnimate = gameObject;

        switch (animationType) {
            case UIAnimationTypes.Fade:
                Fade();
                break;
            case UIAnimationTypes.Move:
                MoveAbsolute();
                break;
            case UIAnimationTypes.Scale:
                Scale();
                break;
            case UIAnimationTypes.ScaleX:
                Scale();
                break;
            case UIAnimationTypes.ScaleY:
                Scale();
                break;
        }

        _tweenObject.setDelay(delay);
        _tweenObject.setEase(easeType);
        _tweenObject.setIgnoreTimeScale(true);
        _tweenObject.setOnComplete(onCompleteEnable);

        if (loop)
            _tweenObject.loopCount = int.MaxValue;

        if (pingpong)
            _tweenObject.setLoopPingPong();
    }

    public void Fade() {
        if (gameObject.GetComponent<CanvasGroup>() == null)
            gameObject.AddComponent<CanvasGroup>();

        if (startPositionOffset)
            objectToAnimate.GetComponent<CanvasGroup>().alpha = from.x;

        _tweenObject = LeanTween.alphaCanvas(objectToAnimate.GetComponent<CanvasGroup>(), to.x, duration);
    }

    public void MoveAbsolute() {
        objectToAnimate.GetComponent<RectTransform>().anchoredPosition = from;
        _tweenObject = LeanTween.move(objectToAnimate.GetComponent<RectTransform>(), to, duration);
    }

    public void Scale() {
        if (startPositionOffset)
            objectToAnimate.GetComponent<RectTransform>().localScale = from;

        _tweenObject = LeanTween.scale(objectToAnimate, to, duration);
    }

    void SwapDirection() {
        (from, to) = (to, from);
    }

    public void Disable() {
        onDisable();
        SwapDirection();
        HandleTween();
        _tweenObject.setOnComplete(() => {
            SwapDirection();
            gameObject.SetActive(false);
        });
    }

    public void DisableAfterDelay(float delay) {
        float initialDelay = this.delay;
        this.delay = delay;
        SwapDirection();
        HandleTween();
        _tweenObject.setOnComplete(() => {
            this.delay = initialDelay;
            SwapDirection();
            gameObject.SetActive(false);
            onDisable();
        });
    }

    public void SelfDestroy() {
        Destroy(this);
    }

    //public void Disable(Action onCompleteAction) {
    //    SwapDirection();
    //    HandleTween();
    //    _tweenObject.setOnComplete(() => {
    //        SwapDirection();
    //        onCompleteAction();
    //    });
    //}
}
