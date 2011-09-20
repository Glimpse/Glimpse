namespace Glimpse.Core2.Extensibility
{
    public interface IGlimpseRuntime
    {
        void BeginRequest();//Init Glimpse Context
        void EndRequest();//This might just be a high order method that contains others
        void FilterRequest();//Test to see if Glimpse should be involved with this request or not
        void GatherData();//Run the plugins
        void Persist();//Save to DB, also serialize? Let DB provider serialize
        void ModifyResponseBody();//Add the Glimpse script tags before the </body> tag
        void ModifyResponseHeaders();//Add the Glimpse Request ID to the Http Header
        void Initialize();//Run all the setup currently in the constructor - rerun when config changes?
    }
}
