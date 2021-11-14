
using DiscriminatedUnion;
using Namespacer;

namespace TestName;

[Union("ExpressionSyntax")]
public enum ExpressionSyntaxType
{
    [UnionProperty<SyntaxToken>("Number")]
    NumberExpressionSyntax,

    [UnionProperty<ExpressionSyntax>("Left")]
    [UnionProperty<SyntaxToken>("Operator")]
    [UnionProperty<ExpressionSyntax>("Right")]
    BinaryExpressionSyntax
}
