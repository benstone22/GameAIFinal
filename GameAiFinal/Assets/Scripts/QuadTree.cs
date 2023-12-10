using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Quadtree<T>
{
    private QuadtreeNode<T> root;

    public Quadtree(Rect bounds)
    {
        root = new QuadtreeNode<T>(bounds);
    }

    public void Insert(Vector3 position, T data)
    {
        root.Insert(position, data);
    }

    public List<T> Query(Vector3 position, float radius)
    {
        return root.Query(position, radius);
    }
}

public class QuadtreeNode<T>
{
    private Rect bounds;
    private List<T> elements;
    private QuadtreeNode<T>[] children;

    public QuadtreeNode(Rect bounds)
    {
        this.bounds = bounds;
        elements = new List<T>();
        children = new QuadtreeNode<T>[4];
    }

    public void Insert(Vector3 position, T data)
    {
        if (!bounds.Contains(position))
        {
            return;
        }

        if (elements.Count < 4)
        {
            elements.Add(data);
        }
        else
        {
            if (children[0] == null)
            {
                Split();
            }

            for (int i = 0; i < 4; i++)
            {
                children[i].Insert(position, data);
            }
        }
    }

    private void Split()
    {
        float subWidth = bounds.width / 2f;
        float subHeight = bounds.height / 2f;

        children[0] = new QuadtreeNode<T>(new Rect(bounds.x, bounds.y, subWidth, subHeight));
        children[1] = new QuadtreeNode<T>(new Rect(bounds.x + subWidth, bounds.y, subWidth, subHeight));
        children[2] = new QuadtreeNode<T>(new Rect(bounds.x, bounds.y + subHeight, subWidth, subHeight));
        children[3] = new QuadtreeNode<T>(new Rect(bounds.x + subWidth, bounds.y + subHeight, subWidth, subHeight));
    }

    public List<T> Query(Vector3 position, float radius)
    {
        List<T> result = new List<T>();

        if (!bounds.Overlaps(new Rect(position.x - radius, position.z - radius, 2 * radius, 2 * radius)))
        {
            return result;
        }

        foreach (T element in elements)
        {
            result.Add(element);
        }

        if (children[0] != null)
        {
            for (int i = 0; i < 4; i++)
            {
                result.AddRange(children[i].Query(position, radius));
            }
        }

        return result;
    }
}
