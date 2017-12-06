using System;
using System.Collections.Generic;
using MyShare.Sample.Dtos;
using MyShare.Sample.Infrastructure;

namespace MyShare.Sample.Query
{
    public class QueryModelFacade : IQueryModelFacade
    {
        public IEnumerable<InventoryItemListDto> GetInventoryItems()
        {
            return InMemoryDatabase.List;
        }

        public InventoryItemDetailsDto GetInventoryItemDetails(Guid id)
        {
            return InMemoryDatabase.Details[id];
        }
    }
}