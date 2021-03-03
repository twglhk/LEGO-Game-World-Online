using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerLog
{
    public static ServerLog _instance { get; } = new ServerLog();

    public void WriteLog(object log)
    {
        Debug.Log($"[Server Log] {log}");
    }
}
