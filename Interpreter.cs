using SimpleInterpreter.Types;

namespace SimpleInterpreter.Interpreter;

class Interpreter(IEnumerable<Statment> statments)
{
    private readonly IEnumerable<Statment> statments = statments;

    private float eval(IExpression expression)
    {
        switch (expression)
        {
            case NumberExpression expr:
                if (expr.value.literal == null)
                {
                    throw new Exception($"{expr.value} is None");
                }
                return (float)expr.value.literal;
            case UnaryExpression expr:
                if (expr.op.type != TokenType.MINUS)
                {
                    throw new Exception($"{expr.op} is not correct unary operator");
                }

                var result = eval(expr);
                return -result;
            case BinaryExpression expr:
                var left = eval(expr.left);
                var right = eval(expr.right);
                
                return expr.op.type switch
                {
                    TokenType.PLUS => left + right,
                    TokenType.MINUS => left - right,
                    TokenType.STAR => left * right,
                    TokenType.SLASH => left / right,
                    _ => throw new Exception("unexpected binary operator"),
                };
            default:
                throw new Exception($"{expression} is not implemented.");
        }
    }

    public IEnumerable<float> cal()
    {
        var result = Enumerable.Empty<float>();

        foreach (var statment in statments)
        {
            result = result.Append(eval(statment.expression));
        }

        return result;
    }
}