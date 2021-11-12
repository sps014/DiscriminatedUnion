
using DiscriminatedUnion;

namespace SampleApp;

[Union("ExpressionSyntax")]
enum ExpressionSyntaxType
{
    [UnionProperty<SyntaxToken>("Number")]
    NumberExpressionSyntax,

    [UnionProperty<ExpressionSyntax>("Left")]
    [UnionProperty<SyntaxToken>("Operator")]
    [UnionProperty<ExpressionSyntax>("Right")]
    BinaryExpressionSyntax
}
public sealed record SyntaxToken(string Value);