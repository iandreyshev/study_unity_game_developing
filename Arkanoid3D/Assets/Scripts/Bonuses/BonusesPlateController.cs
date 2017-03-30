﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusesPlateController : MonoBehaviour
{
    public GameObject m_basicPlate;
    BonusPlate[] m_plates;
    byte[] m_moveMap;

    BonusPlate m_attack;
    BonusPlate m_wall;
    BonusPlate m_fireBall;
    BonusPlate m_multyBall;
    BonusPlate m_multiplier;

    Vector3 m_plateInnerSize;

    const int PLATES_COUNT = 5;

    private void Awake()
    {
        m_plates = new BonusPlate[PLATES_COUNT];
        m_moveMap = new byte[PLATES_COUNT];

        for (int i = 0; i < PLATES_COUNT; i++)
        {
            m_plates[i] = null;
            m_moveMap[i] = 0;
        }

        m_plateInnerSize = gameObject.GetComponent<RectTransform>().rect.size;
    }

    void Start()
    {
        InitPlate(ref m_attack, "Attack");
        InitPlate(ref m_wall, "Wall");
        InitPlate(ref m_fireBall, "FireBall");
        InitPlate(ref m_multyBall, "MultyBall!");
        InitPlate(ref m_multiplier, "Multiplier Up+");

        m_multiplier.DurationInValue(false);
        m_multyBall.DurationInValue(false);
    }
    void InitPlate(ref BonusPlate plate, string name)
    {
        Vector2 plateSize = new Vector2(m_plateInnerSize.x, m_plateInnerSize.y / PLATES_COUNT);
        plate = Instantiate(m_basicPlate).GetComponent<BonusPlate>();
        plate.transform.SetParent(transform);
        plate.Init(name, plateSize);
    }

    void FixedUpdate()
    {
        ClearPlates();
        MovePlates();
        //MovePlates(m_moveMap);
    }

    void ClearPlates()
    {
        for (int i = 0; i < m_plates.Length; i++)
        {
            if (m_plates[i] != null && !m_plates[i].IsActive())
            {
                m_plates[i].Exit();
                m_plates[i] = null;
            }
        }
    }
    void MovePlates()
    {
        for (int i = 1; i < m_plates.Length; i++)
        {
            BonusPlate currPlate = m_plates[i];
            if (currPlate == null)
            {
                continue;
            }

            m_plates[i] = null;
            int currPosition = i;
            byte moveCount = 0;

            while (currPosition > 0 && m_plates[currPosition - 1] == null)
            {
                currPosition--;
                moveCount++;
            }

            currPlate.Move(moveCount);
            m_plates[currPosition] = currPlate;
        }
    }

    void AddPlate(ref BonusPlate plate, float duration)
    {
        plate.AddDuration(duration);
        EnterPlateIf(ref plate);

    }
    public void AddAttack(float duration)
    {
        AddPlate(ref m_attack, duration);
    }
    public void AddWall(float duration)
    {
        AddPlate(ref m_wall, duration);
    }
    public void AddFireBall(float duration)
    {
        AddPlate(ref m_fireBall, duration);
    }
    public void AddMultyball(float duration)
    {
        AddPlate(ref m_multyBall, duration);
    }
    public void AddMultiplier(float duration)
    {
        AddPlate(ref m_multiplier, duration);
    }

    Vector3 GetNewPlatePosition(int order)
    {
        Vector3 position = new Vector3(0 , m_plateInnerSize.y / PLATES_COUNT * order, 0);

        return (-1 * position);
    }
    void EnterPlateIf(ref BonusPlate plate)
    {
        bool isInList = false;

        for (int i = 0; i < m_plates.Length; i++)
        {
            if (m_plates[i] == plate)
            {
                isInList = true;
                break;
            }
        }

        if (!isInList)
        {
            plate.Enter();

            for (int i = 0; i < m_plates.Length; i++)
            {
                if (m_plates[i] == null)
                {
                    Vector2 position = GetNewPlatePosition(i);
                    m_plates[i] = plate;
                    m_plates[i].SetPosition(position);
                    break;
                }
            }
        }
    }

    string GetNameIf(ref BonusPlate plate)
    {
        string name = "null";

        if (plate != null)
        {
            name = plate.name;
        }

        return name + " ";
    }
}