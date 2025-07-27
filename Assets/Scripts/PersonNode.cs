using UnityEngine;
using Radishmouse;
using System.Collections.Generic;
using System;

public class PersonNode : MonoBehaviour, IHitReceiver
{
    [field: SerializeField] public int id { get; private set; }
    [SerializeField] private string personName;

    [SerializeField] private LineRenderer aimLine;
    [field: SerializeField] public Transform aimLineOrigin { get; private set; }
    private bool isAiming;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DeactivateAimLine();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAiming)
        {
            aimLine.SetPosition(1, (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    public void DeactivateAimLine()
    {
        isAiming = false;
        aimLine.enabled = false;
        aimLine.SetPosition(0, aimLineOrigin.position);
        aimLine.SetPosition(1, aimLineOrigin.position);
    }

    public void ActivateAimLine()
    {
        isAiming = true;
        aimLine.enabled = true;
        aimLine.SetPosition(0, aimLineOrigin.position);
    }

    public void SetLineColor(Color color)
    {
        aimLine.startColor = color;
        aimLine.endColor = color;
    }

    public void OnRaycastHit()
    {
        ActivateAimLine();
    }
}
