using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Lo que hice fue que los if's llamaran a los métodos que quería que funcionaran

    public int Speed { get; set; }

    public int TurnSpeed { get; set; }

    private float _horizontal;
    private float _vertical;

    private void Awake()
    {
        Speed = 2;
        TurnSpeed = 45;

        SaveManager.Init();
    }

    private void Update()
    {
        GetInputPlayer();
        Move();
        TurnMove();

        if (Input.GetMouseButtonDown(0) && !GameManager.GM.IsLoadPanelActive())
        {
            GameManager.GM.GenerateButtonsWithSaveStates();
        }

        if (Input.GetKeyDown(KeyCode.F6) && !GameManager.GM.IsLoadPanelActive())
        {
            GameManager.GM.Save(Speed, TurnSpeed, transform.position, transform.eulerAngles);
        }

        if (Input.GetKeyDown(KeyCode.F7))
        {
            GameManager.GM.LoadLastSave();
        }
    }

    private void TurnMove()
    {
        transform.Translate(_vertical * Speed * Time.deltaTime * Vector3.forward);
    }

    private void Move()
    {
        transform.Rotate(_horizontal * TurnSpeed * Time.deltaTime * Vector3.up);
    }

    private void GetInputPlayer()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
    }
}
