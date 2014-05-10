namespace AcceptanceTests
{
    using System;
    using Helpers;
    using LibGit2Sharp;

    public abstract class RepositoryFixtureBase : IDisposable
    {
        public readonly string RepositoryPath;
        public readonly IRepository Repository;

        protected RepositoryFixtureBase(Func<string, IRepository> repoBuilder)
        {
            RepositoryPath = PathHelper.GetTempPath();
            Repository = repoBuilder(RepositoryPath);
            Repository.Config.Set("user.name", "Test");
            Repository.Config.Set("user.email", "test@email.com");
        }

        public ExecutionResults ExecuteGitVersion()
        {
            return GitVersionHelper.ExecuteIn(RepositoryPath);
        }

        public void Dispose()
        {
            Repository.Dispose();

            try
            {
                DirectoryHelper.DeleteDirectory(RepositoryPath);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to clean up repository path at {0}. Received exception: {1}", RepositoryPath, e.Message);
            }
        }
    }
}
