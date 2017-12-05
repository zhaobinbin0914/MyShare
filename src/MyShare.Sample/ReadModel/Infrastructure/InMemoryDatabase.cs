using System;
using System.Collections.Generic;
using MyShare.Sample.ReadModel.Dtos;

namespace MyShare.Sample.ReadModel.Infrastructure
{
    public static class InMemoryDatabase 
    {
        public static readonly Dictionary<Guid, InventoryItemDetailsDto> Details = new Dictionary<Guid,InventoryItemDetailsDto>();
        public static readonly List<InventoryItemListDto> List = new List<InventoryItemListDto>();
    }
}