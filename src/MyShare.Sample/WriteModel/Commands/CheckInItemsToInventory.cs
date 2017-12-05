﻿using System;
using MyShare.Kernel.Commands;

namespace MyShare.Sample.WriteModel.Commands
{
    public class CheckInItemsToInventory : ICommand 
	{
        public readonly int Count;

        public CheckInItemsToInventory(Guid id, int count, int originalVersion) 
		{
            Id = id;
            Count = count;
            ExpectedVersion = originalVersion;
        }

        public Guid Id { get; set; }
        public int ExpectedVersion { get; set; }
	}
}