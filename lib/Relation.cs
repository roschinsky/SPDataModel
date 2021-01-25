using System;
using System.Collections.Generic;

namespace TRoschinsky.SPDataModel.Lib
{
    public class Relation
    {
        public Entity LookupFromEntity {get; private set;}
        public Entity LookupToEntity {get; private set;}

        public bool IsMultiLookup {get; set;} = false;

        public Relation(Entity from, Entity to)
        {
            LookupFromEntity = from;
            LookupToEntity = to;
        }
    }
}
