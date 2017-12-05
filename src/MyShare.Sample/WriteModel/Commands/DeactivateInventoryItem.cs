﻿using System;
using MyShare.Kernel.Commands;

namespace MyShare.Sample.WriteModel.Commands
{
    public class DeactivateInventoryItem : ICommand 
	{
        public DeactivateInventoryItem(Guid id, int originalVersion)
        {
            Id = id;
            ExpectedVersion = originalVersion;
        }

        public Guid Id { get; set; }
        public int ExpectedVersion { get; set; }
	}
}