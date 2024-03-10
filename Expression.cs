namespace SimpleInterpreter.Types;

public interface IExpression { }

public record BinaryExpression(Token op, IExpression left, IExpression right) : IExpression;
public record UnaryExpression(Token op, IExpression expression) : IExpression;

public record NumberExpression(Token value) : IExpression;