﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using RoslynPad.NuGet;
using RoslynPad.Runtime;

namespace RoslynPad.Build
{
    internal interface IExecutionHost
    {
        ExecutionPlatform Platform { get; set; }
        PlatformVersion? PlatformVersion { get; set; }
        string Name { get; set; }
        ImmutableArray<MetadataReference> MetadataReferences { get; }

        event Action<IList<CompilationErrorResultObject>>? CompilationErrors;
        event Action<string>? Disassembled;
        event Action<ResultObject>? Dumped;
        event Action<ExceptionResultObject>? Error;
        event Action? ReadInput;
        event Action? RestoreStarted;
        event Action<RestoreResult>? RestoreCompleted;

        void UpdateLibraries(IList<LibraryRef> libraries);

        Task SendInputAsync(string input);
        Task ExecuteAsync(string code, bool disassemble, OptimizationLevel? optimizationLevel);
        Task TerminateAsync();
    }

    internal class RestoreResult
    {
        public static RestoreResult SuccessResult { get; } = new RestoreResult(success: true, error: null);

        public static RestoreResult FromError(string error) => new RestoreResult(success: false, error: error);

        private RestoreResult(bool success, string? error)
        {
            Success = success;
            Error = error;
        }

        public bool Success { get; }
        public string? Error { get; }
    }
}