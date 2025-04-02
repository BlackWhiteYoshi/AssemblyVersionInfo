﻿using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Reflection;

namespace AssemblyVersionInfo.Tests;

public sealed class ValidSyntaxTest {
    private static string[] GenerateSourceText(string input, out Compilation outputCompilation, out ImmutableArray<Diagnostic> diagnostics) {
        Generator generator = new();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
        driver = driver.RunGeneratorsAndUpdateCompilation(CreateCompilation(input), out outputCompilation, out diagnostics);

        GeneratorDriverRunResult runResult = driver.GetRunResult();
        GeneratorRunResult generatorResult = runResult.Results[0];
        return [.. generatorResult.GeneratedSources.Select((GeneratedSourceResult generatedSource) => generatedSource.SourceText.ToString())];


        static CSharpCompilation CreateCompilation(string source) {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(source);
            PortableExecutableReference metadataReference = MetadataReference.CreateFromFile(typeof(Binder).Assembly.Location);
            CSharpCompilationOptions compilationOptions = new(OutputKind.DynamicallyLinkedLibrary);

            return CSharpCompilation.Create("compilation", [syntaxTree], [metadataReference], compilationOptions);
        }
    }

    /// <summary>
    /// <para>During testing the version number is "0.0.0.0".</para>
    /// <para>I think, the version number can be set in <see cref="GenerateSourceText"/>, but I do not have figured it out yet.</para>
    /// </summary>
    /// <returns></returns>
    [Test]
    public async ValueTask ValidSyntax() {
        string sourceText = GenerateSourceText(string.Empty, out _, out _).Single();

        string expected = $$"""
            // <auto-generated/>
            #pragma warning disable
            #nullable enable annotations


            namespace AssemblyVersionInfo;

            /// <summary>
            /// This class provides constant strings holding information about the Assembly name and version.
            /// </summary>
            [System.CodeDom.Compiler.GeneratedCodeAttribute("AssemblyVersionInfo", "{{typeof(Generator).Assembly.GetName().Version!.ToString(3)}}")]
            internal static class Assembly {
                /// <summary>
                /// The simple name of the assembly.
                /// </summary>
                public const string NAME = "compilation";


                /// <summary>
                /// The major component of the version number, thats usually the first number.
                /// </summary>
                public const string VERSION_MAJOR = "0";

                /// <summary>
                /// The minor component of the version number, thats usually the second number.
                /// </summary>
                public const string VERSION_MINOR = "0";

                /// <summary>
                /// The build component of the version number, thats usually the third number.
                /// </summary>
                public const string VERSION_BUILD = "0";

                /// <summary>
                /// The revision component of the version number, thats usually the fourth number.
                /// </summary>
                public const string VERSION_REVISION = "0";


                /// <summary>
                /// <para>The full version number:</para>
                /// <para>{Major}.{Minor}.{Build}.{Revision}</para>
                /// </summary>
                public const string VERSION = "0.0.0.0";

                /// <summary>
                /// <para>Version number with only major and minor:</para>
                /// <para>{Major}.{Minor}</para>
                /// </summary>
                public const string VERSION_MAJOR_MINOR = "0.0";

                /// <summary>
                /// <para>Version number with only major, minor and build:</para>
                /// <para>{Major}.{Minor}.{Build}</para>
                /// </summary>
                public const string VERSION_MAJOR_MINOR_BUILD = "0.0.0";
            }

            """;
        await Assert.That(sourceText).IsEqualTo(expected);
    }
}
