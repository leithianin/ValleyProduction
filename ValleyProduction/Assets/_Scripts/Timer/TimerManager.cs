using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : VLY_Singleton<TimerManager>
{
    public class Timer
    {
        private float duration;

        private Action callback = null;

        public Coroutine coroutine;

        public Action Callback => callback;

        public float Duration => duration;

        public void SetAsGame(float duration, Action nCallback)
        {
            this.duration = duration;// Time.time + duration;
            callback = nCallback;
        }

        public void SetAsReal(float duration, Action nCallback)
        {
            this.duration = duration;// Time.realtimeSinceStartup + duration;
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
            StopTimerRoutine(coroutine);
        }

        public void Execute()
        {
            Debug.Log(callback);
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
            if(gameTimer[i].Duration > toAdd.Duration)
            {
                Timer tmp = gameTimer[i];
                gameTimer[i] = toAdd;
                toAdd = tmp;
            }
        }

        toAdd.coroutine = StartCoroutine(GameTimerRoutine(toAdd));

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
            if (realTimer[i].Duration > toAdd.Duration)
            {
                Timer tmp = realTimer[i];
                realTimer[i] = toAdd;
                toAdd = tmp;
            }
        }

        toAdd.coroutine = StartCoroutine(RealTimerRoutine(toAdd));

        realTimer.Add(toAdd);

        if (!enabled)
        {
            enabled = true;
        }
    }

    private static void StopTimerRoutine(Coroutine toStop)
    {
        if (toStop != null)
        {
            instance.StopCoroutine(toStop);
        }
    }

    // Update is called once per frame
    /*void Update()
    {
        if (gameTimer.Count <= 0 && realTimer.Count <= 0)
        {
            enabled = false;
            return;
        }

        //Game Timer
        if (gameTimer.Count > 0)
        {
            while (gameTimer[0].Duration <= Time.time)
            {
                gameTimer[0].Execute();
            }
        }

        //Real Timer
        if(realTimer.Count > 0)
        {
            while (realTimer[0].Duration <= Time.unscaledTime)
            {
                realTimer[0].Execute();
            }
        }
    }*/

    IEnumerator GameTimerRoutine(Timer timer)
    {
        yield return new WaitForSeconds(timer.Duration);
        timer.Callback?.Invoke();
        //timer.Execute();
    }

    IEnumerator RealTimerRoutine(Timer timer)
    {
        yield return new WaitForSecondsRealtime(timer.Duration);
        timer.Execute();
    }
}
