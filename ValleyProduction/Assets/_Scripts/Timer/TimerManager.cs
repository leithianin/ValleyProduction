using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : VLY_Singleton<TimerManager>
{
    public class Timer
    {
        private float endTime;

        private Action callback;

        public float EndTime => endTime;

        public void SetAsGame(float duration, Action nCallback)
        {
            endTime = Time.time + duration;
            callback = nCallback;
        }

        public void SetAsReal(float duration, Action nCallback)
        {
            endTime = Time.unscaledTime + duration;
            callback = nCallback;
        }

        public void Stop()
        {
            if (gameTimer.Contains(this))
            {
                gameTimer.Remove(this);
            }
            else
            {
                realTimer.Remove(this);
            }
        }

        public void Execute()
        {
            callback?.Invoke();
            Stop();
        }
    }

    private static List<Timer> gameTimer = new List<Timer>();
    private static List<Timer> realTimer = new List<Timer>();

    public static Timer CreateGameTimer(float time, Action callback)
    {
        Timer toReturn = new Timer();
        toReturn.SetAsGame(time, callback);
        instance.AddGameTimer(toReturn);
        return toReturn;
    }

    public static Timer CreateRealTimer(float time, Action callback)
    {
        Timer toReturn = new Timer();
        toReturn.SetAsReal(time, callback);
        instance.AddRealTimer(toReturn);
        return toReturn;
    }

    private void AddGameTimer(Timer toAdd)
    {
        for(int i = 0; i < gameTimer.Count; i++)
        {
            if(gameTimer[i].EndTime > toAdd.EndTime)
            {
                Timer tmp = gameTimer[i];
                gameTimer[i] = toAdd;
                toAdd = tmp;
            }
        }

        gameTimer.Add(toAdd);

        if (!enabled)
        {
            enabled = true;
        }
    }

    private void AddRealTimer(Timer toAdd)
    {
        for (int i = 0; i < realTimer.Count; i++)
        {
            if (realTimer[i].EndTime > toAdd.EndTime)
            {
                Timer tmp = realTimer[i];
                realTimer[i] = toAdd;
                toAdd = tmp;
            }
        }

        realTimer.Add(toAdd);

        if (!enabled)
        {
            enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameTimer.Count <= 0 && realTimer.Count <= 0)
        {
            enabled = false;
            return;
        }

        //Game Timer
        if (gameTimer.Count > 0)
        {
            while (gameTimer[0].EndTime <= Time.time)
            {
                gameTimer[0].Execute();

                if (gameTimer.Count <= 0)
                {
                    enabled = false;
                    return;
                }
            }
        }

        //Real Timer
        if(realTimer.Count > 0)
        {
            while (realTimer[0].EndTime <= Time.unscaledTime)
            {
                realTimer[0].Execute();

                if (realTimer.Count <= 0)
                {
                    enabled = false;
                    return;
                }
            }
        }
    }
}
