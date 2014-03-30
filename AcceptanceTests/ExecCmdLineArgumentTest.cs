﻿
namespace GitHubFlowVersion.AcceptanceTests
{
    using global::AcceptanceTests.Properties;
    using System.IO;
    using Helpers;
    using Shouldly;
    using Xunit;

    public class ExecCmdLineArgumentTest
    {
        private const string MsBuild = @"c:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe";
        private const string TaggedVersion = "1.2.3";

        [Fact]
        public void RunExecViaCommandLine()
        {
            using (var fixture = new EmptyRepositoryFixture())
            {
                fixture.Repository.MakeATaggedCommit(TaggedVersion);
                fixture.Repository.MakeACommit();

                var buildFile = Path.Combine(fixture.RepositoryPath, "TestBuildFile.proj");
                File.WriteAllBytes(buildFile, Resources.TestBuildFile);
                var result = GitVersionHelper.ExecuteIn(fixture.RepositoryPath, MsBuild, "TestBuildFile.proj /target:OutputResults");

                result.ExitCode.ShouldBe(0);
                result.Log.ShouldContain("GitVersion_FullSemVer: 1.2.4+1");
            }
        }
    }
}