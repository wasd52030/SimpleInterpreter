namespace SimpleInterpreter.Types;

public enum TokenType
{
    /// <summary>
    /// 0,1,2,3,4,5,6,7,8,9
    /// </summary>
    NUMBER,
    /// <summary>
    /// +
    /// </summary>
    PLUS,
    /// <summary>
    /// -
    /// </summary>
    MINUS,
    /// <summary>
    /// *
    /// </summary>
    STAR,
    /// <summary>
    /// /
    /// </summary>
    SLASH,
    /// <summary>
    /// (
    /// </summary>
    LEFT_PARENT,
    /// <summary>
    /// )
    /// </summary>
    RIGHT_PARENT,
    /// <summary>
    /// ;
    /// </summary>
    SEMICOLON,
    /// <summary>
    /// End of Line
    /// </summary>
    EOF
}

/// <summary>
/// Token
/// </summary>
/// <param name="type"></param>
/// <param name="lexeme"></param>
/// <param name="literal"></param>
/// <param name="line"></param>
/// <returns></returns>
public record Token(TokenType type, string lexeme, float? literal, int line);