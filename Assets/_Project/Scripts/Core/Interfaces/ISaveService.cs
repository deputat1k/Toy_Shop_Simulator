namespace ToyShop.Core.Interfaces
{
    public interface ISaveService
    {
        bool HasSave { get; }
        void Save();
        void Load();
    }
}