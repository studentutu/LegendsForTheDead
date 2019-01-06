namespace Game
{
    public interface IDataSerializer<T>
    {
        string extension { get; }
        bool isSaveExist(string name);

        void Save(T data, string name);
        T Load(string name);
        void Delete(string name);
    }
}
