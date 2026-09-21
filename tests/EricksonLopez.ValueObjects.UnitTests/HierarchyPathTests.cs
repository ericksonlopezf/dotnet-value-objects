// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

public sealed class HierarchyPathTests
{
    [Fact]
    public void Create_Root_Succeeds()
    {
        var result = HierarchyPath.Create("/");

        result.IsSuccess.Should().BeTrue();
        result.Value.IsRoot.Should().BeTrue();
        result.Value.Depth.Should().Be(0);
        result.Value.Parent.Should().BeNull();
    }

    [Fact]
    public void Create_NestedPath_SucceedsWithCorrectHierarchy()
    {
        var result = HierarchyPath.Create("/1/4/12/");

        result.IsSuccess.Should().BeTrue();
        result.Value.IsRoot.Should().BeFalse();
        result.Value.Depth.Should().Be(3);
        result.Value.Parent.Should().NotBeNull();
        result.Value.Parent!.Value.Should().Be("/1/4/");
        result.Value.Parent!.Parent!.Value.Should().Be("/1/");
    }

    [Fact]
    public void Append_ChildSegment_ReturnsNewPath()
    {
        var parent = HierarchyPath.Create("/categories/").Value;
        var child = parent.Append("electronics");

        child.Value.Should().Be("/categories/electronics/");
        parent.IsAncestorOf(child).Should().BeTrue();
        child.IsDescendantOf(parent).Should().BeTrue();
    }

    [Theory]
    [InlineData("1/4/12/")] // missing leading slash
    [InlineData("/1/4/12")]  // missing trailing slash
    public void Create_InvalidFormat_ReturnsFormatError(string invalid)
    {
        var result = HierarchyPath.Create(invalid);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("HierarchyPath.InvalidFormat");
    }

    [Theory]
    [InlineData("..")]
    [InlineData(".")]
    [InlineData(@"..\secret")]
    [InlineData("invalid/slash")]
    [InlineData("invalid\\backslash")]
    [InlineData("invalid space")]
    [InlineData("")]
    [InlineData("   ")]
    public void Append_WhenSegmentContainsTraversalOrInvalidCharacters_ThrowsArgumentException(string invalidSegment)
    {
        var parent = HierarchyPath.Create("/categories/").Value;
        Action act = () => parent.Append(invalidSegment);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void TryAppend_WhenValidSegment_ReturnsSuccess()
    {
        var parent = HierarchyPath.Create("/categories/").Value;
        var result = parent.TryAppend("hardware");

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("/categories/hardware/");
    }

    [Theory]
    [InlineData("..")]
    [InlineData(@"branch\override")]
    [InlineData("space inside")]
    public void TryAppend_WhenInvalidSegment_ReturnsFailure(string invalid)
    {
        var parent = HierarchyPath.Create("/categories/").Value;
        var result = parent.TryAppend(invalid);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("HierarchyPath.InvalidSegment");
    }

    [Fact]
    public void Root_And_Sanitize_WorkCorrectly()
    {
        HierarchyPath.RootConstant.Value.Should().Be("/");
        HierarchyPath.Root().Value.Should().Be("/");
        HierarchyPath.Root(null).Value.Should().Be("/");
        HierarchyPath.Root("").Value.Should().Be("/");
        HierarchyPath.Root("   ").Value.Should().Be("/");
        HierarchyPath.Root("electronics").Value.Should().Be("/electronics/");

        HierarchyPath.Sanitize("").Should().BeEmpty();
        HierarchyPath.Sanitize("it/security").Should().Be("it_security");

        HierarchyPath.Create(null).Error.Code.Should().Be("HierarchyPath.Required");
        HierarchyPath.Create("").Error.Code.Should().Be("HierarchyPath.Required");
        HierarchyPath.Create("/dept//sub/").Error.Code.Should().Be("HierarchyPath.InvalidFormat");

        var path = HierarchyPath.Hydrate("/a/b/");
        path.Value.Should().Be("/a/b/");

        var parent = HierarchyPath.Create("/categories/").Value;
        var child = parent.Append("electronics");
        var grandChild = child.Append("phones");

        child.IsDirectChildOf(parent).Should().BeTrue();
        grandChild.IsDirectChildOf(parent).Should().BeFalse();
        child.IsDirectChildOf(null!).Should().BeFalse();
        parent.IsAncestorOf(null!).Should().BeFalse();
        child.IsDescendantOf(null!).Should().BeFalse();
    }
}
