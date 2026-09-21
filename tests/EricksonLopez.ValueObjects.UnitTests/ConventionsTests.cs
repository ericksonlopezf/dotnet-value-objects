// Copyright © Erickson Lopez. MIT License.
using System;
using System.Collections.Generic;
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
                        !f.Contains($"{Path.DirectorySeparatorChar}.git{Path.DirectorySeparatorChar}") &&
                        !f.Contains($"{Path.DirectorySeparatorChar}StrykerOutput{Path.DirectorySeparatorChar}") &&
                        !f.Contains($"{Path.DirectorySeparatorChar}scratch{Path.DirectorySeparatorChar}"))
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
            "GOVERNANCE.md",
            "PULL_REQUEST_TEMPLATE.md"
        };

        var mdFiles = Directory.GetFiles(SolutionRoot, "*.md", SearchOption.AllDirectories)
            .Where(f => !f.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}") &&
                        !f.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}") &&
                        !f.Contains($"{Path.DirectorySeparatorChar}.git{Path.DirectorySeparatorChar}") &&
                        !f.Contains($"{Path.DirectorySeparatorChar}BenchmarkDotNet.Artifacts{Path.DirectorySeparatorChar}") &&
                        !f.Contains($"{Path.DirectorySeparatorChar}MEGA-AUDITORIA{Path.DirectorySeparatorChar}") &&
                        !f.Contains($"{Path.DirectorySeparatorChar}StrykerOutput{Path.DirectorySeparatorChar}") &&
                        !f.Contains($"{Path.DirectorySeparatorChar}TestResults{Path.DirectorySeparatorChar}") &&
                        !f.Contains($"{Path.DirectorySeparatorChar}test-results{Path.DirectorySeparatorChar}"))
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

    [Fact]
    public void FiscalProjects_ShouldHaveDedicatedCountrySpecificIcons_DistinctFromRootIcon()
    {
        // Arrange
        string rootIconPath = Path.Combine(SolutionRoot, "icon.png");
        File.Exists(rootIconPath).Should().BeTrue("Root icon.png must exist at the solution root");

        byte[] rootIconBytes = File.ReadAllBytes(rootIconPath);
        rootIconBytes.Should().NotBeEmpty("Root icon.png must not be empty");
        byte[] rootIconHash = System.Security.Cryptography.SHA256.HashData(rootIconBytes);

        string[] fiscalProjects =
        [
            "Argentina",
            "Chile",
            "Colombia",
            "DominicanRepublic",
            "Mexico",
            "Peru"
        ];

        var fiscalHashes = new Dictionary<string, byte[]>(StringComparer.OrdinalIgnoreCase);

        // Act & Assert
        foreach (string country in fiscalProjects)
        {
            string projectDir = Path.Combine(SolutionRoot, "src", $"EricksonLopez.ValueObjects.Fiscal.{country}");
            Directory.Exists(projectDir).Should().BeTrue($"Fiscal project directory for {country} must exist");

            string iconPath = Path.Combine(projectDir, "icon.png");
            File.Exists(iconPath).Should().BeTrue($"Fiscal project for {country} must have its own dedicated icon.png at {iconPath}");

            byte[] iconBytes = File.ReadAllBytes(iconPath);
            iconBytes.Should().NotBeEmpty($"Icon for {country} must not be empty");

            byte[] iconHash = System.Security.Cryptography.SHA256.HashData(iconBytes);
            iconHash.SequenceEqual(rootIconHash).Should().BeFalse(
                $"Fiscal project for {country} must have a country-specific icon distinct from the root solution icon");

            fiscalHashes[country] = iconHash;
        }

        // Verify each country has a unique icon
        var uniqueHashes = fiscalHashes.Values.Distinct(new ByteArrayEqualityComparer()).Count();
        uniqueHashes.Should().Be(fiscalProjects.Length, "Each fiscal country project must have a distinct, unique icon");
    }

    [Fact]
    public void AllSourceProjects_ShouldResolveValidPackageIcon_ViaLocalOrRootFallback()
    {
        // Arrange
        string srcDir = Path.Combine(SolutionRoot, "src");
        string rootIconPath = Path.Combine(SolutionRoot, "icon.png");
        File.Exists(rootIconPath).Should().BeTrue("Root icon.png must exist as fallback");

        var csprojFiles = Directory.GetFiles(srcDir, "*.csproj", SearchOption.AllDirectories)
            .Where(f => !f.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}") &&
                        !f.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}"))
            .ToList();

        csprojFiles.Should().NotBeEmpty("Solution should contain source projects");

        // Act & Assert
        foreach (string csproj in csprojFiles)
        {
            string projectFolder = Path.GetDirectoryName(csproj)!;
            string localIcon = Path.Combine(projectFolder, "icon.png");

            string effectiveIcon = File.Exists(localIcon) ? localIcon : rootIconPath;
            File.Exists(effectiveIcon).Should().BeTrue($"Project {Path.GetFileName(csproj)} must resolve a valid icon.png");

            var info = new FileInfo(effectiveIcon);
            info.Length.Should().BeGreaterThan(0, $"Effective icon for {Path.GetFileName(csproj)} must not be 0 bytes");
        }
    }

    [Fact]
    public void DirectoryBuildProps_ShouldEnforcePackageIconConfigurationAndFallbackPackaging()
    {
        // Arrange
        string propsPath = Path.Combine(SolutionRoot, "Directory.Build.props");
        string content = File.ReadAllText(propsPath);

        // Act & Assert
        content.Should().Contain("<PackageIcon>icon.png</PackageIcon>",
            "Directory.Build.props must globally declare PackageIcon as icon.png");
        content.Should().Contain(@"<None Include=""icon.png"" Pack=""true"" PackagePath=""\"" Condition=""Exists('icon.png')"" />",
            "Directory.Build.props must pack local project icon when it exists");
        content.Should().Contain(@"<None Include=""$(MSBuildThisFileDirectory)icon.png"" Pack=""true"" PackagePath=""\"" Condition=""!Exists('icon.png') and Exists('$(MSBuildThisFileDirectory)icon.png')"" />",
            "Directory.Build.props must pack fallback root icon when local icon does not exist");
    }

    [Fact]
    public void DirectoryBuildProps_ShouldEnforceImplicitUsingsDisabled()
    {
        // Arrange
        string propsPath = Path.Combine(SolutionRoot, "Directory.Build.props");
        string content = File.ReadAllText(propsPath);

        // Act & Assert
        content.Should().Contain("<ImplicitUsings>disable</ImplicitUsings>",
            "Directory.Build.props must globally declare ImplicitUsings as disable.");
    }

    [Fact]
    public void AllCSharpSourceFiles_ShouldContainZeroPragmaWarningDisableDirectives()
    {
        // Arrange
        var csFiles = Directory.GetFiles(SolutionRoot, "*.cs", SearchOption.AllDirectories)
            .Where(f => !f.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}") &&
                        !f.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}") &&
                        !f.Contains($"{Path.DirectorySeparatorChar}.git{Path.DirectorySeparatorChar}") &&
                        !f.Contains($"{Path.DirectorySeparatorChar}StrykerOutput{Path.DirectorySeparatorChar}"))
            .ToList();

        var pragmaRegex = new Regex(@"^\s*#pragma\s+warning\s+disable\b", RegexOptions.Multiline | RegexOptions.Compiled);

        // Act
        var filesWithPragma = csFiles
            .Where(f => pragmaRegex.IsMatch(File.ReadAllText(f)))
            .Select(Path.GetFileName)
            .ToList();

        // Assert
        filesWithPragma.Should().BeEmpty(
            $"Zero '#pragma warning disable' directives are permitted in the ecosystem. Non-compliant: {string.Join(", ", filesWithPragma)}");
    }

    [Fact]
    public void SecurityPolicy_ShouldReflectCurrentMajorVersionSupport()
    {
        // Arrange
        string secPath = Path.Combine(SolutionRoot, "SECURITY.md");
        File.Exists(secPath).Should().BeTrue("SECURITY.md must exist");

        string content = File.ReadAllText(secPath);

        // Act & Assert
        content.Should().Contain("**2.0.x**", "SECURITY.md must support active 2.0.x major release.");
        content.Should().Contain("**1.0.x**", "SECURITY.md must explicitly document legacy 1.0.x status.");
    }

    [Fact]
    public void IssueTemplates_ShouldFollowKebabCaseNamingAndMaintainerAssignees()
    {
        // Arrange
        string issueTmplDir = Path.Combine(SolutionRoot, ".github", "ISSUE_TEMPLATE");
        if (!Directory.Exists(issueTmplDir)) return;

        var tmplFiles = Directory.GetFiles(issueTmplDir);

        // Act & Assert
        foreach (string tmpl in tmplFiles)
        {
            string fileName = Path.GetFileName(tmpl);
            fileName.Should().NotContain("_", $"Issue template '{fileName}' must use kebab-case naming.");

            string content = File.ReadAllText(tmpl);
            content.Should().NotContain("assignees: 'ericksonlopez'",
                $"Issue template '{fileName}' must reference official maintainer handle 'ericksonlopezf'.");
        }
    }

    private sealed class ByteArrayEqualityComparer : IEqualityComparer<byte[]>
    {
        public bool Equals(byte[]? x, byte[]? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x is null || y is null) return false;
            return x.SequenceEqual(y);
        }

        public int GetHashCode(byte[] obj)
        {
            if (obj is null) return 0;
            var hash = new HashCode();
            foreach (byte b in obj)
            {
                hash.Add(b);
            }
            return hash.ToHashCode();
        }
    }
}

