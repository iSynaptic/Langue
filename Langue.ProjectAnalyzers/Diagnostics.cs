using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Langue.ProjectAnalyzers
{
    public static class Diagnostics
    {
        public static Diagnostic With(this DiagnosticDescriptor self, Location location, params object[] messageArgs) 
            => Diagnostic.Create(self, location, messageArgs);

        public static readonly DiagnosticDescriptor ExtendedObjectParameterNameIncorrect = new DiagnosticDescriptor(
            id: "LPA0001",
            title: "Parameter name for extended object should be 'self'.",
            messageFormat: "Parameter name should be 'self'.",
            category: "Design",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor PatternContextArgumentNameIncorrect = new DiagnosticDescriptor(
            id: "LPA0002",
            title: "Parameter name for pattern delegate should be 'ctx'.",
            messageFormat: "Parameter name should be 'ctx'.",
            category: "Design",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);
    }
}
