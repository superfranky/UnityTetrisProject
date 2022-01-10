using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Channels/IntActionChannelISO")]
public class IntActionChannelISO : ScriptableObject
{
    public Action<int> MyEvent;

    public void Raise(int score)
    {
        MyEvent?.Invoke(score);
    }
}
