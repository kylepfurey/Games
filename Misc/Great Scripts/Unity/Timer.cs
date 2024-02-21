
// Simple Timer Script
// by Kyle Furey

using UnityEngine;
using UnityEngine.Events;

// Creates a simple timer.
public class Timer : MonoBehaviour
{
    [Header("Creates a simple timer.")]

    [Header("Whether the timer has started:")]
    public bool start = true;

    [Header("The current time:")]
    public float timer = 0;

    [Header("Where to start and end the time:")]
    public float startTime = 0;
    public float endTime = 10;

    [Header("Whether the timer will increment up or down:")]
    public TimerIncrement increment = TimerIncrement.Default;

    [Header("Whether the timer will stop, loop, or continue when it ends:")]
    public TimerLoop loop = TimerLoop.Stop;

    [Header("The scale applied to the timer:")]
    public float timeScale = 1;

    [Header("Timer events:")]
    public UnityEvent startEvents = null;
    public UnityEvent tickEvents = null;
    public UnityEvent stopEvents = null;
    public UnityEvent endEvents = null;
    public bool callEndEventsOnTick = false;
    private bool calledEndEvents = false;

    [Header("Debugging:")]
    public bool debugLogs = true;
    [SerializeField] private bool restartTimer = false;

    // Timer incrementing enum
    public enum TimerIncrement { Default, Up, Down };

    // Timer looping enum
    public enum TimerLoop { Stop, Loop, Continue };

    private void Start()
    {
        if (start)
        {
            StartTimer();
        }
    }

    private void Update()
    {
        Tick();
    }

    private void OnValidate()
    {
        if (restartTimer)
        {
            restartTimer = false;

            StartTimer();
        }
    }

    // Starts the timer
    public void StartTimer()
    {
        if (debugLogs) { print("Timer on object " + gameObject.name + " started!"); }

        start = true;

        timer = startTime;

        calledEndEvents = false;

        startEvents.Invoke();
    }

    // Stops the timer and returns the current time
    public float StopTimer()
    {
        if (debugLogs) { print("Timer on object " + gameObject.name + " stopped!"); }

        start = false;

        stopEvents.Invoke();

        return timer;
    }

    // The timer's tick
    private void Tick()
    {
        if (start)
        {
            if (debugLogs) { print("Timer on object " + gameObject.name + " is now " + ConvertTimerToString(':') + "."); }

            tickEvents.Invoke();

            if (increment == TimerIncrement.Default)
            {
                if (startTime < endTime)
                {
                    timer += Time.deltaTime * timeScale;

                    if (timer >= endTime)
                    {
                        End();
                    }
                }
                else if (startTime > endTime)
                {
                    timer -= Time.deltaTime * timeScale;

                    if (timer <= endTime)
                    {
                        End();
                    }
                }
                else
                {
                    End();
                }
            }
            else if (increment == TimerIncrement.Up)
            {
                timer += Time.deltaTime * timeScale;

                if (timer >= endTime)
                {
                    End();
                }
            }
            else if (increment == TimerIncrement.Down)
            {
                timer -= Time.deltaTime * timeScale;

                if (timer <= endTime)
                {
                    End();
                }
            }
        }
    }

    // The timer when reaching the end
    private void End()
    {
        switch (loop)
        {
            case TimerLoop.Stop:

                if (debugLogs) { print("Timer on object " + gameObject.name + " reached its end!"); }

                endEvents.Invoke();

                StopTimer();

                break;

            case TimerLoop.Loop:

                if (debugLogs) { print("Timer on object " + gameObject.name + " reached its end!"); }

                endEvents.Invoke();

                StartTimer();

                break;

            case TimerLoop.Continue:

                if (!calledEndEvents || callEndEventsOnTick)
                {
                    if (debugLogs) { print("Timer on object " + gameObject.name + " reached its end!"); }

                    calledEndEvents = true;

                    endEvents.Invoke();
                }

                break;
        }
    }

    // Converts a given time into a readable string (like 00.00.00)
    public static string ConvertTimeToString(float time)
    {
        // Change timer float to a readable time value
        float seconds = time;

        string milliseconds = (seconds - ((int)seconds)).ToString();

        if (milliseconds.Length > 2)
        {
            milliseconds = milliseconds.Remove(0, 2);
        }

        while (milliseconds.Length > 2)
        {
            milliseconds = milliseconds.Remove(milliseconds.Length - 1, 1);
        }

        if (milliseconds.Length == 1)
        {
            milliseconds += "0";
        }

        int minutes = 0;

        while (seconds > 60)
        {
            minutes += 1;

            seconds -= 60;
        }

        if (seconds < 10 && minutes < 10)
        {
            return "0" + minutes.ToString() + ".0" + ((int)seconds).ToString() + "." + milliseconds;
        }
        else if (seconds < 10)
        {
            return minutes.ToString() + ".0" + ((int)seconds).ToString() + "." + milliseconds;
        }
        else if (minutes < 10)
        {
            return "0" + minutes.ToString() + "." + ((int)seconds).ToString() + "." + milliseconds;
        }
        else
        {
            return minutes.ToString() + "." + ((int)seconds).ToString() + "." + milliseconds;
        }
    }

    // Converts a given time into a readable string (like 00.00.00)
    public static string ConvertTimeToString(float time, char separator)
    {
        // Change timer float to a readable time value
        float seconds = time;

        string milliseconds = (seconds - ((int)seconds)).ToString();

        if (milliseconds.Length > 2)
        {
            milliseconds = milliseconds.Remove(0, 2);
        }

        while (milliseconds.Length > 2)
        {
            milliseconds = milliseconds.Remove(milliseconds.Length - 1, 1);
        }

        if (milliseconds.Length == 1)
        {
            milliseconds += "0";
        }

        int minutes = 0;

        while (seconds > 60)
        {
            minutes += 1;

            seconds -= 60;
        }

        if (seconds < 10 && minutes < 10)
        {
            return "0" + minutes.ToString() + separator + "0" + ((int)seconds).ToString() + separator + milliseconds;
        }
        else if (seconds < 10)
        {
            return minutes.ToString() + separator + "0" + ((int)seconds).ToString() + separator + milliseconds;
        }
        else if (minutes < 10)
        {
            return "0" + minutes.ToString() + separator + ((int)seconds).ToString() + separator + milliseconds;
        }
        else
        {
            return minutes.ToString() + separator + ((int)seconds).ToString() + separator + milliseconds;
        }
    }

    // Converts this timer's value into a readable string (like 00.00.00)
    public string ConvertTimerToString()
    {
        return ConvertTimeToString(timer);
    }

    // Converts this timer's value into a readable string (like 00.00.00)
    public string ConvertTimerToString(char separator)
    {
        return ConvertTimeToString(timer, separator);
    }
}
