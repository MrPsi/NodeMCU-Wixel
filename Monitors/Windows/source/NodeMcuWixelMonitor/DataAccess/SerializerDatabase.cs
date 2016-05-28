using System;
using System.IO;

namespace NodeMcuWixelMonitor.DataAccess
{
    public abstract class SerializerDatabase<TEntity> : Database<TEntity> where TEntity : new()
    {
        private readonly string _filename;

        protected SerializerDatabase(string filename)
        {
            _filename = filename;
        }

        public override void Save(TEntity entity)
        {
            var path = GetPath(_filename);
            using (var fileStream = File.Open(path, FileMode.Create))
            {
                Serialize(fileStream, entity);
                fileStream.Close();
            }
        }

        protected abstract void Serialize(FileStream fileStream, TEntity entity);

        public override TEntity Load()
        {
            TEntity entity;
            var path = GetPath(_filename);
            using (var fileStream = File.Open(path, FileMode.OpenOrCreate))
            {
                try
                {
                    entity = Deserialize(fileStream);
                    fileStream.Close();
                }
                catch
                {
                    entity = new TEntity();
                }
            }

            return entity;
        }

        protected abstract TEntity Deserialize(FileStream fileStream);

        protected string GetPath(string filename)
        {
            var applicationData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var path = Path.Combine(applicationData, "NodeMcuWixelMonitor");
            Directory.CreateDirectory(path);
            var pathAndFile = Path.Combine(path, filename);
            return pathAndFile;
        }
    }
}