using UnityEngine;
using System.Collections.Generic;


// Used to inherit data types
public abstract class MessageData { };
public enum MessageType { DAMAGED, HEALTHCHANGED, DIED };

// Delegate function that can send messages out
// Type of message
// GameObject that is sending the message
// Data to be sent
public delegate void MessageDelegate(MessageType messageType, GameObject go, MessageData data);

public class MessageHandler : MonoBehaviour
{
    [Tooltip("Types of messages this GameObject can receive")]
    public List<MessageType> messages;

    private List<MessageDelegate> m_messageDelegates = new List<MessageDelegate>();
    
    // Register a new delegate
    public void RegisterDelegate(MessageDelegate messageDelegate)
    {
        m_messageDelegates.Add(messageDelegate);

    }

    public bool CustomSendMessage(MessageType messageType, GameObject go, MessageData data)
    {
        bool approved = false;

        // Check to see if we have a message that can be sent
        for (int i = 0; i < messages.Count; i++)
        {
            if (messages[i] == messageType)
            {
                approved = true;
                break;
            }

        }

        if (!approved)
            return false;

        for (int i = 0; i < m_messageDelegates.Count; i++)
        {
            m_messageDelegates[i](messageType, go, data);
        }

        return true;

    }
}

public class DamageData : MessageData {
    public int damage;
}


// Data passed on death
public class DeathData : MessageData
{
    public GameObject attacker;
    public GameObject attacked;
}

// Data passed on health change
public class HealthData: MessageData
{
    public int maxHealth;
    public int curHealth;
}