using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevelopEngine;
using EventCenter;

public class DataManager : MonoSingleton<DataManager>
{
    private int score;
    public int Score { get { return score; } }
    private int time;

    public void AddScore(int value)
    {
        score += value;
        DataEvent.OnDataTriggerEvent(DataType.Score, score);
    }

    private void ReduceTime(int value)
    {
        time -= value;
        DataEvent.OnDataTriggerEvent(DataType.Time, time);
    }

    private IEnumerator ICountDown()
    {
        while (time >= 0) 
        {
            yield return new WaitForSeconds(1);
            ReduceTime(1);
        }
        ButtonEvent.OnButtonTriggerEvent(ButtonType.Over);
    }

    public void CountDown()
    {
        StartCoroutine("ICountDown");
    }

    public void StopCountDown()
    {
        StopCoroutine("ICountDown");
    }

    public void InitData()
    {
        time = 30;
        score = 0;

        DataEvent.OnDataTriggerEvent(DataType.Time, time);
        DataEvent.OnDataTriggerEvent(DataType.Score, score);
    }

    public bool IsTimeOut()
    {
        if (time <= 0)
        {
            return true;
        }
        return false;
    }
}
