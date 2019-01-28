using System;
using UnityEngine;

public class Logger
{
    public static void Format(string format, params object[] args)
    {
        string methodInfo = "";
        // The stacks doesn't work on WebGL.
        if (Application.isEditor)
        {
            var st = new System.Diagnostics.StackTrace();
            var method = st.GetFrame(1).GetMethod();
            methodInfo = method.ReflectedType.Name + "." + method.Name + "(): ";
        }
        Debug.LogFormat(methodInfo + format, args);
    }
}

