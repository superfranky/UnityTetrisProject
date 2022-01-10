using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewManager : MonoBehaviour
{
    [SerializeField] private StringActionChannelISO previewBoardChannel;
    [SerializeField] private GameObject[] _previewBlocks;
    // Start is called before the first frame update
    void Start()
    {
        previewBoardChannel.MyEvent += DisplayPreview;
    }
    private void DisplayPreview(string previewBlockName)
    {
        foreach (var block in _previewBlocks)
        {
            block.SetActive(block.name == previewBlockName);
        }
    }

}
