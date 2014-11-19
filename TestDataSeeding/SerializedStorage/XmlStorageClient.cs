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
using YAXLib;

namespace TestDataSeeding.SerializedStorage
{
    /// <summary>
    /// An XML based storage implementation of the ISerializedStorageClient.
    /// </summary>
    public class XmlStorageClient : ISerializedStorageClient
    {
        public void SaveEntity(Entity entity, EntityStructure entityStructure, string path)
        {
            if (!Directory.Exists(path + "\\Entities"))
            {
                Directory.CreateDirectory(path + "\\Entities");
            }

            try
            {
                var xmlFileName = BuildFileName(entity, entityStructure, path);
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

        public EntityStructures GetEntityStructures(string path)
        {
            EntityStructures entityStructures = null;

            try
            {
                entityStructures = Deserialize<EntityStructures>(path + "\\Structures.xml");
            }
            catch (Exception exception)
            {
                throw exception;
            }

            return entityStructures;
        }

        public void SaveEntityStructures(EntityStructures entityStructures, string path)
        {
            try
            {
                Serialize<EntityStructures>(entityStructures, path + "\\Structures.xml");
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
            StringBuilder builder = new StringBuilder(path + "\\Entities\\" + entity.Name);

            foreach (var keyName in entityStructure.PrimaryKeys)
            {
                builder.Append("_" + entity.AttributeValues[keyName]);
            }

            builder.Append(".xml");

            return builder.ToString();
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
            StringBuilder builder = new StringBuilder(path + "\\Entities\\" + entityStructure.Name);

            for (int i = 0; i < entityStructure.PrimaryKeys.Count; i++)
            {
                builder.Append("_" + primaryKeyValues[i]);
            }

            builder.Append(".xml");

            return builder.ToString();
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

            XElement xElement = XElement.Load(xmlFilePath);
            deserializedObject = deserializer.Deserialize(xElement.ToString());

            if (deserializer.ParsingErrors.ContainsAnyError)
            {
                Console.WriteLine("Succeeded to deserialize, but these problems also happened:");
                Console.WriteLine(deserializer.ParsingErrors.ToString());
            }

            return ((T) deserializedObject);
        }
    }
}