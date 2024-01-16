using System.Collections.Generic;
using UnityEngine;

public class ColliderPlaceholderController : MonoBehaviour
{
    public List<Collider> colliders;


    public void TurnOffColliders()
    {
        for (int i = 0; i < colliders.Count; i++)
        {
            colliders[i].enabled = false;
        }
    }

    public void TurnOnColliders()
    {
        for (int i = 0; i < colliders.Count; i++)
        {
            colliders[i].enabled = true;
        }
    }
}
