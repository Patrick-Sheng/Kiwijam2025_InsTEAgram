using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

public class HomeScreenManager : MonoBehaviour
{
    [SerializeField]
    private Screen screen;

    [SerializeField]
    private Scrollbar scrollbar;

    [SerializeField]
    private Image dmList;

    private float dmListMinHeight = -204f;
    private float dmListMaxHeight = 9.5f;
    private float dmListDiff = 213.5f;

    private void Update()
    {
        Debug.Log(dmListMinHeight + (dmListDiff * scrollbar.value));
        dmList.rectTransform.anchoredPosition = new Vector3(0, dmListMinHeight + (dmListDiff * scrollbar.value), 0);
    }
}
