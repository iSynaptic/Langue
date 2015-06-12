using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langue.ProjectAnalyzers.Design
{
    [TestFixture]
    public class PatternSignatureConsistencyAnalyzerTests : DiagnosticAnalyzerVerifier<PatternSignatureConsistencyAnalyzer>
    {
        protected override IEnumerable<Type> GetContextAssembliesFromTypes()
        {
            yield return typeof(Pattern<,>);
            yield return typeof(Text);
            yield return typeof(Json);
            yield return typeof(Xml);
        }

        [Test]
        public void ExtendedObjectParameterNameIncorrect()
        {
            var code = WrapInStaticClass(@"
                public static Pattern<string, TextContext> PassThru(this Pattern<string, TextContext> notSelf)
                {
                    return ctx => notSelf(ctx);
                }");

            var expected = new DiagnosticResult
            {
                Id = Diagnostics.ExtendedObjectParameterNameIncorrect.Id,
                Message = "Parameter name should be 'self'.",
                Severity = Diagnostics.ExtendedObjectParameterNameIncorrect.DefaultSeverity,
                Locations = new[]
                {
                    new DiagnosticResultLocation("Test0.cs", 2, 103)
                }
            };

            VerifyCSharpDiagnostic(code, expected);
        }

        [Test]
        public void PatternContextArgumentNameIncorrect()
        {
            var code = WrapInStaticClass(@"
                public static Langue.Pattern<string, Langue.TextContext> PassThru(this Langue.Pattern<string, Langue.TextContext> self) => notCtx => self(notCtx);");

            var expected = new DiagnosticResult
            {
                Id = Diagnostics.PatternContextArgumentNameIncorrect.Id,
                Message = "Parameter name should be 'ctx'.",
                Severity = Diagnostics.PatternContextArgumentNameIncorrect.DefaultSeverity,
                Locations = new[]
                {
                    new DiagnosticResultLocation("Test0.cs", 2, 140)
                }
            };

            VerifyCSharpDiagnostic(code, expected);
        }
    }
}
