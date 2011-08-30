namespace Glimpse.EF.Extensibility
{
    public interface ICommandParamaterParser
    {
        string Parse(string command, string parameterName, object parameterValue, string parameterType, int parameterSize);
    }
}