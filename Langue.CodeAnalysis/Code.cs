using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Langue
{
    public static class Code
    {
        public static Pattern<CompilationUnitSyntax, SyntaxNode> CompilationUnit => ctx =>
        {
            if(ctx.IsKind(SyntaxKind.CompilationUnit))
            {
                return Match.Success((CompilationUnitSyntax)ctx, ctx);
            }

            return Match<CompilationUnitSyntax>.Failure(ctx);
        };
    }
}
