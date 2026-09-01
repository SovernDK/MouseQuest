using System;
using System.Collections.Generic;

namespace Atlas.DB
{
    [Serializable]
    public class Shop
    {
        public string name;
        public List<ItemPrototype> itemsOnSale;
    }
}