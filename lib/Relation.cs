using System;
using System.Collections.Generic;

namespace TRoschinsky.SPDataModel.Lib
{
    public class Relation
    {
        public bool IsResolvedRelation { get { return LookupFromEntity != null && LookupToEntity != null; } }
        public Entity LookupFromEntity { get; private set; }
        public string LookupFromEntityName { get; private set; } = String.Empty;
        public Entity LookupToEntity { get; private set; }
        public string LookupToEntityName { get; private set; } = String.Empty;

        public bool IsMultiLookup { get; set; } = false;

        public Relation(Entity from, Entity to)
        {
            LookupFromEntity = from;
            LookupFromEntityName = from.InternalName;
            LookupToEntity = to;
            LookupToEntityName = to.InternalName;
        }

        public Relation(string fromInternalName, string toInternalName)
        {
            LookupFromEntityName = fromInternalName;
            LookupToEntityName = toInternalName;
        }

        public Relation(string toInternalName)
        {
            LookupToEntityName = toInternalName;
        }

        public bool Resolve(Model model)
        {
            LookupFromEntity = model.Entities.Find(e => e.InternalName == LookupFromEntityName);
            if(LookupFromEntity != null && LookupFromEntity.InternalName != null)
            {
                LookupFromEntityName = LookupFromEntity.InternalName;
            }

            LookupToEntity = model.Entities.Find(e => e.InternalName == LookupToEntityName);            
            if(LookupToEntity != null && LookupToEntity.InternalName != null)
            {
                LookupToEntityName = LookupToEntity.InternalName;
            }

            return LookupFromEntity != null && LookupToEntity != null;
        }
    }
}