using System.Text;
using Xunit;
using Yoakke.C.Syntax;
using Yoakke.Lexer;
using Yoakke.Parser;

namespace Cesium.Parser.Tests;

public class ParserTests
{
    private static string? GetErrorString<T>(ParseResult<T> result)
    {
        if (!result.IsError) return null;

        var errorMessage = new StringBuilder();
        var err = result.Error;
        foreach (var element in err.Elements.Values)
        {
            errorMessage.AppendLine($"expected {string.Join(" or ", element.Expected)} while parsing {element.Context}");
        }

        var got = err.Got switch
        {
            null => "end of input",
            IToken<CTokenType> t => $"{t.Kind}: {t.Text}",
            var o => o.ToString()
        };
        errorMessage.AppendLine($" but got {got}");

        return errorMessage.ToString();
    }

    [Fact]
    public void SimpleParserTest()
    {
        const string source = "int main() {}";
        var parser = new CParser(new CLexer(source));

        var result = parser.ParseTranslationUnit();
        Assert.True(result.IsOk, GetErrorString(result));
    }
}
