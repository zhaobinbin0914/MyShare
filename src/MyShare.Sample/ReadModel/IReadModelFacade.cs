using System;
using System.Collections.Generic;
using MyShare.Sample.ReadModel.Dtos;

namespace MyShare.Sample.ReadModel
{
    public interface IReadModelFacade
    {
        IEnumerable<InventoryItemListDto> GetInventoryItems();
        InventoryItemDetailsDto GetInventoryItemDetails(Guid id);
    }
}