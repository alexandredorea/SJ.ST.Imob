namespace SJ.ST.Imob.Core
{
    public interface IRedisDataAgent
    {
        void DeleteStringValue(string key);
        string GetStringValue(string key);
        void SetStringValue(string key, string value);
    }
}