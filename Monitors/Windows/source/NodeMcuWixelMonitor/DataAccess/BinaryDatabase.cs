using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NodeMcuWixelMonitor.DataAccess
{
    public class BinaryDatabase<TEntity> : SerializerDatabase<TEntity> where TEntity : new()
    {
        public BinaryDatabase(string filename)
            : base(filename)
        {
        }

        protected override void Serialize(FileStream fileStream, TEntity entity)
        {
            var binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fileStream, entity);
        }

        protected override TEntity Deserialize(FileStream fileStream)
        {
            var binaryFormatter = new BinaryFormatter();
            return (TEntity)binaryFormatter.Deserialize(fileStream);
        }
    }
}