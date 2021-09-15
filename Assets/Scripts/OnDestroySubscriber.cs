using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDestroySubscriber : MonoBehaviour
{
    [SerializeField]
    private Companion companion;

    internal void NotifyOnDestroy()
    {
        companion.OnEnemyDestroyed();
    }
}
