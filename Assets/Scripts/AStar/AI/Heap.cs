using UnityEngine;
using System.Collections;
using System;

// Generic heap.
// ** Where T implements IHeapItem<t> garauntees that we can sort the item.
public class Heap<T> where T : IHeapItem<T>
{
    /// The heap
    private T[] m_items;
    
    // Number of items in the hea
    private int m_currentItemCount;

    public Heap(int maxHeapSize)
    {
        m_items = new T[maxHeapSize];
    }
    
    // Add item to heap.
    // Item to add to heap
    public void Add(T item)
    {
        item.HeapIndex = m_currentItemCount;
        m_items[m_currentItemCount] = item;
        SortUp(item);
        m_currentItemCount++;
    }

    // Remove the first item of the heap
    // First item of heap
    public T RemoveFirst()
    {
        // Remove first item and reduce heap count.
        T firstItem = m_items[0];
        m_currentItemCount--;

        // Take last item and make it first
        m_items[0] = m_items[m_currentItemCount];
        m_items[0].HeapIndex = 0;

        // Sort item down to maintain order
        SortDown(m_items[0]);

        return firstItem;
    }
    
    // Resort item
    public void UpdateItem(T item)
    {
        SortUp(item);
        // ***May need to also sort down. For pathfinding case not needed.***
    }

    public int Count
    {
        get
        {
            return m_currentItemCount;
        }
    }

    public bool Contains(T item)
    {
        return Equals(m_items[item.HeapIndex], item);
    }

    // Sort item down towards bottom of heap
    void SortDown(T item)
    {
        while (true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;    // 2n + 1
            int childIndexRight = item.HeapIndex * 2 + 2;   // 2n + 2
            int swapIndex = 0;

            // Does this item have a child on the left. Set to left by default.
            if (childIndexLeft < m_currentItemCount)
            {
                swapIndex = childIndexLeft;

                // Does this item have a child on the right?
                if (childIndexRight < m_currentItemCount)
                {

                    // Which child has the highest priority
                    if (m_items[childIndexLeft].CompareTo(m_items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }

                // If the parent is lower priority then swap.
                if (item.CompareTo(m_items[swapIndex]) < 0)
                {
                    Swap(item, m_items[swapIndex]);
                }
                else
                {
                    // Parent is in correct position. Exit loop.
                    return;
                }

            }
            else
            {
                // No children
                return;
            }

        }
    }
    
    // Sort item towards top of hea
    void SortUp(T item)
    {
        // find parent (n-1)/2
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            T parentItem = m_items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }
    
    // Swap itemA with itemB in the heap
    void Swap(T itemA, T itemB)
    {
        m_items[itemA.HeapIndex] = itemB;
        m_items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}
