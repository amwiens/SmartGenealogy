using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using SmartGenealogy.DesignData;

namespace SmartGenealogy.Tests.Avalonia;

[TestClass]
public class DesignDataTests
{
    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
        DesignData.DesignData.Initialize();
    }

    // Return all properties
    public static IEnumerable<object[]> DesignDataProperties =>
        typeof(DesignData.DesignData).GetProperties()
            .Select(p => new object[] { p });

    [TestMethod]
    [DynamicData(nameof(DesignDataProperties))]
    public void Property_ShouldBeNotNull(PropertyInfo property)
    {
        var value = property.GetValue(null);
        Assert.IsNotNull(value);
    }
}