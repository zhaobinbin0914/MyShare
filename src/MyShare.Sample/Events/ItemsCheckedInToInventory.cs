﻿using System;
using MyShare.Kernel.Events;

namespace MyShare.Sample.Events
{
    public class ItemsCheckedInToInventory : IEvent
    {
        public readonly int Count;
 
        public ItemsCheckedInToInventory(Guid id, int count) 
        {
            Id = id;
            Count = count;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }
}