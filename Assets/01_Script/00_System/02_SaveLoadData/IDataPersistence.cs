public interface IDataPersistence 
{
    public void WriteDataToFileContiner(ref GameDataContiner continer);

    public void ReadDataToFileContiner(GameDataContiner continer);
}
