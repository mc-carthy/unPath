﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T> where T : IComparable<T>
{

    List<T> data;
    public int Count { get { return data.Count; }}

    public PriorityQueue()
    {
        this.data = new List<T>();
    }

    public void Enqueue(T item)
    {
        data.Add(item);

        int childIndex = data.Count - 1;

        while (childIndex > 0)
        {
            // As we're using integers, the decimal gets dropped,
            // so no need for Mathf.floor
            int parentIndex = (childIndex - 1) / 2;

            if (data[childIndex].CompareTo(data[parentIndex]) >= 0)
            {
                break;
            }

            T tmp = data[childIndex];
            data[childIndex] = data[parentIndex];
            data[parentIndex] = tmp;

            // This allows us to traverse one step up the binary heap
            childIndex = parentIndex;
        }
    }

    public T Dequeue()
    {
        int lastIndex = data.Count - 1;

        T topItem = data[0];

        data[0] = data[lastIndex];
        data.RemoveAt(lastIndex);
        lastIndex--;

        int parentIndex = 0;

        while (true)
        {
            int childIndex = (parentIndex * 2) + 1;

            if (childIndex > lastIndex)
            {
                break;
            }

            int rightChild = childIndex + 1;

            if (
                rightChild <= lastIndex && 
                data[rightChild].CompareTo(data[childIndex]) < 0
            )
            {
                childIndex = rightChild;
            }

            if (data[parentIndex].CompareTo(data[childIndex]) <= 0)
            {
                break;
            }

            T tmp = data[parentIndex];
            data[parentIndex] = data[childIndex];
            data[childIndex] = tmp;

            parentIndex = childIndex;
        }

        return topItem;
    }

    public T Peek()
    {
        return data[0];
    }

    public bool Contains(T item)
    {
        return data.Contains(item);
    }

    public List<T> ToList()
    {
        return data;
    }

}