using UnityEngine;

public class ButtonNextDialog:MonoBehaviour
{
    [SerializeField] private DialogHandler dialogManager;
    private void Start()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        dialogManager.Action();
    }
}