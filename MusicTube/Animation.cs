using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace MusicTube
{
    public class Animation : Storyboard
    {
        public Animation(double from, double to, TimeSpan timeSpan, int repeat, IEasingFunction easing, PropertyPath path, FrameworkElement element)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = from;
            animation.To = to;
            animation.Duration = new Duration(timeSpan);
            animation.RepeatBehavior = new RepeatBehavior(repeat);
            animation.EasingFunction = easing;

            this.Children.Add(animation);
            Storyboard.SetTargetName(animation, element.Name);
            Storyboard.SetTargetProperty(animation, path);
            this.Begin(element);
        }
        public Animation(double from, double to, TimeSpan timeSpan, int repeat, IEasingFunction easing, PropertyPath path, FrameworkElement element, bool hasCompleted)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = from;
            animation.To = to;
            animation.Duration = new Duration(timeSpan);
            animation.RepeatBehavior = new RepeatBehavior(repeat);
            animation.EasingFunction = easing;

            this.Children.Add(animation);
            Storyboard.SetTargetName(animation, element.Name);
            Storyboard.SetTargetProperty(animation, path);
        }

    }
}
