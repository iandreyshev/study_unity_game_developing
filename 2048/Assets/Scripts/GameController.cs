﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public MapLoader m_mapLoader;
    Field m_field;

    public User m_user;
    public UIController m_UIController;
    public DataController m_data;
    ScenesController m_scenesController;

    int m_mapIndex;

    public GameObject m_sceneCurtain;
    public GameObject m_gameoverPanel;
    public Button m_newGameButton;

    bool m_isGameover = false;

    private void Awake()
    {
        m_scenesController = new ScenesController();
        m_mapIndex = m_data.GetMapIndex();
        m_field = m_mapLoader.GetField(m_mapIndex);
        m_sceneCurtain.SetActive(false);
    }
    private void Start()
    {

        string userName = m_data.GetUsername();
        m_user.SetName(userName);

        StartGame();
    }
    public void StartGame()
    {
        SetGameOver(false);
        m_user.Reset();
        m_field.StartEvents();

        uint bestScore = m_data.GetBestScore(m_mapIndex);
        m_UIController.SetBestScore(bestScore);
    }

    private void FixedUpdate()
    {
        if (!m_isGameover)
        {
            GameplayUpdate();
        }
        else
        {
            GameoverUpdate();
        }
    }
    void GameplayUpdate()
    {
        if (m_field.IsAutoTurnAllowed())
        {
            m_field.SetAutoTurn(1, true);

            uint pointsToAdd = m_field.GetPointsFromLastTurn();
            m_user.AddPoints(pointsToAdd);
        }

        CheckGameStatus();
    }
    void GameoverUpdate()
    {

    }

    void CheckGameStatus()
    {
        if (!m_field.IsTurnPossible())
        {
            Debug.Log("GameOver");
            SetGameOver(true);
        }
    }

    void SetGameOver(bool isGameover)
    {
        m_isGameover = isGameover;
        m_gameoverPanel.SetActive(m_isGameover);
        m_newGameButton.interactable = !m_isGameover;

        if (isGameover)
        {
            GameoverEvents();
        }
    }
    void GameoverEvents()
    {
        m_gameoverPanel.GetComponent<Animation>().Play();

        uint userPoints = m_user.GetPoints();
        string userName = m_user.GetName();

        m_data.SetBestScore(m_mapIndex, userPoints, userName);
    }

    public void BackTomenu()
    {
        StartCoroutine(m_scenesController.SetMenuScene());
        m_sceneCurtain.SetActive(true);
        m_sceneCurtain.GetComponent<Animation>().Play();
    }
}