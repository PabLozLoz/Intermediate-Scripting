using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager gm;
    private const string SAVE_NAME = "save_";
    private SaveObject[] _arraySaves;
    public GameObject loadPanel;
    [SerializeField] private GameObject loadSaveButtom;

    private void Awake()
    {
        gm = this;
    }

    public void LoadLastSave()
    {
        loadPanel.SetActive(false);
        var loadJson = SaveManager.Load();
        var load = GameObject.FindGameObjectWithTag("Player");
        if (!string.IsNullOrEmpty(loadJson))
        {
            var loadedPlayer = JsonUtility.FromJson<SaveObject>(loadJson);

            if (loadedPlayer == null)
            {
                Debug.LogError("LOAD ERROR, LOADED PLAYER NULL");
                return;
            }
            load.GetComponent<PlayerMovement>().SetLoadPositionAndRotation(loadedPlayer);
        }
    }

    public void SelectLoad()
    {
        if (loadPanel.transform.childCount > 0)
            for (int i = loadPanel.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(loadPanel.transform.GetChild(i).gameObject);
            }


        var loadJson = SaveManager.SelectLoad();
        var loadedPlayer = new SaveObject[loadJson.Length];
        if (loadJson.Length != 0 && loadJson != null)
        {
            for (int i = 0; i < loadJson.Length; i++)
            {
                loadedPlayer[i] = JsonUtility.FromJson<SaveObject>(loadJson[i]);

                loadSaveButtom.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = SAVE_NAME + i;
                loadSaveButtom.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = loadedPlayer[i].saveHour ?? null;
                Instantiate(loadSaveButtom, loadPanel.transform);
            }

            if (loadedPlayer == null)
            {
                Debug.LogError("LOAD ERROR, LOADED PLAYER NULL");
                return;
            }
            _arraySaves = loadedPlayer;
            loadPanel.SetActive(true);
        }


    }

    public void Save(int speed, int turnSpeed, Vector3 position, Vector3 rotation)
    {
        var player = new SaveObject
        {

            speedPlayer = speed,
            turnSpeedPlayer = turnSpeed,
            playerPosition = position,
            playerRotation = rotation,
            saveHour = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss")

        };
        string saveJson = JsonUtility.ToJson(player);
        SaveManager.Save(saveJson);
    }

    public void ClickPlayerMovement(string hour)
    {
        var load = GameObject.FindGameObjectWithTag("Player");
        foreach (var player in _arraySaves)
        {
            if (hour == player.saveHour)
            {
                loadPanel.SetActive(false);
                load.GetComponent<PlayerMovement>().SetLoadPositionAndRotation(player);
                break;
            }
        }

    }
    public class SaveObject
    {
        public int speedPlayer;
        public int turnSpeedPlayer;
        public Vector3 playerPosition;
        public Vector3 playerRotation;
        public string saveHour;
    }
}