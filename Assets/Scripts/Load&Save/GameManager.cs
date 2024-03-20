using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /* Intenté hacerlo todo en el PlayerMovement para hacer una lista de datos guardados con la que
     * elegir un archivo y cargar este con la intención de seguir el video 60, pero...
     * Todo se me torció muchísimo en el momento en que me di cuenta que el botón de guardado
     * era realmente un panel y quería utilizar el On Click del prefab. Que MUCHO MÁS TARDE deseché al ver que con
     * una llamada al ButtonScript podía hacer lo mismo, pero con un evento. Hice los cambios de panel a botón y 
     * ví que el método de On Click no se guardaba en el prefab por no tener el script puesto.
     * Por lo que terminé creando ButtonScript y en este punto ya no pude evitar hacer esta clase.
     * 
     * Entonces viene todo el texto otra vez (lo siento)
     * Use de base del funcionamiento de botones en buscaminas.
    */

    [SerializeField] private GameObject loadSaveButton;
    [SerializeField] private GameObject loadPanel;
    public static GameManager GM;

    private List<ButtonScript> _listBtn;

    private SaveObject[] _arraySaves;

    private void Awake()
    {
        GM = this;
    }

    private void DeleteButtonsFromPanel()
    {
            foreach (var btn in _listBtn)
                Destroy(btn.gameObject);
            _listBtn.Clear();
    }
    private Sprite SetTextureAndGetSprite(string sourceScreenShoot)
    {
        var imageBytes = SaveManager.GetImageFromPath(sourceScreenShoot);
        var loadTexture = new Texture2D(1, 1);

        loadTexture.LoadImage(imageBytes);
        var sprite = Sprite.Create(loadTexture, new Rect(0, 0, loadTexture.width, loadTexture.height), new Vector2(0.5f, 0.5f));

        return sprite;

    }

    public bool IsLoadPanelActive()
    {
        return loadPanel.activeSelf;
    }

    public void Save(int speed, int turnSpeed, Vector3 position, Vector3 rotation)
    {
        var player = new SaveObject
        {
            speedPlayer = speed,
            turnSpeedPlayer = turnSpeed,
            playerPosition = position,
            playerRotation = rotation,
            saveHour = DateTime.Now.ToString("dd'-'MM'-'yyyy'   'HH':'mm':'ss"),
            sourceSaveImage = SaveManager.SaveImage()
        };
        string jsonSaveFile = JsonUtility.ToJson(player);
        SaveManager.Save(jsonSaveFile);
    }

    public void LoadLastSave()
    {
        loadPanel.SetActive(false);

        var jsonSaveFile = SaveManager.GetLastSave();
        var playerGameObject = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        if (!string.IsNullOrEmpty(jsonSaveFile))
        {
            var saveFile = JsonUtility.FromJson<SaveObject>(jsonSaveFile);

            if (saveFile == null)
            {
                Debug.LogError("LOAD ERROR, LOADED PLAYER NULL");
                return;
            }
            playerGameObject.transform.position = saveFile.playerPosition;
            playerGameObject.transform.eulerAngles = saveFile.playerRotation;
            playerGameObject.Speed = saveFile.speedPlayer;
            playerGameObject.TurnSpeed = saveFile.turnSpeedPlayer;
        }
    }

    public void GenerateButtonsWithSaveStates()
    {
        var jsonSaveFiles = SaveManager.ArrayOfSaves();
        _listBtn = new List<ButtonScript>();
        if (jsonSaveFiles.Length != 0 && jsonSaveFiles != null)
        {
            var arraySaveObjects = new SaveObject[jsonSaveFiles.Length];
            for (int i = 0; i < jsonSaveFiles.Length; i++)
            {
                arraySaveObjects[i] = JsonUtility.FromJson<SaveObject>(jsonSaveFiles[i]);

                var btn = Instantiate(loadSaveButton, loadPanel.transform).GetComponent<ButtonScript>();

                btn.NumberSaveTextMesh.text = Constants.SaveFiles.SAVE_NAME + i;
                btn.DateTimeTextMesh.text = arraySaveObjects[i].saveHour ?? null;
                btn.ImageButton.sprite = SetTextureAndGetSprite(arraySaveObjects[i].sourceSaveImage);
                _listBtn.Add(btn);
            }

            if (arraySaveObjects == null)
            {
                Debug.LogError("LOAD ERROR, LOADED PLAYER NULL");
                return;
            }

            _arraySaves = arraySaveObjects;
            loadPanel.SetActive(true);
        }
    }

    public void ChangePlayerPosToSelectedSavePos(string hour)
    {
        var playerGameObject = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        foreach (var save in _arraySaves)
        {
            if (hour == save.saveHour)
            {
                loadPanel.SetActive(false);
                playerGameObject.transform.position = save.playerPosition;
                playerGameObject.transform.eulerAngles = save.playerRotation;
                playerGameObject.Speed = save.speedPlayer;
                playerGameObject.TurnSpeed = save.turnSpeedPlayer;

                DeleteButtonsFromPanel();
                return;
            }
        }
    }

    private class SaveObject
    {
        public int speedPlayer;
        public int turnSpeedPlayer;
        public Vector3 playerPosition;
        public Vector3 playerRotation;
        public string saveHour;
        public string sourceSaveImage;
    }
}