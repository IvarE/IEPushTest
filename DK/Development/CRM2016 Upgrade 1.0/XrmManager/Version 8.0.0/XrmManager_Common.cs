using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CGIXrmWin
{
    /// <summary>
    /// Some of the common fucntions are included here
    /// </summary>
    public partial class XrmManager
    {
        /// <summary>
        /// Remove all associated records of given relationshipName from the specified record
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="entityId"></param>
        /// <param name="associatedEntityName"></param>
        /// <param name="relationshipEntityName"></param>
        public void RemoveAssociates(string entityName, Guid entityId, string associatedEntityName, string relationshipEntityName)
        {
            //Get all associated circulations from supplement
            EntityReferenceCollection associatedCirculations = GetAssociatedRecords(entityName, entityId, associatedEntityName, relationshipEntityName);
            //Disassociate circulations
            pService.Disassociate(entityName, entityId, new Relationship(relationshipEntityName), associatedCirculations);
        }

        /// <summary>
        /// Copy associated records from a given record to other record
        /// </summary>
        /// <param name="fromEntityName"></param>
        /// <param name="fromEntityId"></param>
        /// <param name="toEntityName"></param>
        /// <param name="toEntityId"></param>
        /// <param name="associatedEntityName"></param>
        /// <param name="fromEntityRelationshipName"></param>
        /// <param name="toEntityRelationshipName"></param>
        public void CopyAssociates(string fromEntityName, Guid fromEntityId, string toEntityName, Guid toEntityId, string associatedEntityName, string fromEntityRelationshipName, string toEntityRelationshipName)
        {
            //Get assocaited records from fromEntity
            EntityReferenceCollection associatedRecords = GetAssociatedRecords(fromEntityName, fromEntityId, associatedEntityName, fromEntityRelationshipName);
            //Associate the records to toEntity
            pService.Associate(toEntityName, toEntityId, new Relationship(toEntityRelationshipName), associatedRecords);
        }

        /// <summary>
        /// Get all associated records for a given record and relationshipEntityName
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="entityId"></param>
        /// <param name="associatedEntityName"></param>
        /// <param name="relationshipEntityName"></param>
        /// <returns></returns>
        public EntityReferenceCollection GetAssociatedRecords(string entityName, Guid entityId, string associatedEntityName, string relationshipEntityName)
        {
            QueryExpression query = new QueryExpression(relationshipEntityName);
            query.ColumnSet = new ColumnSet(associatedEntityName + "id");
            query.Criteria = new FilterExpression(LogicalOperator.And);
            query.Criteria.AddCondition(new ConditionExpression(entityName + "id", ConditionOperator.Equal, entityId));
            Entity[] relationshipEntities = Get((QueryBase)query);
            EntityReferenceCollection associatedRecordReferenceCollection = new EntityReferenceCollection();
            foreach (Entity relationshipEntity in relationshipEntities)
            {
                associatedRecordReferenceCollection.Add(new EntityReference { Name = associatedEntityName, Id = relationshipEntity.GetAttributeValue<Guid>(associatedEntityName + "id") });
            }
            return associatedRecordReferenceCollection;
        }
    }
}
