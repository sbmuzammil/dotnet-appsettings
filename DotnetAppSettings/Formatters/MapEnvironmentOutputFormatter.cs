﻿namespace DotnetAppSettings.Formatters;

internal class MapEnvironmentOutputFormatter : BaseEnvironmentOutputFormatter, IOutputFormatter
{
    public Task WriteAsync(Stream stream, IEnumerable<AzureAppSetting> settings)
    {
        return SerializeAsync(stream, settings.ToDictionary(s => s.Name, s => s.Value));
    }
}
