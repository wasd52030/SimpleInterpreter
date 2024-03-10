using SimpleInterpreter.Types;

namespace SimpleInterpreter.Lexer;

public class Lexer(string source)
{
    private readonly string source = source;
    private int start = 0, end = 0, line = 1;

    private readonly Dictionary<string, TokenType> opsType = new() {
        { "+", TokenType.PLUS },
        { "-", TokenType.MINUS },
        { "*", TokenType.STAR },
        { "/", TokenType.SLASH },
        { "(", TokenType.LEFT_PARENT },
        { ")", TokenType.RIGHT_PARENT },
        { ";", TokenType.SEMICOLON }
    };

    private bool isAtEnd() => end >= source.Length;

    private char peek()
    {
        if (!isAtEnd())
        {
            return source[end];
        }

        return '\0';
    }

    private void advance()
    {
        if (peek() == '\n')
        {
            line++;
        }
        end++;
    }

    private char take()
    {
        var c = peek();
        advance();
        return c;
    }

    private Token? prase()
    {
        var c = take();

        switch (c)
        {
            case char space when char.IsWhiteSpace(space):
                return null;
            case char op when op == '+' || op == '-' || op == '*' || op == '/' || op == '(' || op == ')' || op == ';':
                return new Token(opsType[op.ToString()], op.ToString(), null, line);
            case char number when char.IsDigit(number):
                while (!isAtEnd() && char.IsDigit(peek()))
                {
                    advance();
                }

                if (!isAtEnd() && peek() == '.')
                {
                    advance();
                    while (!isAtEnd() && char.IsDigit(peek()))
                    {
                        advance();
                    }
                }

                string lexeme = source[start..end];
                return new Token(TokenType.NUMBER, lexeme, float.Parse(lexeme), line);
            default:
                throw new ArgumentException($"{line}:{c} is an illegal symbol");
        }
    }

    public IEnumerable<Token> scan()
    {
        var tokens = Enumerable.Empty<Token>();
        while (!isAtEnd())
        {
            var token = prase();
            if (token is not null)
            {
                tokens = tokens.Append(token);
            }
            start = end;
        }
        tokens = tokens.Append(new Token(TokenType.EOF, "", null, line));

        return tokens;
    }
}