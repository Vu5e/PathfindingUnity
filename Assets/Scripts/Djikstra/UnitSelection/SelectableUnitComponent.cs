using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


public class SelectableUnitComponent : MonoBehaviour
{
    [SerializeField] private bool selection = false;

    // Set the selection.
    public void setSelection(bool value)
    {
        this.selection = value;
    }

    // Check if it is selected
    public bool isSelected()
    {
        bool result = selection;
        return result;
    }
}