using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class DialogHandler : MonoBehaviour
{
    [SerializeField] DialogState state = DialogState.Playing;
    [SerializeField] private UnityEvent onDialogsCompleted;
    [SerializeField] private bool autoShow = true;
    public event Action OnDialogsCompleted;
    public List<string> dialogLines;
    public TypingAnimationHandler typingAnimationHandler;
    int index = 0;

    



    private void OnEnable()
    {
        typingAnimationHandler.onComplete += StopByCompleted;
    }

    private void OnDisable()
    {
        typingAnimationHandler.onComplete -= StopByCompleted;
    }

    private void StopByCompleted() 
    {
        if (state == DialogState.Playing)
        {
            state = DialogState.Stopped;
        }
    }

    public void Action()
    {
        NextState();
        switch (state)
        {
            case DialogState.Empty:
                ResetText();
                NextDialog();
                if(autoShow)
                    Action();
                break;
            case DialogState.Playing:
                PlayDialog();
                break;
            case DialogState.Stopped:
                StopDialog();
                break;
            default:
                break;
        }
    }

    public void ResetText()=> typingAnimationHandler.FadeOut();

    public void PlayDialog() => typingAnimationHandler.Animate(dialogLines[index]);

    private void StopDialog()
    {
        typingAnimationHandler.ShowFull(dialogLines[index]);
        if (!typingAnimationHandler.Running)
        { 
            NextDialog();
            NextState();
        }
    }

    public void NextState()
    {
        int total = Enum.GetNames(typeof(DialogState)).Length;
        state = (DialogState)(((int)state + 1) % total);
    }

    public void NextDialog()
    {
        index++;
        if (index >= dialogLines.Count)
        {
            index = 0;
            onDialogsCompleted?.Invoke();
            OnDialogsCompleted?.Invoke();
        }
    }
}

public enum DialogState
{
    Empty,
    Playing,
    Stopped
}
