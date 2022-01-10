using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Channels/StringActionChannelISO")]
public class StringActionChannelISO : ScriptableObject
{
    public Action<String> MyEvent;

    public void Raise(string clipName)
    {
        MyEvent?.Invoke(clipName);
    }
}
