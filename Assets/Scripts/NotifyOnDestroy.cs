using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotifyOnDestroy : MonoBehaviour
{

    private List<OnDestroySubscriber> subscribers = new List<OnDestroySubscriber>();

    public void Subscribe(OnDestroySubscriber sub)
    {
        subscribers.Add(sub);
    }

    public void Unsubscribe(OnDestroySubscriber sub)
    {
        subscribers.Remove(sub);
    }

    private void OnDestroy()
    {
        foreach (OnDestroySubscriber sub in subscribers)
        {
            sub.NotifyOnDestroy();
        }    
    } 

}
