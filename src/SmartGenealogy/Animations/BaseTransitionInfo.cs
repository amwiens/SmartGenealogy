using System;

using FluentAvalonia.UI.Media.Animation;

namespace SmartGenealogy.Animations;

public abstract class BaseTransitionInfo : NavigationTransitionInfo
{
    /// <summary>
    /// The duration of the animation at 1c animation scale
    /// </summary>
    public abstract TimeSpan Duration { get; set; }
}