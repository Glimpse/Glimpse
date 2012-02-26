namespace Glimpse.Test.Core2.TestDoubles
{
    public interface IDummyMixin
    {
        string Name { get; }    
    }

    public class DummyMixin : IDummyMixin
    {
        public string Name { get; set; }
    }
}