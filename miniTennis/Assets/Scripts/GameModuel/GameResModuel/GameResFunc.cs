using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameResFunc 
{

    public static string GetResPath(EResourceType resType, string name)
    {
        switch (resType)
        {
                case EResourceType.AnimController:
                    return string.Format("Entity/Anim/{0}", name);
                case EResourceType.Ball:
                    return string.Format("Ball/{0}", name);
                case EResourceType.Ground:
                    return string.Format("Ground/Prefab/{0}", name);
                case EResourceType.Role:
                    return string.Format("Entity/Prefab/{0}", name);
                case EResourceType.UI:
                    return name;
                case EResourceType.BallMechine:
                    return string.Format("BallMechine/Prefab/{0}", name);
        }
        return name;
    }
}
