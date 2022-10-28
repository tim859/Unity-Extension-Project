using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPath : MonoBehaviour
{
    public Path path;
    public bool closeLoop;
    public bool topDown;

    public Path CreatePath()
    {
        return path = new Path(Vector3.zero);
    }

    private void Reset()
    {
        CreatePath();
    }
}
