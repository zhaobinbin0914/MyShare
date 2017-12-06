using System;
using System.Collections.Generic;
using MyShare.Sample.Dtos;

namespace MyShare.Sample.Infrastructure
{
    public static class InMemoryDatabase 
    {
        public static readonly Dictionary<Guid, InventoryItemDetailsDto> Details = new Dictionary<Guid,InventoryItemDetailsDto>();
        public static readonly List<InventoryItemListDto> List = new List<InventoryItemListDto>();
    }
}