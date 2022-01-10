using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using MyClasses;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    [SerializeField] private IntActionChannelISO scoreBoardChannel;
    [SerializeField] private IntActionChannelISO lineBoardChannel;
    [SerializeField] private IntActionChannelISO levelBoardChannel;
    [SerializeField] private BlockActionChannelISO statBoardChannel;

    [SerializeField] private TMP_Text _scoreCount;
    [SerializeField] private TMP_Text _lineCount;
    [SerializeField] private TMP_Text _levelCount;
    [SerializeField] private TMP_Text[] _blockCounters;

    void Start()
    {
        scoreBoardChannel.MyEvent += DisplayScore;
        lineBoardChannel.MyEvent += DisplayLines;
        levelBoardChannel.MyEvent += DisplayLevel;
        statBoardChannel.MyEvent += DisplayStats;
    }
    private void DisplayStats(Block block)
    {
        foreach (var counter in _blockCounters)
        {
            if (counter.name == block.GetType().Name)
            {
                var currentNumber = int.Parse(counter.text);
                currentNumber++;
                var convertedNumber = (float)currentNumber;
                convertedNumber /= 1000;
                var newString = convertedNumber.ToString(CultureInfo.CurrentCulture);
                newString = newString.Remove(0, 2);
                counter.text = newString;
            }
        }
    }
    private void DisplayLevel(int level)
    {
        _levelCount.text = level switch
        {
            < 10 => "0" + level,
            < 100 => level.ToString(),
            _ => _levelCount.text
        };
    }
    private void DisplayScore(int score)
    {
        _scoreCount.text = score switch
        {
            < 10 => "00000" + score,
            < 100 => "0000" + score,
            < 1000 => "000" + score,
            < 10000 => "00" + score,
            < 100000 => "0" + score,
            < 1000000 => score.ToString(),
            _ => _scoreCount.text
        };

    }
    private void DisplayLines(int lines)
    {
        _lineCount.text = lines switch
        {
            < 10 => "00" + lines,
            < 100 => "0" + lines,
            < 1000 => lines.ToString(),
            _ => _lineCount.text
        };
    }

}
