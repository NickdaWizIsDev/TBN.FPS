using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public MyInputs input;
    public MyInputs.PlayerActions actions;

    private void Awake()
    {
        input = new MyInputs();
        actions = input.Player;
    }    

    private void OnEnable()
    {
        actions.Enable();
    }

    private void OnDisable()
    {
        actions.Disable();
    }
}