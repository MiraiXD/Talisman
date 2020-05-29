using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreadSynchronizer : MonoBehaviour
{
    private static object _lock = new object();
    private static System.Action _callbacks;

    public static void SyncTask(System.Action callback)
    {
        lock (_lock)
        {
            _callbacks += callback;
        }
    }

    private void Update()
    {
        //sure, this updates constantly, again, for demo purposes... implement the actual update hook however you please


        //here we pull the delegates out, incase the callback calls 'Invoke'
        //if Invoke is called, the lock would create a deadlock, and freeze the game
        System.Action a = null;
        lock (_lock)
        {
            if (_callbacks != null)
            {
                a = _callbacks;
                _callbacks = null;
            }
        }
        a?.Invoke();
    }
}
