namespace Magic.Core.Repository
{
    public interface IKittenGenesisService
    {
        Kitten CreateNewKitten(string extra = "");
    }
}