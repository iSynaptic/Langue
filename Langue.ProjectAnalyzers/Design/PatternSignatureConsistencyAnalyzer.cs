using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Langue.ProjectAnalyzers.Design
{
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    using static Diagnostics;

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class PatternSignatureConsistencyAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics 
            => ImmutableArray.Create(ExtendedObjectParameterNameIncorrect, PatternContextArgumentNameIncorrect);

        public override void Initialize(AnalysisContext ctx)
        {
            ctx.RegisterCompilationStartAction(c =>
            {
                var patternDelegate = c.Compilation.GetTypeByMetadataName("Langue.Pattern`2");
                ctx.RegisterSymbolAction(sac => AnalyzeMethod(sac, patternDelegate), SymbolKind.Method);
            });
        }

        private void AnalyzeMethod(SymbolAnalysisContext ctx, INamedTypeSymbol patternDelegate)
        {
            var method = (IMethodSymbol)ctx.Symbol;

            if(method.IsExtensionMethod)
            {
                var parameter = method.Parameters[0];
                if (parameter.Name != "self")
                    ctx.ReportDiagnostic(ExtendedObjectParameterNameIncorrect.With(parameter.Locations[0]));
            }

            if (patternDelegate != null && method.ReturnType.OriginalDefinition == patternDelegate)
            {
                var methodSyntax = (MethodDeclarationSyntax)method.DeclaringSyntaxReferences[0].GetSyntax();
                if(methodSyntax.ExpressionBody != null && methodSyntax.ExpressionBody.Expression.IsKind(SyntaxKind.SimpleLambdaExpression))
                {
                    CheckLambdaExpression((SimpleLambdaExpressionSyntax)methodSyntax.ExpressionBody.Expression, ctx);
                }
            }
        }

        private void CheckLambdaExpression(SimpleLambdaExpressionSyntax node, SymbolAnalysisContext context)
        {
            if (node.Parameter.Identifier.Text != "ctx")
            {
                context.ReportDiagnostic(PatternContextArgumentNameIncorrect.With(node.Parameter.GetLocation()));
            }
        }
    }
}