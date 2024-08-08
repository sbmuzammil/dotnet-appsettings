using DotnetAppSettings.Formatters;

namespace DotnetAppSettings;

internal static class FormatterFactory
{
    internal static IOutputFormatter Create(bool isMap, bool isEnv, bool isJson,bool isJsonObject, bool isText)
    {
        if (isMap)
            return new MapEnvironmentOutputFormatter();

        if (isEnv)
            return new ArrayEnvironmentOutputFormatter();

        if (isJson)
            return new JsonEnvironmentOutputFormatter();

        if (isJsonObject)
          return new JsonObjectEnvironmentOutputFormatter();

        return isText ?
            new TextOutputFormatter() :
            new AppServiceJsonOutputFormatter();
    }
}
