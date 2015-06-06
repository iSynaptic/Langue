using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Langue
{
    [TestFixture]
    public partial class TextTests
    {
        [Test]
        public void InterleaveWith_WithMultipleInterleaves_Parses()
        {
            var parser = from greeting in Text.Literal("Hello")
                         from sep in Text.Literal(",")
                         from subject in Text.Literal("World")
                         from punc in Text.Literal("!")
                         select new {greeting, sep, subject, punc};

            parser = parser
                .InterleaveWith(Text.Literal(" "))
                .InterleaveWith(Text.Literal("\r"))
                .InterleaveWith(Text.Literal("\n"));

            var result = parser(@"  

            Hello

,
   World

     !


    ");

            result.HasValue.Should().BeTrue();
            result.Value.Should().NotBeNull();

            var value = result.Value;

            value.greeting.Should().Be("Hello");
            value.sep.Should().Be(",");
            value.subject.Should().Be("World");
            value.punc.Should().Be("!");
        }

        [Test]
        public void Or_WithNoResults_UsesDescriptionToBuildErrorMessage()
        {
            var parser = Text.Literal("public").Or(
                Text.Literal("private")).DescribeAs("visibility modifier").Or(
                    Text.Literal("class").DescribeAs("class keyword"));

            var result = parser("D");
            result.HasValue.Should().BeFalse();

            result.Observations.Count().Should().Be(1);
            var error = result.Observations.ElementAt(0);

            error.Message.Should().Be("Expected: visibility modifier or class keyword");
        }
    }
}
