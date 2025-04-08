namespace Instructor.Core.Common.Seeds;

/// <summary>
/// Represents an instruction with a value of type <typeparamref name="TValue"/>.
/// </summary>
/// <typeparam name="TValue">The type of the value.</typeparam>
public interface IInstruction<TValue> { }

/// <summary>
/// Defines a handler for processing instructions of type <typeparamref name="TInstruction"/> and returning a value of type <typeparamref name="TValue"/>.
/// </summary>
/// <typeparam name="TInstruction">The type of the instruction.</typeparam>
/// <typeparam name="TValue">The type of the value.</typeparam>
public interface IInstructionHandler<TInstruction, TValue> where TInstruction : IInstruction<TValue> where TValue : notnull
{
    /// <summary>
    /// Handles the specified instruction.
    /// </summary>
    /// <param name="instruction">The instruction to handle.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the value of type <typeparamref name="TValue"/>.</returns>
    Task<TValue> Handle(TInstruction instruction, CancellationToken cancellationToken);
}

/// <summary>
/// Defines a handler for processing query instructions of type <typeparamref name="TInstruction"/> and returning a value of type <typeparamref name="TValue"/>.
/// </summary>
/// <typeparam name="TInstruction">The type of the instruction.</typeparam>
/// <typeparam name="TValue">The type of the value.</typeparam>
public interface IQueryHandler<TInstruction, TValue> : IInstructionHandler<TInstruction, TValue> where TInstruction : IInstruction<TValue> where TValue : notnull { }

/// <summary>
/// Defines a handler for processing command instructions of type <typeparamref name="TInstruction"/> and returning a value of type <typeparamref name="TValue"/>.
/// </summary>
/// <typeparam name="TInstruction">The type of the instruction.</typeparam>
/// <typeparam name="TValue">The type of the value.</typeparam>
public interface ICommandHandler<TInstruction, TValue> : IInstructionHandler<TInstruction, TValue>  where TInstruction : IInstruction<TValue> where TValue : notnull { }


/// <summary>
/// Dispatches instructions to the appropriate handler.
/// </summary>
public interface IInstructionDispatcher
{
    /// <summary>
    /// Sends the specified instruction to the appropriate handler.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="instruction">The instruction to send.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the value of type <typeparamref name="TValue"/>.</returns>
    Task<TValue> SendInstruction<TValue>(IInstruction<TValue> instruction, CancellationToken cancellationToken = default) where TValue : notnull;
}
