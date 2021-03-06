﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using TestDataSeeding.Model;
using TestDataSeeding.Logic;
using TestDataSeeding.Client;
using YAXLib;
using TestDataSeeding.Common;

namespace TestDataSeeding.SerializedStorage
{
    /// <summary>
    /// An XML based storage implementation of the ISerializedStorageClient.
    /// </summary>
    internal class XmlStorageClient : ISerializedStorageClient, ISerializedStorageStructureManager
    {
        /// <summary>
        /// The dictionary with the entities to save.
        /// </summary>
        private Dictionary<Entity, string> entitiesToSave = new Dictionary<Entity, string>();

        public void BeginTransaction()
        {
            entitiesToSave.Clear();
        }

        public void ExecuteTransaction()
        {
            List<string> savedEntities = new List<string>();

            foreach (KeyValuePair<Entity, string> entry in entitiesToSave)
            {
                try
                {
                    InnerSaveEntity(entry.Key, entry.Value);
                    savedEntities.Add(entry.Value);
                }
                catch (Exception e)
                {
                    DeleteFromDisk(savedEntities);
                    throw e;
                }
            }

            entitiesToSave.Clear();
        }

        /// <summary>
        /// Deletes the given saved entities.
        /// </summary>
        /// <param name="savedEntities">The saved entities.</param>
        private static void DeleteFromDisk(List<string> savedEntities)
        {
            foreach (var entityName in savedEntities)
            {
                File.Delete(entityName);
            }
        }

        public void SaveEntity(Entity entity, EntityStructure entityStructure, string path)
        {
            if (!Directory.Exists(path + "\\Entities"))
            {
                Directory.CreateDirectory(path + "\\Entities");
            }

            entitiesToSave.Add(entity, BuildFileName(entity, entityStructure, path));
        }

