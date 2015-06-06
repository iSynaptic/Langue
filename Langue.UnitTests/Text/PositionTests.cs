using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Langue
{
    [TestFixture]
    public class PositionTests
    {
        [Test]
        public void DefaultValue_HasIndex0Line1Column1()
        {
            var pos = default(Location);

            pos.Index.Should().Be(0);
            pos.Line.Should().Be(1);
            pos.Column.Should().Be(1);
        }

        [Test]
        public void NegativeIndex_ThrowsException()
        {
            Action<int> act = index => new Location(index, 1, 1);

            foreach (var index in Enumerable.Range(-10, 10))
            {
                int idx = index;
                act.Invoking(a => a(idx)).ShouldThrow<ArgumentOutOfRangeException>();
            }
        }

        [Test]
        public void Line_WhenZeroOrLessThan_ThrowsException()
        {
            Action<int> act = line => new Location(0, line, 1);

            foreach (var line in Enumerable.Range(-10, 11))
            {
                int l = line;
                act.Invoking(a => a(l)).ShouldThrow<ArgumentOutOfRangeException>();
            }
        }

        [Test]
        public void Column_WhenZeroOrLessThan_ThrowsException()
        {
            Action<int> act = column => new Location(0, 1, column);

            foreach (var column in Enumerable.Range(-10, 11))
            {
                int c = column;
                act.Invoking(a => a(c)).ShouldThrow<ArgumentOutOfRangeException>();
            }
        }

        [Test]
        public void ProvidedValuesAreReturned()
        {
            var pos = new Location(45, 13, 4);

            pos.Index.Should().Be(45);
            pos.Line.Should().Be(13);
            pos.Column.Should().Be(4);
        }

        [Test]
        public void DefaultValues_AreEqual()
        {
            var left = default(Location);
            var right = default(Location);

            (left == right).Should().BeTrue();
        }

        [Test]
        public void SameValues_AreEqual()
        {
            var left = new Location(42, 3, 7);
            var right = new Location(42, 3, 7);

            (left == right).Should().BeTrue();
        }

        [Test]
        public void DifferenceValues_AreNotEqual()
        {
            var left = new Location(42, 3, 13);

            var diffIndex = new Location(84, 3, 13);
            var diffLine = new Location(42, 7, 13);
            var diffColumn = new Location(42, 3, 22);

            (left != diffIndex).Should().BeTrue();
            (left != diffLine).Should().BeTrue();
            (left != diffColumn).Should().BeTrue();
        }

        [Test]
        public void NullValue_IsNotEqual()
        {
            var pos = default(Location);
            pos.Equals(null).Should().BeFalse();
        }

        [Test]
        public void NonPositionValue_IsNotEqual()
        {
            var pos = default(Location);
            pos.Equals("Hello!").Should().BeFalse();
        }

        [Test]
        public void SameValue_WhenProvidedAsObject_AreEqual()
        {
            var left = new Location(42, 4, 13);
            object right = new Location(42, 4, 13);

            left.Equals(right).Should().BeTrue();
        }

        [Test]
        public void SameValues_HaveSameHashCode()
        {
            var left = new Location(42, 4, 13);
            var right = new Location(42, 4, 13);

            var leftHash = left.GetHashCode();
            var rightHash = right.GetHashCode();

            leftHash.Should().Be(rightHash);
        }

        [Test]
        public void Advance_IncrementsIndexAndColumn()
        {
            var pos = new Location(42, 4, 13);
            var newPos = pos.Advance();

            newPos.Index.Should().Be(43);
            newPos.Line.Should().Be(4);
            newPos.Column.Should().Be(14);
        }

        [Test]
        public void AdvanceWithNewLine_IncrementsIndexAndLineAndResetsColumn()
        {
            var pos = new Location(42, 4, 13);
            var newPos = pos.AdvanceWithNewLine();

            newPos.Index.Should().Be(43);
            newPos.Line.Should().Be(5);
            newPos.Column.Should().Be(1);
        }
    }
}
