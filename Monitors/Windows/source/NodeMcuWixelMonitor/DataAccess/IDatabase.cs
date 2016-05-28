namespace NodeMcuWixelMonitor.DataAccess
{
    public interface IDatabase<TEntity>
    {
        void Save(TEntity entity);
        TEntity Load();
    }
}