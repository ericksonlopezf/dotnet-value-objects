// Copyright © Erickson Lopez. MIT License.
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AwesomeAssertions;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Automated Architectural Enforcement Suite ensuring long-term code quality,
/// copyright header compliance, zero-obsolete policy, kebab-case documentation naming,
/// and metadata consistency across the entire repository.
/// </summary>
public sealed class ConventionsTests
{
    private static readonly string SolutionRoot = FindSolutionRoot();

    private static string FindSolutionRoot()
    {
        string current = AppContext.BaseDirectory;
        while (!string.IsNullOrEmpty(current))
        {
            if (File.Exists(Path.Combine(current, "Directory.Build.props")) ||
                File.Exists(Path.Combine(current, "EricksonLopez.ValueObjects.slnx")) ||
                File.Exists(Path.Combine(current, "EricksonLopez.ValueObjects.sln")))
            {
                return current;
            }

            DirectoryInfo? parent = Directory.GetParent(current);
            if (parent == null) break;
            current = parent.FullName;
        }

        return Directory.GetCurrentDirectory();
    }

    [Fact]
    public void AllCSharpSourceFiles_ShouldContainMitLicenseHeader()
    {
        // Arrange
        const string expectedHeader = "// Copyright © Erickson Lopez. MIT License.";
        var csFiles = Directory.GetFiles(SolutionRoot, "*.cs", SearchOption.AllDirectories)
            .Where(f => !f.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}") &&
                        !f.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}") &&
                        !f.Contains($"{Path.DirectorySeparatorChar}.git{Path.DirectorySeparatorChar}"))
            .ToList();

        csFiles.Should().NotBeEmpty("Solution should contain C# source files");

        // Act & Assert
        var missingHeaderFiles = csFiles
            .Where(f =>
            {
                using var reader = new StreamReader(f);
                string? firstLine = reader.ReadLine()?.Trim();
                return firstLine != expectedHeader;
            })
            .ToList();

        missingHeaderFiles.Should().BeEmpty(
            $"Every C# file must start with '{expectedHeader}'. Missing in: {string.Join(", ", missingHeaderFiles.Select(Path.GetFileName))}");
    }

    [Fact]
    public void MarkdownDocumentationFiles_ShouldFollowKebabCaseNaming_ExceptStandardRootFiles()
    {
        // Arrange
        var allowedUppercaseRootFiles = new HashSet<string>(StringComparer.Ordinal)
        {
            "README.md",
            "SECURITY.md",
            "CONTRIBUTING.md",
            "CODE_OF_CONDUCT.md",
            "CHANGELOG.md",
            "SUPPORT.md",
            "AGENTS.md",
            "PULL_REQUEST_TEMPLATE.md"
        };

        var mdFiles = Directory.GetFiles(SolutionRoot, "*.md", SearchOption.AllDirectories)
            .Where(f => !f.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}") &&
                        !f.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}") &&
                        !f.Contains($"{Path.DirectorySeparatorChar}.git{Path.DirectorySeparatorChar}") &&
                        !f.Contains($"{Path.DirectorySeparatorChar}BenchmarkDotNet.Artifacts{Path.DirectorySeparatorChar}"))
            .ToList();

        mdFiles.Should().NotBeEmpty();

        var kebabCaseRegex = new Regex(@"^[a-z0-9\-_]+(\.[a-z0-9\-_]+)*\.md$", RegexOptions.Compiled);

        // Act
        var invalidFiles = mdFiles
            .Where(f =>
            {
                string fileName = Path.GetFileName(f);
                if (allowedUppercaseRootFiles.Contains(fileName))
                {
                    return false;
                }

                return !kebabCaseRegex.IsMatch(fileName);
            })
            .ToList();

        // Assert
        invalidFiles.Should().BeEmpty(
            $"All non-standard Markdown documentation files must follow lowercase kebab-case naming. Non-compliant: {string.Join(", ", invalidFiles.Select(Path.GetFileName))}");
    }

    [Fact]
    public void ProductionSourceCode_ShouldContainZeroObsoleteAttributes()
    {
        // Arrange
        string srcDir = Path.Combine(SolutionRoot, "src");
        var srcCsFiles = Directory.GetFiles(srcDir, "*.cs", SearchOption.AllDirectories)
            .Where(f => !f.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}") &&
                        !f.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}"))
            .ToList();

        srcCsFiles.Should().NotBeEmpty("Production source code should be present");

        var obsoleteRegex = new Regex(@"\[\s*Obsolete(\s*\(.*\))?\s*\]", RegexOptions.Compiled);

        // Act
        var filesWithObsolete = srcCsFiles
            .Where(f => obsoleteRegex.IsMatch(File.ReadAllText(f)))
            .Select(Path.GetFileName)
            .ToList();

        // Assert
        filesWithObsolete.Should().BeEmpty("Ecosystem enforces a strict zero-[Obsolete] policy in production code.");
    }

    [Fact]
    public void DirectoryBuildProps_ShouldContainCorrectRepositoryUrlAndAuthor()
    {
        // Arrange
        string propsPath = Path.Combine(SolutionRoot, "Directory.Build.props");
        File.Exists(propsPath).Should().BeTrue("Directory.Build.props must exist at root");

        string content = File.ReadAllText(propsPath);

        // Act & Assert
        content.Should().Contain("<Authors>Erickson Lopez</Authors>");
        content.Should().Contain("<RepositoryUrl>https://github.com/ericksonlopezf/dotnet-value-objects</RepositoryUrl>");
        content.Should().Contain("<PackageProjectUrl>https://ericksonlopez.dev/value-objects</PackageProjectUrl>");
    }
}
