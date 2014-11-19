﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDataSeeding.Model;

namespace TestDataSeeding.Logic
{
    /// <summary>
    /// Database client interface.
    /// </summary>
    public interface IDbClient
    {
        /// <summary>
        /// Returns a new entity identified by the entity name from the <paramref name="entityStructure"/> and <paramref name="primaryKeyValues"/>,
        /// or null, if there is no such entity.
        /// </summary>
        /// <param name="entityStructure">The structure of the entity.</param>
        /// <param name="primaryKeyValues">A list with the pramary key values that identifies the entity.</param>
        /// <returns>A new entity identified by <paramref name="entityName"/> and <paramref name="primaryKeyValues"/>.</returns>
        Entity GetEntity(EntityStructure entityStructure, List<string> primaryKeyValues);

        /// <summary>
        /// Add the <paramref name="entity"/> to be inserted on next ExecuteTransaction() call.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="entityStructure">The structure of the entity.</param>
        void InsertWithTransaction(Entity entity, EntityStructure entityStructure);

        /// <summary>
        /// Add the <paramref name="entity"/> to be updated on next ExecuteTransaction() call.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="entityStructure">The structure of the entity.</param>
        void UpdateWithTransaction(Entity entity, EntityStructure entityStructure);

        /// <summary>
        /// Restores all the Entities added with Insert- or UpdateWithTransaction() or none, if an operation fails.
        /// On successful transaction, clears the transaction data for a fresh new transaction.
        /// </summary>
        void ExecuteTransaction();
    }
}
