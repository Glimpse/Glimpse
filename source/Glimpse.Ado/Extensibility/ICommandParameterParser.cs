namespace Glimpse.Ado.Extensibility
{
    public interface ICommandParameterParser
    {
        string Parse(string command, string parameterName, object parameterValue, string parameterType, int parameterSize);
    }
}