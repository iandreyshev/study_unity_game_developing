﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    public Text m_pointsUI;
    public Text m_lifeUI;
    public Text m_wallUI;
    public Text m_ballsUI;

    float m_addingPointsSpeed = 0.01f;
    float m_addingTime = 0;
    int m_pointsPerOneAdd = 1;

    public void UpdateBalls(int ballsCount)
    {
        m_ballsUI.text = "Balls: " + ballsCount.ToString();
    }
    public void UpdateWall(float duration, float maxDuration)
    {
        int seconds = (int)maxDuration - (int)duration;

        if (seconds != 0)
        {
            m_wallUI.text = "Wall time: " + seconds.ToString();
        }
        else
        {
            m_wallUI.text = "Wall disable";
        }
    }
    public void UpdateLife(int lifeCount)
    {
        m_lifeUI.text = "Life: " + lifeCount.ToString();
    }
    public void UpdatePoints(int points)
    {
        int currPoints = int.Parse(m_pointsUI.text);
        int pointsToAdd = points - currPoints;

        m_addingTime += Time.deltaTime;

        if (m_addingTime >= m_addingPointsSpeed && pointsToAdd != 0)
        {
            if (pointsToAdd >= m_pointsPerOneAdd)
            {
                AddPointsToText(m_pointsPerOneAdd);
            }
            else
            {
                AddPointsToText(pointsToAdd);
            }

            m_addingTime = 0;
        }
    }
    void AddPointsToText(int points)
    {
        int currPoints = int.Parse(m_pointsUI.text);
        int newPoints = currPoints + points;

        m_pointsUI.text = newPoints.ToString();
    }
}
