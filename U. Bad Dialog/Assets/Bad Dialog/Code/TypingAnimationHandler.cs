using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class TypingAnimationHandler : MonoBehaviour
{
    [SerializeField] TMP_Text textCompo;
    [SerializeField] [Range(0.0001f,0.5f)] float timePerChar = 0.1f;
    public event Action onComplete;
    bool running = false;
    public string Text => textCompo.text;
    public bool Running => running;

    private void Start()
    {
        FadeOut();
    }

    public void Animate(string newText) 
    {
        textCompo.text = newText;
        Animate();
    }
    void Animate()
    {
        DOTweenTMPAnimator animator = new DOTweenTMPAnimator(textCompo);
        Sequence sequence2 = DOTween.Sequence().OnComplete(() => 
        {
            print("fin");
            if (running)
                onComplete?.Invoke();
                running = false;

        }).OnStart(()=> { running = true;});

        for (int i = 0; i < animator.textInfo.characterCount; ++i)
        {
            if (!animator.textInfo.characterInfo[i].isVisible)
                continue;

            var col = animator.GetCharColor(i);
            col.a = 0f;
            animator.SetCharColor(i, col);
            animator.SetCharOffset(i, new Vector3(30, 0, 0));
            animator.SetCharScale(i, Vector3.zero);
            col.a = 1f;
            Vector3 currCharOffset = animator.GetCharOffset(i);
            sequence2.Append(animator.DOColorChar(i, col, timePerChar));
            sequence2.Join(animator.DOOffsetChar(i, Vector3.zero, timePerChar));
            sequence2.Join(animator.DOScaleChar(i, Vector3.one, timePerChar));
        }
    }

    public void FadeOut()
    {
        DOTweenTMPAnimator animator = new DOTweenTMPAnimator(textCompo);
        Sequence sequence2 = DOTween.Sequence();
        for (int i = 0; i < animator.textInfo.characterCount; ++i)
        {
            if (!animator.textInfo.characterInfo[i].isVisible)
                continue;

            var col = animator.GetCharColor(i);
            col.a = 0f;
            animator.SetCharColor(i, col);
        }
    }

    private void FadeIn() => textCompo.alpha = 1f;


    public void ShowFull() 
    {
        DOTweenTMPAnimator animator = new DOTweenTMPAnimator(textCompo);
        for (int i = 0; i < animator.textInfo.characterCount; ++i)
        {
            if (!animator.textInfo.characterInfo[i].isVisible)
                continue;
            var col = animator.GetCharColor(i);
            col.a = 1f;
            animator.SetCharColor(i, col);
            animator.SetCharOffset(i, new Vector3(0, 0, 0));
            animator.SetCharScale(i, Vector3.one);
        }
        //FadeIn();
    }

    public void ShowFull(string newText)
    {
        textCompo.text = newText;
        ShowFull();
        running = false;
    }
}
