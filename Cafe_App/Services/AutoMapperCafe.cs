using Cafe_App.Services;

namespace Cafe_App.Services;

[AutoRegisterFlag]
public class AutoMapperCafe
{
    public Out Map<Out, In>(In source)
        where Out : class, new()
        where In : class
    {
        var outType = typeof(Out);
        var inType = typeof(In);
        var constructor = outType.GetConstructors().First(x => x.GetParameters().Length == 0);
        var outObject = constructor.Invoke(null) as Out;

        var outProperties = outType.GetProperties();
        var inProperties = inType.GetProperties(); ;
        foreach (var outProperty in outProperties)
        {
            var commonProperty = inProperties.FirstOrDefault(x => x.Name == outProperty.Name);
            if (commonProperty != null)
            {
                var valueOfSoruceProperty = commonProperty.GetValue(source);
                outProperty.SetValue(outObject, valueOfSoruceProperty);
            }
        }

        return outObject;
    }
}