/*
    MIT License

    Copyright (c) 2016 Nils Kübler

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    /// <summary>
    /// Timers act like coroutines. The different is that timers do not require a MonoBehavior.
    /// </summary>
    public class Timer
    {
        /// <summary>
        /// The function that needs to be executed.
        /// </summary>
        public System.Action function;

        /// <summary>
        /// The amount of time in seconds before the function is called again.
        /// </summary>
        public float interval;

        /// <summary>
        /// How many times the function is called.
        /// </summary>
        public int repeatedTimes = 1;

        /// <summary>
        /// When the function will be executed.
        /// </summary>
        public double nextRunTime;

        public Timer(System.Action function, float interval, int repeatedTimes = 1)
        {
            this.function = function;
            this.interval = interval;
            this.repeatedTimes = repeatedTimes;
        }

        public void CalculateNextRunTime(double elapsedTime)
        {
            nextRunTime = elapsedTime + interval;
        }
    }

    /// <summary>
    /// Used by behavior trees to execute nodes that have to be executed on multiple frames.
    /// </summary>
    public class TimerManager : SingletonComponent<TimerManager>
    {
        double elapsedTime = 0.0;
        bool isInUpdate = false;

        private Dictionary<System.Action, Timer> timers;
        private Dictionary<System.Action, Timer> addedTimers;
        private Dictionary<System.Action, Timer> removedTimers;

        protected override void OnSingletonAwake()
        {
            timers = new Dictionary<System.Action, Timer>();
            addedTimers = new Dictionary<System.Action, Timer>();
            removedTimers = new Dictionary<System.Action, Timer>();
        }

        private void Update()
        {
            elapsedTime += Time.deltaTime;

            isInUpdate = true;

            // Execute timers.
            foreach (var timer in timers)
            {
                if (removedTimers.ContainsKey(timer.Key))
                    continue;

                if (timer.Value.nextRunTime <= elapsedTime)
                {
                    if (timer.Value.repeatedTimes == 0)
                    {
                        RemoveTimer(timer.Key);
                        continue;
                    }
                    else if (timer.Value.repeatedTimes > 0)
                    {
                        --timer.Value.repeatedTimes;
                    }

                    timer.Key.Invoke();
                    timer.Value.CalculateNextRunTime(elapsedTime);
                }
            }

            // Add timers.
            foreach (var timer in addedTimers)
            {
                timers[timer.Key] = addedTimers[timer.Key];
            }

            // Remove timers.
            foreach (var timer in removedTimers)
            {
                timers.Remove(timer.Key);
            }

            // Clear addedTimers and removedTimers.
            addedTimers.Clear();
            removedTimers.Clear();

            //Debug.Log($"{name}: Timer(s) count: {timers.Count}");

            isInUpdate = false;
        }

        private void OnDestroy()
        {
            timers?.Clear();
            addedTimers?.Clear();
            removedTimers?.Clear();
        }

        /// <summary>
        /// Adds a timer to the timer manager.
        /// </summary>
        /// <param name="function">The function from which the manager creates a timer.</param>
        /// <param name="interval">How long in seconds before the function is called again.</param>
        /// <param name="repeatedTimes">How many times the function is called.</param>
        public void AddTimer(System.Action function, float interval, int repeatedTimes = 1)
        {
            Timer timer = null;

            // If the timer manager is not running Update(), add the timer directly to timers.
            if (!isInUpdate)
            {
                if (!timers.ContainsKey(function))
                {
                    timers.Add(function, new Timer(function, interval, repeatedTimes));
                }

                timer = timers[function];
            }

            // If the timer manager is running Update(), add the timer to addedTimers so that it will be added after the manager executes other timers.
            else
            {
                if (!addedTimers.ContainsKey(function))
                {
                    addedTimers.Add(function, new Timer(function, interval, repeatedTimes));
                }

                timer = addedTimers[function];

                if (removedTimers.ContainsKey(function))
                {
                    removedTimers.Remove(function);
                }
            }

            timer.CalculateNextRunTime(elapsedTime);
        }

        /// <summary>
        /// Removes a timer from the manager.
        /// </summary>
        /// <param name="function">The function whose timer needs to be removed from the manager.</param>
        public void RemoveTimer(System.Action function)
        {
            // If the timer manager is not running Update(), remove the timer directly from timers.
            if (!isInUpdate)
            {
                if (timers.ContainsKey(function))
                {
                    timers.Remove(function);
                }
            }

            // If the timer manager is running Update(), add the timer to removedTimers so that it will be removed after the manager executes other timers.
            else
            {
                if (timers.ContainsKey(function))
                {
                    removedTimers.Add(function, timers[function]);
                }

                if (addedTimers.ContainsKey(function))
                {
                    addedTimers.Remove(function);
                }
            }
        }

        /// <summary>
        /// Is the manager currently containing a timer?
        /// </summary>
        /// <param name="function">The function associated with the timer.</param>
        /// <returns></returns>
        public bool HasTimer(System.Action function)
        {
            if (!isInUpdate)
            {
                return timers.ContainsKey(function);
            }
            else
            {
                if (removedTimers.ContainsKey(function))
                    return false;
                else if (addedTimers.ContainsKey(function))
                    return true;
                else
                    return timers.ContainsKey(function);
            }
        }
    }
}
