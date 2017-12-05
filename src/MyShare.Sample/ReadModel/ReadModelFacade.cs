using System;
using System.Collections.Generic;
using MyShare.Sample.ReadModel.Dtos;
using MyShare.Sample.ReadModel.Infrastructure;

namespace MyShare.Sample.ReadModel
{
    public class ReadModelFacade : IReadModelFacade
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