namespace Glimpse.Core2.Framework
{
    public class TabResult
    {
        public object Data { get; set; }//TODO: Should this be an ISerializable since it will have to be stored in a DB depending on the implementation of IPersistanceStore?
        public string Name { get; set; }

        public TabResult(string name, object data)
        {
            Data = data;
            Name = name;
        }
    }
}