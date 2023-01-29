namespace Survey.xIntegrationTests.Fixtures
{
    public class UserScope : IAsyncDisposable
    {
        private readonly FixtureImp _fixturesImp;
        private readonly string _email;

        public UserScope(FixtureImp fixtures, string email)
        {
            _fixturesImp = fixtures;
            _email = email;
        }

        public async ValueTask DisposeAsync()
        {
            await _fixturesImp.DeleteUserAsync(_email);
        }
    }
}
