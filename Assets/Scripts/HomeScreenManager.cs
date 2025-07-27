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
    private float dmListDiff = 302f;

    private void Update()
    {
        dmList.rectTransform.anchoredPosition = new Vector3(0, dmListMinHeight + (dmListDiff * scrollbar.value), 0);
    }
}
