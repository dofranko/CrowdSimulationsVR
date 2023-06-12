using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Rvo2Helper
{
    public static RVO.Vector2 ToRVOVector(Vector3 param)
    {
        return new RVO.Vector2(param.x, param.z);
    }

    public static Vector3 ToUnityVector(RVO.Vector2 param)
    {
        return new Vector3(param.x(), 0, param.y());
    }
}
