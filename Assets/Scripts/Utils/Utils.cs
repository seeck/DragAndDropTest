using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Utils {
    public static void SafeInvoke<T1,T2>(this Action<T1,T2> action, T1 arg1, T2 arg2)
    {
        if(action!=null)
        {
            action(arg1,arg2);
        }
    }
}
