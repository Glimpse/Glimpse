namespace Glimpse.Mvc.Model
{
    public class DefaultValueModel<T>
    {
        public T Value { get; set; }
        
        public T Default { get; set; }

        public bool IsDefault()
        {
            if (Value == null && Default == null)
            {
                return true;
            }

            if (Value == null && Default != null)
            {
                return false;
            }

            return Value.Equals(Default);
        }
    }
}