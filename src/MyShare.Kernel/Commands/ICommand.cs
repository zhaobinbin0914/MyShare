using MyShare.Kernel.Messages;

namespace MyShare.Kernel.Commands
{
    /// <summary>
    /// Defines an command with required fields.
    /// </summary>
    public interface ICommand : IMessage
    {
        int ExpectedVersion { get; }
    }
}