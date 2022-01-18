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

        public void Add(float duration, Action nCallback)
        {
            endTime = Time.time + duration;
            callback = nCallback;

            instance.AddTimer(this);
        }

        public void Stop()
        {
            gameTimer.Remove(this);
        }

        public void Execute()
        {
            callback?.Invoke();
            Stop();
        }
    }

    private static List<Timer> gameTimer = new List<Timer>();

    public static Timer CreateTimer(float time, Action callback)
    {
        Timer toReturn = new Timer();
        toReturn.Add(time, callback);
        return toReturn;
    }

    private void AddTimer(Timer toAdd)
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

    // Update is called once per frame
    void Update()
    {
        while(gameTimer[0].EndTime <= Time.time)
        {
            gameTimer[0].Execute();
            
            if(gameTimer.Count <= 0)
            {
                enabled = false;
                return;
            }
        }
    }
}
