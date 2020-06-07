using TMPro;
using UnityEngine;

public class HintScript : MonoBehaviour
{
    public TextMeshProUGUI hintTitle;
    public TextMeshProUGUI hintText;

    public void SetHintTitle(string text) {
        hintTitle.text = text;
    }
    
    public void SetHintText(string text) {
        hintText.text = text;
    }
}
