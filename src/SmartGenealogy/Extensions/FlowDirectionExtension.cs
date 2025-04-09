namespace SmartGenealogy.Extensions;

public class FlowDirectionExtension : IMarkupExtension
{
    public object ProvideValue(IServiceProvider serviceProvider)
    {
        var binding = new Binding
        {
            Path = "[KeyName]",
            Source = FlowDirectionManager.Instance
        };
        return binding;
    }
}