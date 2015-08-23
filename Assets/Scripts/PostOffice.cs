using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public static class PostOffice
{
    public delegate void DebugMessage(string message);

    public static event DebugMessage debugMessage;

    public static void PostDebugMessage(string message)
    {
        if (debugMessage != null)
        {
            debugMessage(message);
        }
    }
}

