namespace Glimpse.Ado.Extensibility
{
    public interface IGlimpseCommandParameterParser
    {
        string Parse(string command, string parameterName, object parameterValue, string parameterType, int parameterSize);
    }
}