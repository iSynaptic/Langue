using System;
using FluentAssertions;
using NUnit.Framework;

namespace Langue
{
    [TestFixture]
    public class TextContextTests
    {
        [Test]
        public void Context_WithZeroLengthInput_ReturnsExpectedValues()
        {
            string input = "";
            var ctx = new TextContext(input);

            ctx.Input.Should().Be(input);
            ctx.ReadTo.Should().Be(Position.Beginning);
            ctx.ConsumedTo.Should().Be(Position.Beginning);
            ctx.Source.Should().Be(String.Empty);
            ctx.AtEnd.Should().BeTrue();
            ctx.Interleaving.Should().BeNull();
        }

        [Test]
        public void Context_WithNonZeroLengthInput_ReturnsExpectedValues()
        {
            string input = "Hello, World!";
            var ctx = new TextContext(input);

            ctx.Input.Should().Be(input);
            ctx.ReadTo.Should().Be(Position.Beginning);
            ctx.ConsumedTo.Should().Be(Position.Beginning);
            ctx.Current.Should().Be('H');
            ctx.Source.Should().Be(String.Empty);
            ctx.AtEnd.Should().BeFalse();
            ctx.Interleaving.Should().BeNull();
        }

        [Test]
        public void Context_WithNullInput_ThrowsException()
        {
            Action act = () => new TextContext(null);

            act.Invoking(a => a()).ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void ReadWithCount_ReturnsContext_WithNewPositionAndCurrent()
        {
            string input = "Hello, World!";
            var ctx = new TextContext(input);

            ctx = ctx.Read(7);

            ctx.ReadTo.Should().Be(new Position(7, 1, 8));
            ctx.ConsumedTo.Should().Be(Position.Beginning);
            ctx.Current.Should().Be('W');
        }

        [Test]
        public void Read_WhenBuildingNewContext_RespectsNewLines()
        {
            string input = "Hello\r\nWorld!";
            var ctx = new TextContext(input).Read(6);

            ctx.ReadTo.Should().Be(new Position(6, 1, 7));

            ctx = ctx.Read(1);

            ctx.ReadTo.Should().Be(new Position(7, 2, 1));
            ctx.ConsumedTo.Should().Be(Position.Beginning);
        }

        [Test]
        public void ReadWithCount_WhenBuildingNewContext_RespectsNewLines()
        {
            string input = "Hello\r\nWorld!";
            var ctx = new TextContext(input);
            
            ctx = ctx.Read(7);

            ctx.ReadTo.Should().Be(new Position(7, 2, 1));
            ctx.ConsumedTo.Should().Be(Position.Beginning);
        }

        [Test]
        public void Advancing_ByNegativeCount_ThrowsException()
        {
            var ctx = new TextContext("Hello");
            ctx.Invoking(c => c.Read(-2)).ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        public void Advancing_ByZero_ReturnsUnchangedContext()
        {
            var ctx = new TextContext("Hello");

            var newCtx = ctx.Read(0);

            newCtx.ReadTo.Should().Be(ctx.ReadTo);
            newCtx.ConsumedTo.Should().Be(ctx.ConsumedTo);
            newCtx.Input.Should().Be(ctx.Input);
            newCtx.Current.Should().Be(ctx.Current);
            newCtx.Source.Should().Be(ctx.Source);
            newCtx.AtEnd.Should().Be(ctx.AtEnd);
        }

        [Test]
        public void Read_WhenAtEnd_ThrowsException()
        {
            var ctx = new TextContext("");

            ctx.Invoking(c => c.Read(1)).ShouldThrow<ArgumentException>();
        }

        [Test]
        public void Read_PastEndOfInput_ThrowsException()
        {
            var ctx = new TextContext("Hello");

            ctx.Invoking(c => c.Read(10)).ShouldThrow<ArgumentException>();
        }
        
        [Test]
        public void Consume_AtBeginning_ReadAndConsumesNothing()
        {
            var ctx = new TextContext("Hello");

            ctx = ctx.Consume();

            ctx.ReadTo.Should().Be(Position.Beginning);
            ctx.ConsumedTo.Should().Be(Position.Beginning);
        }

        [Test]
        public void WithInterleave_ReturnsContext_WithInterleaving()
        {
            var ctx = new TextContext("Hello");

            Pattern<object, TextContext> interleaving = c => Match<object>.Success(42, "ultimate", c);
            ctx = ctx.WithInterleave(interleaving);

            ctx.Interleaving.Should().Be(interleaving);
        }

        [Test]
        public void WithInterleave_ReturnsContext_WithOtherValuesUnchanged()
        {
            var ctx = new TextContext("Hello").Read(3);

            Pattern<object, TextContext> interleaving = c => Match<object>.Success(42, "ultimate", c);
            var newCtx = ctx.WithInterleave(interleaving);

            newCtx.Input.Should().Be(ctx.Input);
            newCtx.ReadTo.Should().Be(ctx.ReadTo);
            newCtx.ConsumedTo.Should().Be(ctx.ConsumedTo);
            newCtx.Current.Should().Be(ctx.Current);
            newCtx.Source.Should().Be(ctx.Source);
            newCtx.AtEnd.Should().Be(ctx.AtEnd);

            newCtx.Interleaving.Should().Be(interleaving);
        }

        [Test]
        public void ImplicitConvertion_FromStringToContext_Exists()
        {
            string input = "Hello";
            TextContext ctx = input;

            ctx.Input.Should().Be(input);
            ctx.ReadTo.Should().Be(Position.Beginning);
            ctx.ConsumedTo.Should().Be(Position.Beginning);
            ctx.Current.Should().Be('H');
            ctx.Source.Should().Be(String.Empty);
            ctx.AtEnd.Should().BeFalse();
            ctx.Interleaving.Should().BeNull();

        }
    }
}
