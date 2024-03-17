using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    private Button _buttonPrefab;

    private void Awake()
    {
        _buttonPrefab = GetComponent<Button>();
    }

    public void Click()
    {
        GameManager.gm.ClickPlayerMovement(_buttonPrefab.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text);
    }

}
