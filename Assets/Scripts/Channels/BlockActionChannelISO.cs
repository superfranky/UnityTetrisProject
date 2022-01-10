using System;
using System.Collections;
using System.Collections.Generic;
using MyClasses;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Channels/BlockActionChannelISO")]
public class BlockActionChannelISO : ScriptableObject
{
    public Action<Block> MyEvent;

    public void Raise(Block block)
    {
        MyEvent?.Invoke(block);
    }
}
