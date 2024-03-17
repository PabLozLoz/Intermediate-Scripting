using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] private int turnSpeed;


    private void Awake()
    {
        SaveManager.Init();
    }
    private void Update()
    {
        transform.Translate(speed * Vector3.forward * Input.GetAxis("Vertical") * Time.deltaTime);
        transform.Rotate(turnSpeed * Vector3.up * Input.GetAxis("Horizontal") * Time.deltaTime);

        if (Input.GetMouseButtonDown(0) && !GameManager.gm.loadPanel.activeSelf)
            GameManager.gm.SelectLoad();
        if (Input.GetKeyDown(KeyCode.F6) && !GameManager.gm.loadPanel.activeSelf)
        {
            GameManager.gm.Save(speed, turnSpeed, transform.position, transform.eulerAngles);
        }

        if (Input.GetKeyDown(KeyCode.F7))
        {

            GameManager.gm.LoadLastSave();
        }

    }

    public void SetLoadPositionAndRotation(GameManager.SaveObject player)
    {

        transform.position = player.playerPosition;
        transform.eulerAngles = player.playerRotation;
        speed = player.speedPlayer;
        turnSpeed = player.turnSpeedPlayer;
    }



}
