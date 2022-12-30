namespace SpaceBattle
{
    public interface IUObject
    {
        public void SetProperty(string key, object value);
        public object GetProperty(string key);
    }
}
