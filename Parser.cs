namespace SimpleInterpreter.Parser;

using SimpleInterpreter.Types;
using SimpleInterpreter.Lexer;
using System.Linq.Expressions;

class Parser(IEnumerable<Token> tokens)
{
    private readonly IEnumerable<Token> tokens = tokens;
    private int current = 0;

    private bool isAtEnd(int offset = 0) => tokens.ElementAt(current + offset).type == TokenType.EOF;


    private Token peek(int offset = 0)
    {
        if (!isAtEnd(offset))
        {
            return tokens.ElementAt(current + offset);
        }
        return new Token(TokenType.EOF, "\0", null, 0);
    }

    private Token previous() => peek(-1);

    private void advance() => current++;

    private Token take()
    {
        var token = peek();
        advance();
        return token;
    }

    private void consume(TokenType type)
    {
        var token = peek();
        if (token.type == type)
        {
            advance();
            return;
        }

        throw new Exception($"[line: {token.line}] {token.lexeme} should be {type}, instead of {token.type}");
    }

    private bool match(TokenType type)
    {
        if (peek().type == type)
        {
            advance();
            return true;
        }
        return false;
    }

    private IExpression number()
    {
        var token = take();
        if (token.type != TokenType.NUMBER)
        {
            throw new Exception($"{token.lexeme} should be a number, instead of {token.type}");
        }
        return new NumberExpression(token);
    }

    private IExpression primary()
    {
        if (match(TokenType.LEFT_PARENT))
        {
            var expr = term();
            consume(TokenType.RIGHT_PARENT);

            return expr;
        }

        return number();
    }

    private IExpression unary()
    {
        if (match(TokenType.MINUS))
        {
            var op = previous();
            return new SimpleInterpreter.Types.UnaryExpression(op, unary());
        }

        return primary();
    }

    private IExpression factor()
    {
        var expr = unary();
        while (match(TokenType.STAR) || match(TokenType.SLASH))
        {
            var op = previous();
            var right = unary();
            expr = new SimpleInterpreter.Types.BinaryExpression(op, expr, right);
        }

        return expr;
    }

    private IExpression term()
    {
        var expr = factor();
        while (match(TokenType.PLUS) || match(TokenType.MINUS))
        {
            var op = previous();
            var right = factor();
            expr = new SimpleInterpreter.Types.BinaryExpression(op, expr, right);
        }

        return expr;
    }

    private Statment expressionStatment()
    {
        var expression = term();
        consume(TokenType.SEMICOLON);
        return new Statment(expression);
    }

    public IEnumerable<Statment> prase()
    {
        var statments = Enumerable.Empty<Statment>();

        while (!isAtEnd())
        {
            statments = statments.Append(expressionStatment());
        }

        return statments;
    }
}