﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tds.Engine.Exceptions;
using Tds.Interfaces;
using Tds.Interfaces.Model;

namespace Tds.Engine.Core
{
    class MaintenanceTask
    {
        public IRepository SourceRepository { get; set; }

        public IRepository TargetRepository { get; set; }

        public IMetadataWorkspace MetadataWorkspace { get; set; }

        public IDependencyResolver DependencyResolver { get; set; }

        public void Save(string entityName, IEnumerable<EntityKey> keyMembers)
        {
            Entity entity = SourceRepository.Read(entityName, keyMembers).First();

            if (entity != null)
            {
                Save(entity);
            }
            else
            {
                throw new EntityNotFoundInDatabaseException(entity.Name, keyMembers, MetadataWorkspace.GetEntityType(entity.Name));
            }
        }

        private void Save(Entity sourceEntity)
        {
            SaveEntitiesWhereCurrentEntityIsDependent(sourceEntity);

            SaveCurrentEntity(sourceEntity);

            SaveEntitiesWhereCurrentEntityIsPrincipal(sourceEntity);
        }

        private IEnumerable<EntityKey> GetEntityPrimaryKey(Entity entity)
        {
            var entityType = MetadataWorkspace.GetEntityType(entity.Name);
            var result = new EntityKey[entityType.PrimaryKey.Count];

            int index = 0;
            foreach (var keyMember in entityType.PrimaryKey.OrderBy(member => member.Sequence))
            {
                result[index] = new EntityKey()
                {
                    Name = keyMember.Name,
                    Value = entity.Properties[keyMember.Name]
                };

                index++;
            }

            return result;
        }

        private void SaveCurrentEntity(Entity sourceEntity)
        {
            var primaryKey = GetEntityPrimaryKey(sourceEntity);
            var targetEntity = TargetRepository.Read(sourceEntity.Name, primaryKey);

            if (targetEntity != null)
            {
                if (!sourceEntity.Equals(targetEntity))
                {
                    TargetRepository.Write(sourceEntity, primaryKey, EntityStatus.Modified);
                }
            }
            else
            {
                TargetRepository.Write(sourceEntity, primaryKey);
            }
        }

        private void SaveEntitiesWhereCurrentEntityIsDependent(Entity sourceEntity)
        {
            foreach (var association in MetadataWorkspace.GetAssociationsWhereEntityIsDependent(sourceEntity.Name))
            {
                foreach (var entity in DependencyResolver.GetEntitiesWhereEntityIsDependent(sourceEntity, association))
                {
                    Save(entity);
                }
            }
        }

        private void SaveEntitiesWhereCurrentEntityIsPrincipal(Entity sourceEntity)
        {
            foreach (var association in MetadataWorkspace.GetAssociationsWhereEntityIsPrincipal(sourceEntity.Name))
            {
                foreach (var entity in DependencyResolver.GetEntitiesWhereEntityIsPrincipal(sourceEntity, association))
                {
                    Save(entity);
                }
            }
        }
    }
}
