﻿using System.Diagnostics;

namespace SmartGenealogy.Helpers;

public class RestartApplication
{
    public static void Restart()
    {
        // Get the path to the current executable
        var exePath = Environment.ProcessPath;

        // Start a new instance of the application
        Process.Start(exePath);

        // Terminate the current instance
        Environment.Exit(0);
    }
}