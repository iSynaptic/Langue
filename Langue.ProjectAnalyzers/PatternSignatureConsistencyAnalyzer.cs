using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Langue.ProjectAnalyzers
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

            if (method.ReturnType.OriginalDefinition == patternDelegate)
            {
                var signatureWalker = new MethodSignatureWalker(ctx, patternDelegate);
                signatureWalker.Visit(method.DeclaringSyntaxReferences[0].GetSyntax());
            }
        }

        private class MethodSignatureWalker : CSharpSyntaxWalker
        {
            private readonly SymbolAnalysisContext _context;
            private readonly INamedTypeSymbol _patternDelegateType;

            public MethodSignatureWalker(SymbolAnalysisContext context, INamedTypeSymbol patternDelegateType)
            {
                _context = context;
                _patternDelegateType = patternDelegateType;
            }

            public override void VisitArrowExpressionClause(ArrowExpressionClauseSyntax node)
            {
                if(node.Expression.IsKind(SyntaxKind.SimpleLambdaExpression))
                    base.VisitArrowExpressionClause(node);
            }

            public override void VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node)
            {
                if(node.Parameter.Identifier.Text != "ctx")
                {
                    _context.ReportDiagnostic(PatternContextArgumentNameIncorrect.With(node.Parameter.GetLocation()));
                }
            }
        }
    }
}