        /// <summary>
        /// Saves a given entity to the disk.
        /// </summary>
        /// <param name="entity">The given entity.</param>
        /// <param name="xmlFileName">The filename of the entity.</param>
        private void InnerSaveEntity(Entity entity, string xmlFileName)
        {
            try
            {
                Serialize<Entity>(entity, xmlFileName);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public Entity GetEntity(EntityStructure entityStructure, List<string> primaryKeyValues, string path)
        {
            string xmlFileName = BuildFileName(entityStructure, primaryKeyValues, path);
            Entity deserializedObject = null;

            try
            {
                deserializedObject = Deserialize<Entity>(xmlFileName);
            }
            catch (Exception exception)
            {
                throw exception;
            }

            return deserializedObject;
        }

        public Entity GetEntity(string filename)
        {
            Entity deserializedObject = null;

            try
            {
                deserializedObject = Deserialize<Entity>(filename);
            }
            catch (Exception exception)
            {
                throw exception;
            }

            return deserializedObject;
        }

        public bool IsSaved(string entityName, List<string> primaryKeyValues, string path)
        {
            var xmlFileName = BuildFileName(entityName, primaryKeyValues, path);

            return File.Exists(xmlFileName);
        }

        public string GetEntityFileName(string entityName, List<string> primaryKeyValues, string path)
        {
            return BuildFileName(entityName, primaryKeyValues, path);
        }

        public EntityStructures GetEntityStructures(string pathToFile)
        {
            EntityStructures entityStructures = null;

            try
            {
                entityStructures = Deserialize<EntityStructures>(pathToFile);
            }
            catch (Exception exception)
            {
                throw exception;
            }

            return entityStructures;
        }

        public void SaveEntityStructures(EntityStructures entityStructures, string path, bool overwrite)
        {
            if (!overwrite && File.Exists(path + "\\Structures.xml"))
            {
                throw new EntityStructureAlreadyExistsException();
            }
            try
            {
                Serialize<EntityStructures>(entityStructures, path + "\\Structures.xml");
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public EntityCatalog GetCatalog(string storageFolder)
        {
            EntityCatalog catalog = null;

            try
            {
                catalog = Deserialize<EntityCatalog>(Path.Combine(storageFolder, Consts.Structure.STRUCTURE_FILE_DEFAULT_NAME));
            }
            catch (FileNotFoundException)
            {
                catalog = new EntityCatalog();
            }
            catch (Exception exception)
            {
                throw exception;
            }

            return catalog;
        }

        public void SaveCatalog(EntityCatalog catalog, string path)
        {
            try
            {
                Serialize<EntityCatalog>(catalog, path + "\\Catalog.xml");
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Builds an XML file name for an entity with <paramref name="path"/> and based on the name and key values of the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="entityStructure">The entity structure.</param>
        /// <param name="path">The path where the file is stored.</param>
        /// <returns>The XML file name with path.</returns>
        private string BuildFileName(Entity entity, EntityStructure entityStructure, string path)
        {
            StringBuilder builder = new StringBuilder(entity.Name);

            foreach (var keyName in entityStructure.PrimaryKeys)
            {
                builder.Append("_" + entity.AttributeValues[keyName]);
            }

            builder.Append(".xml");

            string filename = builder.ToString();
            filename = filename.Replace("\\", "_").Replace("/", "_")
                               .Replace(":", "_").Replace("*", "_").Replace("?", "_")
                               .Replace("<", "_").Replace(">", "_").Replace("|", "_");
            string filepath = Path.Combine(path, "Entities", filename);
            return filepath;
        }

        /// <summary>
        /// Builds an XML file name for an entity with <paramref name="path"/> and based on the name and key values of the entity.
        /// </summary>
        /// <param name="entityStructure">The given EntityStructure.</param>
        /// <param name="primaryKeyValues">The given primary key values.</param>
        /// <param name="path">The path where the file is stored.</param>
        /// <returns>The XML file name with path.</returns>
        private string BuildFileName(EntityStructure entityStructure, List<string> primaryKeyValues, string path)
        {
            StringBuilder builder = new StringBuilder(entityStructure.Name);

            for (int i = 0; i < entityStructure.PrimaryKeys.Count; i++)
            {
                builder.Append("_" + primaryKeyValues[i]);
            }

            builder.Append(".xml");

            string filename = builder.ToString();
            filename = filename.Replace("\\", "_").Replace("/", "_")
                               .Replace(":", "_").Replace("*", "_").Replace("?", "_")
                               .Replace("<", "_").Replace(">", "_").Replace("|", "_");
            string filepath = Path.Combine(path, "Entities", filename);
            return filepath;
        }

        /// <summary>
        /// Builds an XML file name based on the name and key values of the entity.
        /// </summary>
        /// <param name="entityName">The name of the entity.</param>
        /// <param name="primaryKeyValues">The primary key values.</param>
        /// <param name="path">The path where the file is stored.</param>
        /// <returns>The XML file name with path.</returns>
        private string BuildFileName(string entityName, List<string> primaryKeyValues, string path)
        {
            StringBuilder builder = new StringBuilder(entityName);

            for (int i = 0; i < primaryKeyValues.Count; i++)
            {
                builder.Append("_" + primaryKeyValues[i]);
            }

            builder.Append(".xml");

            string filename = builder.ToString();
            filename = filename.Replace("\\", "_").Replace("/", "_")
                               .Replace(":", "_").Replace("*", "_").Replace("?", "_")
                               .Replace("<", "_").Replace(">", "_").Replace("|", "_");
            string filepath = Path.Combine(path, "Entities", filename);
            return filepath;
        }

        /// <summary>
        /// A generic serializer.
        /// </summary>
        /// <typeparam name="T">Should be Entity or EntityStructure.</typeparam>
        /// <param name="object"> The object to serialize.</param>
        /// <param name="xmlFileName">The storage path where to save the serialized object.</param>
        private void Serialize<T>(T objectToSerialize, String xmlFileName)
        {
            var serializer = new YAXSerializer(typeof(T));
            var writer = new XmlTextWriter(xmlFileName, null);
            writer.Formatting = Formatting.Indented;
            serializer.Serialize(objectToSerialize, writer);
            writer.Close();
        }

        /// <summary>
        /// A generic deserializer.
        /// </summary>
        /// <typeparam name="T">Should be Entity or EntityStructure.</typeparam>
        /// <param name="xmlFilePath">The path of the file.</param>
        /// <returns>The deserialized object of the given type.</returns>
        private T Deserialize<T>(string xmlFilePath)
        {
            var deserializer = new YAXSerializer(typeof(T), YAXExceptionHandlingPolicies.ThrowErrorsOnly,
               YAXExceptionTypes.Warning);
            object deserializedObject = null;

            //XElement xElement = XElement.Load(xmlFilePath);
            deserializedObject = deserializer.Deserialize(File.ReadAllText(xmlFilePath));

            if (deserializer.ParsingErrors.ContainsAnyError)
            {
                Console.WriteLine("Succeeded to deserialize, but these problems also happened:");
                Console.WriteLine(deserializer.ParsingErrors.ToString());
            }

            return ((T)deserializedObject);
        }
    }
}