using System.IO;

namespace NodeMcuWixelMonitor.DataAccess
{
    public class XmlDatabase<TEntity> : SerializerDatabase<TEntity> where TEntity : new()
    {
        public XmlDatabase(string filename)
            : base(filename)
        {
        }

        protected override void Serialize(FileStream fileStream, TEntity entity)
        {
            var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(TEntity));
            xmlSerializer.Serialize(fileStream, entity);
        }

        protected override TEntity Deserialize(FileStream fileStream)
        {
            var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(TEntity));
            return (TEntity)xmlSerializer.Deserialize(fileStream);
        }
    }
}