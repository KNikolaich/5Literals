using System.IO;
using System;
using System.Threading.Tasks;
using Xunit;

namespace WordChainsGenerator.Tests;

public class ProgramDeletionTests
{
    [Fact]
    public async Task InteractiveDeletion_RemovesAllMatchingLines_CaseInsensitive()
    {
        // Arrange: create a temp words file with duplicate-case entries
        var temp = Path.Combine(Path.GetTempPath(), $"words_test_{Guid.NewGuid()}.txt");
        File.WriteAllLines(temp, new[] { "кумыс", "КУМЫС", "остальное" });

        var input = new StringReader("-d кумыс\n");
        var output = new StringWriter();
        var originalIn = Console.In;
        var originalOut = Console.Out;

        try
        {
            Console.SetIn(input);
            Console.SetOut(output);

            // Act
            var exitCode = await Program.Main(new[] { temp });

            // Assert
            Assert.Equal(0, exitCode);
            var lines = File.ReadAllLines(temp);
            Assert.Single(lines);
            Assert.Equal("остальное", lines[0]);
        }
        finally
        {
            Console.SetIn(originalIn);
            Console.SetOut(originalOut);
            File.Delete(temp);
        }
    }
}
