namespace SpaceBattle
{
    // абстрактный объект, который может быть чем угодно
    public interface IUObject
    {
        public void SetProperty(string key, object value);
        public object GetProperty(string key);
    }
}
