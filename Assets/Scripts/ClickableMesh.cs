using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableMesh : MonoBehaviour
{
    public bool Clicked { get; private set; }
	
    void OnMouseDown()
    {
        Clicked = true;
    }

    void LateUpdate()
    {
        Clicked = false;
    }
}
