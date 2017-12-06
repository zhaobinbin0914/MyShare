using System;
using System.Collections.Generic;
using MyShare.Sample.Dtos;

namespace MyShare.Sample.Query
{
    public interface IQueryModelFacade
    {
        IEnumerable<InventoryItemListDto> GetInventoryItems();
        InventoryItemDetailsDto GetInventoryItemDetails(Guid id);
    }
}