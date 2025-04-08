using FluentAssertions;
using Instructor.Core.Common.Models;

namespace Instructor.Core.Unit.Tests.Common.Models;

public class NoneTests
{
    [Fact]
    public void None_value_should_return_the_same_instance()
    {
        var firstInstance  = None.Value;
        var secondInstance = None.Value;
        
        firstInstance.Should().Be(secondInstance);
    }

    [Fact]
    public void ToString_should_return_the_empty_set_character()
    {
        var noneInstance = None.Value;
       
        noneInstance.ToString().Should().Be("Ø");
    }
}
