using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    //Hago variable public y private para evitar poder acceder al prefab y modificarlo

    //Para dejar de utilizar On Click() desde el hierachy de unity he generado un evento
    //cuando despierta el botón, añadiéndole un AddListener cuando lo pulsan. Este me llama
    //al método SentDataFromButtonClicked()

    public TextMeshProUGUI NumberSaveTextMesh => numberSaveTextMesh;
    [SerializeField] private TextMeshProUGUI numberSaveTextMesh;

    public TextMeshProUGUI DateTimeTextMesh => dateTimeTextMesh;
    [SerializeField] private TextMeshProUGUI dateTimeTextMesh;

    public Image ImageButton => imageButton;
    [SerializeField] private Image imageButton;

    [SerializeField] private Button prefabButton;

    private void Awake()
    {
        prefabButton.onClick.AddListener(SentDataFromButtonClicked);
    }

    public void SentDataFromButtonClicked()
    {
        GameManager.GM.ChangePlayerPosToSelectedSavePos(DateTimeTextMesh.text);
    }

    private void OnDisable()
    {
        prefabButton.onClick.RemoveListener(SentDataFromButtonClicked);
    }

}
