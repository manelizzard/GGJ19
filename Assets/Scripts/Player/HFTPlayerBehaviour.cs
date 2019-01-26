using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HFTPlayerBehaviour : MonoBehaviour
{
    private HFTInput hftInput;
    private HFTGamepad hftGamepad;
 
    void Awake()
    {
        hftInput = GetComponent<HFTInput>();
        hftGamepad = GetComponent<HFTGamepad>();
    }

    void Update()
    {
        bool buttonPressed = hftInput.GetButtonDown("fire1");
        if (buttonPressed)
        {
            // TODO: Make something when user presses the button
            Debug.Log(string.Format("User {0} pressed the button", hftGamepad.playerName));        
        }
    }
}
