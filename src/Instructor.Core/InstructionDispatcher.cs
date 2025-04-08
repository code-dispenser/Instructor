using Instructor.Core.Common.Seeds;
using System.Reflection;

namespace Instructor.Core;

/// <summary>
/// Dispatches instructions to the appropriate handler based on their type.
/// </summary>
/// <param name="handlerResolver">A function that resolves the appropriate handler for the instruction type.</param>
public class InstructionDispatcher(Func<Type, object> handlerResolver) : IInstructionDispatcher
{
    private readonly Func<Type, object> _handlerResolver = handlerResolver;

    /// <summary>
    /// Sends the specified instruction to the appropriate handler based on its type.
    /// </summary>
    /// <typeparam name="TValue">The type of the value returned by the handler.</typeparam>
    /// <param name="instruction">The instruction to send.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the value of type <typeparamref name="TValue"/>.</returns>
    public Task<TValue> SendInstruction<TValue>(IInstruction<TValue> instruction, CancellationToken cancellationToken = default) where TValue : notnull
    {
        var instructionType = instruction.GetType();
        var handlerType     = typeof(IInstructionHandler<,>).MakeGenericType(instructionType, typeof(TValue));

        var handlerInstance = _handlerResolver(handlerType);
        var handleMethod    = handlerType.GetMethod(nameof(IInstructionHandler<IInstruction<TValue>, TValue>.Handle));

        return (Task<TValue>)handleMethod!.Invoke(handlerInstance, [instruction, cancellationToken])!;
    }

}
