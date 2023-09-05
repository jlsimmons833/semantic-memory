// Copyright (c) Microsoft. All rights reserved.

using Microsoft.Extensions.Logging;
using Microsoft.SemanticMemory.Configuration;

namespace Microsoft.SemanticMemory.Diagnostics;

public static class DefaultLogger
{
    private static ILoggerFactory? s_loggerFactory;

    public static void SetLoggerFactory(ILoggerFactory loggerFactory)
    {
        DefaultLogger.s_loggerFactory = loggerFactory;
    }

    public static ILoggerFactory? LoggerFactory => s_loggerFactory;
}

/// <summary>
/// Create and cache a logger instance using the same
/// configuration sources supported by Semantic Memory config.
/// </summary>
/// <typeparam name="T"></typeparam>
public static class DefaultLogger<T>
{
    public static readonly ILogger<T> Instance = GetLogFactory()
        .CreateLogger<T>();

    private static ILoggerFactory GetLogFactory()
    {
        return DefaultLogger.LoggerFactory ?? throw new ConfigurationException("Unable to provide logger factory");
    }
}
