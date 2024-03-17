using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSaveData : MonoBehaviour
{
    private int _number = 3;

    private void Start()
    {
        Debug.Log(_number);
        SaveData();
        _number = 4;

        Debug.Log(_number);
        _number = LoadData();

        Debug.Log(_number);
    }

    private void Update()
    {

    }

    private void SaveData()
    {
        //Puede guardar tres tipos de datos entre sesiones. String key y float/int/string value.
        //No es seguro, no es recomendable para muchos datos.
        //lo ideal es con un json, aunque lo top es binario
        PlayerPrefs.SetInt("Number", _number);
    }

    private int LoadData()
    {

        return PlayerPrefs.GetInt("Number");
    }
}
