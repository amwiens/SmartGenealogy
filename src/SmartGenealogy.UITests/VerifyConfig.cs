﻿using AsyncAwaitBestPractices;

namespace SmartGenealogy.UITests;

internal static class VerifyConfig
{
    public static VerifySettings Default { get; }

    static VerifyConfig()
    {
        Default = new VerifySettings();
        Default.IgnoreMembersWithType<WeakEventManager>();
        Default.DisableDiff();
    }
}