using UnityEngine;
using System.Collections.Generic;

public class RangeChecker : MonoBehaviour
{
    public List<string> tags;

    private List<GameObject> m_targets = new List<GameObject>();

    void OnTriggerEnter(Collider other)
    {

        bool invalid = true;
        for (int i = 0; i < tags.Count; i++)
        {
            if (other.CompareTag(tags[i]))
            {
                invalid = false;
            }

            if (invalid)
            {
                //Debug.Log("Exiting Invalid");
                return;
            }

            m_targets.Add(other.gameObject);
        }
    }
    
    // Remove target from list so we do not add to calculations
    void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < m_targets.Count; i++)
        {
            if (other.gameObject == m_targets[i])
            {
                m_targets.Remove(other.gameObject);

                return;
            }
        }
    }

    // List of targets acquired
    public List<GameObject> GetValidtargets()
    {
        return m_targets;
    }
    
    // Determine if target is within list of valid targets.
    // current object
    // true is GameObject is in list of valid game objects.
    public bool InRange(GameObject go)
    {
        for (int i = 0; i < m_targets.Count; i++)
        {
            if (go == m_targets[i])
            {
                return true;
            }
        }
        return false;
    }


}
