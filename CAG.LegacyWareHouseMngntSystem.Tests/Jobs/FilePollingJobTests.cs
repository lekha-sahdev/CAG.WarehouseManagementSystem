using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Quartz;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using CAG.LegacyWareHouseMngntSystem.Jobs;

public class FilePollingJobTests
{
    private readonly Mock<ILogger<FilePollingJob>> _loggerMock;
    private readonly Mock<IConfiguration> _configMock;
    private readonly Mock<IFileProcessService> _fileProcessServiceMock;
    private readonly FilePollingJob _job;
    private readonly Mock<IJobExecutionContext> _jobContextMock;

    public FilePollingJobTests()
    {
        _loggerMock = new Mock<ILogger<FilePollingJob>>();
        _configMock = new Mock<IConfiguration>();
        _fileProcessServiceMock = new Mock<IFileProcessService>();
        _job = new FilePollingJob(_loggerMock.Object, _configMock.Object, _fileProcessServiceMock.Object);
        _jobContextMock = new Mock<IJobExecutionContext>();
    }

    [Fact]
    public async Task Execute_LocalMode_ProcessesAllFiles()
    {
        // Arrange
        var testDir = "testdir";
        Directory.CreateDirectory(testDir);
        var file1 = Path.Combine(testDir, "file1.txt");
        var file2 = Path.Combine(testDir, "file2.txt");
        File.WriteAllText(file1, "data1");
        File.WriteAllText(file2, "data2");
        var localPathSection = new Mock<IConfigurationSection>();
		localPathSection.Setup(s => s.Value).Returns(testDir);
		_configMock.Setup(c => c.GetSection("Polling:LocalPath")).Returns(localPathSection.Object);


		// Act
		await _job.Execute(_jobContextMock.Object);

        // Assert
        _fileProcessServiceMock.Verify(f => f.ProcessFileAsync(file1), Times.Once);
        _fileProcessServiceMock.Verify(f => f.ProcessFileAsync(file2), Times.Once);

        // Cleanup
        File.Delete(file1);
        File.Delete(file2);
        Directory.Delete(testDir);
    }
}
