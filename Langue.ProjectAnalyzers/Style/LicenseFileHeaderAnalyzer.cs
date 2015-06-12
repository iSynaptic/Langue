using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.IO;

namespace Langue.ProjectAnalyzers.Style
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class LicenseFileHeaderAnalyzer : DiagnosticAnalyzer
    {
        private static readonly ImmutableArray<string> ExpectedHeader
            = GetExpectedHeader();

        private static ImmutableArray<string> GetExpectedHeader()
        {
            var diagsType = typeof(Diagnostics);

            using (var stream = diagsType.Assembly.GetManifestResourceStream($"{diagsType.FullName}.LICENSE"))
            {
                if (stream == null) return ImmutableArray<string>.Empty;

                using (var reader = new StreamReader(stream))
                {
                    var contents = reader.ReadToEnd()
                        .Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                        .Select(x => $"// {x}");

                    return ImmutableArray.CreateRange(contents);
                }
            }
        }

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics 
            => ImmutableArray.Create(Diagnostics.LicenseFileHeader);

        public override void Initialize(AnalysisContext context)
        {
            if (ExpectedHeader.IsDefaultOrEmpty) return;
        }
    }
}