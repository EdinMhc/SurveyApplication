using Survey.Domain.Services.IdentityService.Requests;

namespace Survey.xIntegrationTests.Fixtures
{
    public class UserScope : IAsyncDisposable
    {
        private readonly FixtureImp _fixturesImp;
        private readonly string _email;
        private readonly Role _role;

        public UserScope(FixtureImp fixtures, string email, Role role)
        {
            _fixturesImp = fixtures;
            _email = email;
            _role = role;
        }

        public async ValueTask DisposeAsync()
        {
            await _fixturesImp.DeleteUserAsync(_email, _role);
        }
    }
}
