namespace SmartGenealogy.Extensions;

public static class VisualTreeHelperExtensions
{
    public static T? FindVisualChildByPredicate<T>(this VisualElement element, Func<T, bool> predicate) where T : VisualElement
    {
        if (element is T && predicate((T)element))
        {
            return (T)element;
        }

        foreach (var child in element.LogicalChildren)
        {
            var result = (child as VisualElement)?.FindVisualChildByPredicate(predicate);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }
}