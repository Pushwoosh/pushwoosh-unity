using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class NotificationSettings
{
    public bool enabled;
#if UNITY_IPHONE
    public bool pushAlert;
    public bool pushBadge;
    public bool pushSound;
#endif
}
