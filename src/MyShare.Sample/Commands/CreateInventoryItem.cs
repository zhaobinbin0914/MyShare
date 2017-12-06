using System;
using MyShare.Kernel.Commands;

namespace MyShare.Sample.Commands
{
    public class CreateInventoryItem : ICommand 
	{
        public readonly string Name;
	    
        public CreateInventoryItem(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }
        public int ExpectedVersion { get; set; }
	}
}