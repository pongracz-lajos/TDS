﻿using System.Linq;

using Tds.Engine.Exceptions;
using Tds.Interfaces;
using Tds.Interfaces.Model;
using Tds.Interfaces.Metadata;
using Tds.Types;

namespace Tds.Engine
{
    public class Api
    {
        #region Private fields
        private IMetadataProvider _metadataProvider;
        private IStorageProvider _productionStorageProvider;
        private IStorageProvider _backupStorageProvider;
        #endregion

        #region Constructurs
        public Api(IMetadataProvider metadataProvider = null, 
            IStorageProvider productionStorageProvider = null,
            IStorageProvider backupStorageProvider = null)
        {
            _metadataProvider = metadataProvider;
            _productionStorageProvider = productionStorageProvider;
            _backupStorageProvider = backupStorageProvider;
        }
        #endregion

        /*#region Public methods
        public void Backup(string entityName, params string[] keys)
        {
            // ==================================
            // Get entity structure
            // ==================================
            var entityStructure = _metadataProvider.GetEntityStructure(entityName);
            if (entityStructure == null)
            {
                throw new EntityStructureNotFoundException(entityName);
            }

            // ==================================
            // Get entity from database
            // ==================================
            var entityKeys = GetEntityKeys(entityStructure.Keys, keys);
            var entityFromDatabase = _productionStorageProvider.Read(entityName, entityKeys, entityStructure);
            if (entityFromDatabase == null) 
            {
                throw new EntityNotFoundInDatabaseException(entityName, 
                    entityKeys.ToDictionary(x => x.Name, x => x.Value.ToString()));
            }

            // ==================================
            // Save entity into a file
            // ==================================
            _backupStorageProvider.Write(entityFromDatabase, entityKeys, entityStructure);
        }
        #endregion

        #region Private fields
        private IEntityKey[] GetEntityKeys(IKeyStructure[] keysStructure, string[] keys)
        {
            var result = new IEntityKey[keysStructure.Length];
            
            var index = 0;
            foreach (var item in keysStructure.OrderBy(x => x.Sequence))
	        {
                result[index] = new IEntityKey() 
                { 
                    Name = item.Name,
                    Value = Converter.ConvertFromString(item.Type, keys[index])
                };

                index++;
	        }

            return result;
        }
        #endregion*/
    }
}
