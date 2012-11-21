namespace Glimpse.Test.Core.TestDoubles
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