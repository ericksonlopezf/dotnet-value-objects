// Copyright © Erickson Lopez. MIT License.
using System;
using System.Linq;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Immutable Value Object representing a hierarchical tree node path using the Materialized Path pattern (e.g. <c>/1/4/12/</c>).
/// </summary>
public sealed record HierarchyPath : StringValueObject<HierarchyPath>
{
    private static readonly HierarchyPath RootInstance = new("/");

    /// <summary>The root level path constant (<c>/</c>).</summary>
    public static HierarchyPath RootConstant => RootInstance;

    /// <summary>
    /// Creates a root-level <see cref="HierarchyPath"/>, optionally initializing with the specified child segment.
    /// </summary>
    /// <param name="initialSegment">An optional root child segment (e.g. <c>"dept_1"</c> producing <c>"/dept_1/"</c>).</param>
    /// <returns>A new <see cref="HierarchyPath"/>.</returns>
    public static HierarchyPath Root(string? initialSegment = null)
    {
        if (string.IsNullOrWhiteSpace(initialSegment))
        {
            return RootInstance;
        }

        string clean = Sanitize(initialSegment);
        return new HierarchyPath($"/{clean}/");
    }

    private HierarchyPath(string value) : base(value) { }

    /// <summary>Gets the hierarchy depth level (root is 0, /1/ is 1, /1/4/ is 2).</summary>
    public int Depth => Value == "/" ? 0 : Value.Count(c => c == '/') - 1;

    /// <summary>Gets a value indicating whether this path represents the root node.</summary>
    public bool IsRoot => Value == "/";

    /// <summary>Gets the parent <see cref="HierarchyPath"/>, or <see langword="null"/> if this is the root path.</summary>
    public HierarchyPath? Parent
    {
        get
        {
            if (IsRoot)
            {
                return null;
            }

            string trimmed = Value[..^1]; // strip trailing slash
            int lastSlash = trimmed.LastIndexOf('/');
            return new HierarchyPath(Value[..(lastSlash + 1)]);
        }
    }

    /// <summary>
    /// Sanitizes an input string to be a valid path segment.
    /// </summary>
    public static string Sanitize(string segment) =>
        string.IsNullOrWhiteSpace(segment) ? string.Empty : segment.Replace('/', '_').Trim();

    /// <summary>
    /// Creates a validated <see cref="HierarchyPath"/> instance from a materialized path string.
    /// </summary>
    /// <param name="value">The path string formatted as <c>/segment1/segment2/.../</c>.</param>
    /// <returns>A <see cref="Result{HierarchyPath}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<HierarchyPath> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result<HierarchyPath>.Failure(Error.Validation(
                "HierarchyPath.Required", "Hierarchy path is required."));
        }

        string trimmed = value.Trim();
        if (!trimmed.StartsWith('/') || !trimmed.EndsWith('/'))
        {
            return Result<HierarchyPath>.Failure(Error.Validation(
                "HierarchyPath.InvalidFormat", "Hierarchy path must start and end with a forward slash ('/')."));
        }

        if (trimmed.Contains("//"))
        {
            return Result<HierarchyPath>.Failure(Error.Validation(
                "HierarchyPath.InvalidFormat", "Hierarchy path cannot contain empty segments or consecutive slashes ('//')."));
        }

        if (trimmed == "/")
        {
            return Result<HierarchyPath>.Success(RootInstance);
        }

        string[] segments = trimmed.Split('/', StringSplitOptions.RemoveEmptyEntries);

        foreach (string seg in segments)
        {
            if (string.IsNullOrWhiteSpace(seg) || seg.Any(c => c is '/' or '\\' or ' ' or '\t'))
            {
                return Result<HierarchyPath>.Failure(Error.Validation(
                    "HierarchyPath.InvalidSegment", $"Path segment '{seg}' contains invalid whitespace or delimiter characters."));
            }

            if (seg is ".." or ".")
            {
                return Result<HierarchyPath>.Failure(Error.Validation(
                    "HierarchyPath.InvalidSegment", $"Path segment '{seg}' cannot be a path traversal token."));
            }
        }

        return Result<HierarchyPath>.Success(new HierarchyPath(trimmed));
    }

    /// <summary>
    /// Reconstitutes a HierarchyPath from trusted persistence data without validation overhead.
    /// </summary>
    internal static HierarchyPath Hydrate(string value) => new(value);

    /// <summary>
    /// Appends a validated child segment to the current path, returning a new <see cref="HierarchyPath"/>.
    /// </summary>
    /// <param name="childSegment">The child segment to append.</param>
    /// <returns>A new <see cref="HierarchyPath"/> with the appended segment.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="childSegment"/> is null, whitespace, or contains invalid characters.</exception>
    public HierarchyPath Append(string childSegment)
    {
        var result = TryAppend(childSegment);
        if (result.IsFailure)
        {
            throw new ArgumentException(result.Error.Description, nameof(childSegment));
        }

        return result.Value;
    }

    /// <summary>
    /// Attempts to append a child segment to the current path, returning a <see cref="Result{HierarchyPath}"/>.
    /// </summary>
    /// <param name="childSegment">The child segment to append.</param>
    /// <returns>A <see cref="Result{HierarchyPath}"/> containing the new path or a validation error.</returns>
    public Result<HierarchyPath> TryAppend(string? childSegment)
    {
        if (string.IsNullOrWhiteSpace(childSegment))
        {
            return Result<HierarchyPath>.Failure(Error.Validation(
                "HierarchyPath.Required", "Child segment is required."));
        }

        string clean = childSegment.Trim().Trim('/');
        if (string.IsNullOrWhiteSpace(clean) || clean.Any(c => c is '/' or '\\' or ' ' or '\t'))
        {
            return Result<HierarchyPath>.Failure(Error.Validation(
                "HierarchyPath.InvalidSegment", $"Path segment '{clean}' contains invalid whitespace or delimiter characters."));
        }

        if (clean is ".." or ".")
        {
            return Result<HierarchyPath>.Failure(Error.Validation(
                "HierarchyPath.InvalidSegment", $"Path segment '{clean}' cannot be a path traversal token."));
        }

        return Result<HierarchyPath>.Success(new HierarchyPath($"{Value}{clean}/"));
    }

    /// <summary>Determines whether this path is an ancestor of the specified child path.</summary>
    public bool IsAncestorOf(HierarchyPath other) =>
        other is not null && other.Value.Length > Value.Length && other.Value.StartsWith(Value, StringComparison.Ordinal);

    /// <summary>Determines whether this path is a descendant of the specified parent path.</summary>
    public bool IsDescendantOf(HierarchyPath other) =>
        other is not null && other.IsAncestorOf(this);

    /// <summary>Determines whether this path is a direct child of the specified parent path.</summary>
    public bool IsDirectChildOf(HierarchyPath parent) =>
        parent is not null && Parent == parent;
}
