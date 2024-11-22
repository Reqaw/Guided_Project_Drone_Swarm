using System.Collections.Generic;

public class PriorityQueue<T>
{
    private List<(T item, float priority)> elements = new List<(T item, float priority)>();

    public int Count => elements.Count;

    public void Enqueue(T item, float priority)
    {
        elements.Add((item, priority));
        elements.Sort((a, b) => a.priority.CompareTo(b.priority));
    }

    public T Dequeue()
    {
        var item = elements[0];
        elements.RemoveAt(0);
        return item.item;
    }
}