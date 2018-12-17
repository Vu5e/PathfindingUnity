using UnityEngine;
using System.Collections.Generic;
using System.Text;

public class UnitSelectionComponent : MonoBehaviour
{
    private bool isSelecting = false;
    private Vector3 mousePosition1;
    private List<Transform> selectedObjects = new List<Transform>();

    // Selection system.
    private void unitSelectionSystem()
    {
        // Press Left Mouse Button to remember the selection we have
        if (Input.GetMouseButtonDown(0))
        {
            isSelecting = true;
            mousePosition1 = Input.mousePosition;

            foreach (var selectableObject in FindObjectsOfType<SelectableUnitComponent>())
            {
                // Disable and remove selection.
                if (selectableObject.isSelected())
                {
                    Renderer rend = selectableObject.GetComponent<Renderer>();
                    rend.material.color = Color.yellow;
                    selectableObject.setSelection(false);
                }
            }
        }
        // End selection on let go
        if (Input.GetMouseButtonUp(0))
        { 
            foreach (var selectableObject in FindObjectsOfType<SelectableUnitComponent>())
            {
                if (this.isInBound(selectableObject.gameObject))
                {
                    selectedObjects.Add(selectableObject.transform);
                }
            }

            isSelecting = false;
        }

        // All objects inside is highlighted
        if (isSelecting)
        {
            foreach (var selectableObject in FindObjectsOfType<SelectableUnitComponent>())
            {
                if (this.isInBound(selectableObject.gameObject))
                {

                    // Change the color of the selected object.
                    if (!selectableObject.isSelected())
                    {
                        selectableObject.setSelection(true);
                        Renderer rend = selectableObject.GetComponent<Renderer>();
                        rend.material.color = Color.green;
                    }
                }
                else
                {
                    // Destroy all the selected object.
                    if (selectableObject.isSelected())
                    {
                        Renderer rend = selectableObject.GetComponent<Renderer>();
                        rend.material.color = Color.yellow;
                        selectableObject.setSelection(false);
                    }
                }
            }
        }
    }

    // Check if gameobject is within selection bound
    private bool isInBound(GameObject gameObject)
    {
        if( !isSelecting )
		{
            return false;
		}

        var camera = Camera.main;
        var viewportBounds = Utils.GetViewportBounds( camera, mousePosition1, Input.mousePosition );
        return viewportBounds.Contains( camera.WorldToViewportPoint( gameObject.transform.position ) );
    }

    // Get the list of selected objects
    public List<Transform> getSelectedObjects()
    {
        List<Transform> result = selectedObjects;
        return result;
    }

    // Clear all selected objects.
    public void clearSelections()
    {
        this.selectedObjects.Clear();
    }

    // Draw box.
    void OnGUI()
    {
        if( isSelecting )
        {
            // Create a rect from both mouse positions
            var rect = Utils.GetScreenRect( mousePosition1, Input.mousePosition );
            Utils.DrawScreenRect( rect, new Color( 0.8f, 0.8f, 0.95f, 0.25f ) );
            Utils.DrawScreenRectBorder( rect, 2, new Color( 0.8f, 0.8f, 0.95f ) );
        }
    }

    void Update()
    {
        this.unitSelectionSystem();
    }
}