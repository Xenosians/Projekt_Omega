﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance { set; get; }

    public void Awake()
    {
        Instance = this; 
    }

    public void OnLocalGameButton()
    {
        Debug.Log("OnLocalGameButton");
    }

    public void OnOnlineGameButton()
    {
        Debug.Log("OnOnlineGameButton");
    }

    public void OnOnlineHostButton()
    {
        Debug.Log("OnOnlineHostButton");
    }

    public void OnOnlineConnectButton()
    {
        Debug.Log("OnOnlineConnectButton");
    }

    public void OnOnlineBackButton() 
    {
        Debug.Log("OnOnlineBackButton");
    }

    public void OnHostBackButton()
    {
        Debug.Log("OnHostBackButton");
    }
}
