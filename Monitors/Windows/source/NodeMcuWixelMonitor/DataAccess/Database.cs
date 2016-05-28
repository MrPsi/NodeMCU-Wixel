namespace NodeMcuWixelMonitor.DataAccess
{
    public abstract class Database<TEntity> : IDatabase<TEntity>
    {
        public abstract void Save(TEntity entity);
        public abstract TEntity Load();
    }
}