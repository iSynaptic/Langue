using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Collections;
using NUnit.Framework;

namespace Langue
{
    [TestFixture]
    public partial class TextTests
    {
        [Test]
        public void Literal_WithEmptyInput()
        {
            var parser = Text.Literal("Hello");

            var result = parser("");

            result.HasValue.Should().BeFalse();
            result.Description.Should().Be("literal");
            result.Observations.Should().HaveCount(1);
            result.Context.ConsumedTo.Should().Be(Position.Beginning);
            result.Context.ReadTo.Should().Be(Position.Beginning);

            var error = result.Observations.Cast<ParseError>().First();
            error.Position.Should().Be(Position.Beginning.ToRange());
            error.Message.Should().Be("Unexpected: end of input");
        }

        [Test]
        public void Literal_WithParialMatch()
        {
            var parser = Text.Literal("Hello");

            var result = parser("Help");

            result.HasValue.Should().BeFalse();
            result.Description.Should().Be("literal");
            result.Observations.Should().HaveCount(1);
            result.Context.ConsumedTo.Should().Be(Position.Beginning);
            result.Context.ReadTo.Should().Be(new Position(3, 1, 4));

            var error = result.Observations.Cast<ParseError>().First();
            error.Position.Should().Be(new Position(3, 1, 4).ToRange());
            error.Message.Should().Be("Unexpected: character 'p'");
        }

        [Test]
        public void Literal_WithUnexpectedEndOfInput()
        {
            var parser = Text.Literal("Hello, World!");

            var result = parser("Hello");

            result.HasValue.Should().BeFalse();
            result.Description.Should().Be("literal");
            result.Observations.Should().HaveCount(1);
            result.Context.ConsumedTo.Should().Be(Position.Beginning);
            result.Context.ReadTo.Should().Be(new Position(5, 1, 6));

            var error = result.Observations.Cast<ParseError>().First();
            error.Position.Should().Be(new Position(5, 1, 6).ToRange());
            error.Message.Should().Be("Unexpected: end of input");
        }

        [Test]
        public void Literal_WithFullMatch()
        {
            var parser = Text.Literal("Hello");

            var result = parser("Hello, World!");

            result.Value.Should().Be("Hello");
            result.HasValue.Should().BeTrue();
            result.Description.Should().Be("literal");
            result.Observations.Should().BeEmpty();
            result.Context.ConsumedTo.Should().Be(new Position(5, 1, 6));
            result.Context.ReadTo.Should().Be(new Position(5, 1, 6));
        }
    }
}
