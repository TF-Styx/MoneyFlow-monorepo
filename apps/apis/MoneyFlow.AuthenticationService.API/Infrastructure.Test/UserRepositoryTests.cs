using Microsoft.EntityFrameworkCore;
using MoneyFlow.AuthenticationService.Application.InterfaceRepositories;
using MoneyFlow.AuthenticationService.Application.Interfaces.Realization;
using MoneyFlow.AuthenticationService.Domain.DomainModels;
using MoneyFlow.AuthenticationService.Domain.ValueObjects;
using MoneyFlow.AuthenticationService.Infrastructure.Data;
using MoneyFlow.AuthenticationService.Infrastructure.Repositories;

namespace Infrastructure.Test
{
    internal class UserRepositoryTests : IDisposable
    {
        private readonly Context _context;
        private readonly IUserRepository _userRepository;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            _context = new Context(options);
            _context.Database.EnsureCreated();

            _userRepository = new UserRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        private UserDomain CreateSampleUserDomain(string? login = "testLogin", string? userName = "Styx", string? email = "test@example.com", string? phone = "01234567891", string? password = "4444")
        {
            var errors = new List<string>();
            var passwordHasher = new ArgonPasswordHasher();

            var loginResult = Login.TryCreate(login, out var loginResultVo);
            var emailAddressResult = EmailAddress.TryCreate(email, out var emailAddressVo);
            var phoneNumberResult = PhoneNumber.TryCreate(phone, out var phoneNumberVo);
            var passwordHash = passwordHasher.HashPassword(password);

            var (Domain, Message) = UserDomain.Create(loginResultVo, userName, passwordHash, emailAddressVo, phoneNumberVo, 1, 1);

            TestContext.Out.WriteLine($"PasswordHash: '{passwordHash}'");

            if (!string.IsNullOrEmpty(Message))
                TestContext.Out.WriteLine($"[SETUP INFO from CreateSimpleUserDomain] {Message}");

            if (!loginResult.IsValid)
                for (int i = 0; i < loginResult.ErrorList.Count; i++)
                    TestContext.Out.WriteLine($"[SETUP INFO from Login.TryCreate] {loginResult.ErrorList[i]}");

            if (!emailAddressResult.IsValid)
                for (int i = 0; i < emailAddressResult.ErrorList.Count; i++)
                    TestContext.Out.WriteLine($"[SETUP INFO from EmailAddress.TryCreate] {emailAddressResult.ErrorList[i]}");

            if (!phoneNumberResult.IsValid)
                for (int i = 0; i < phoneNumberResult.ErrorList.Count; i++)
                    TestContext.Out.WriteLine($"[SETUP INFO from PhoneNumber.TryCreate] {phoneNumberResult.ErrorList[i]}");

            return Domain;
        }

        #region Create

        [Test]
        public async Task CreateAsync_ShouldAddUserInMemoryDataBase_AndReturnAddedUserDomain()
        {
            var userDomainToCreate = CreateSampleUserDomain();

            TestContext.Out.WriteLine($"[SETUP INFO from CreateSampleUserDomain] - Исходные данные:");
            TestContext.Out.WriteLine($"[SETUP INFO from CreateSampleUserDomain] - Login: '{userDomainToCreate.Login}'");
            TestContext.Out.WriteLine($"[SETUP INFO from CreateSampleUserDomain] - UserName: '{userDomainToCreate.UserName}'");
            TestContext.Out.WriteLine($"[SETUP INFO from CreateSampleUserDomain] - Email: '{userDomainToCreate.Email}'");
            TestContext.Out.WriteLine($"[SETUP INFO from CreateSampleUserDomain] - Phone: '{userDomainToCreate.Phone}'");
            TestContext.Out.WriteLine($"[SETUP INFO from CreateSampleUserDomain] - PasswordHash: '{userDomainToCreate.PasswordHash}'");

            var returnUserDomain = await _userRepository.CreateAsync(userDomainToCreate);

            Assert.Multiple(() =>
            {
                Assert.That(returnUserDomain.Login,         Is.EqualTo(userDomainToCreate.Login));
                Assert.That(returnUserDomain.UserName,      Is.EqualTo(userDomainToCreate.UserName));
                Assert.That(returnUserDomain.Email,         Is.EqualTo(userDomainToCreate.Email));
                Assert.That(returnUserDomain.Phone,         Is.EqualTo(userDomainToCreate.Phone));
                Assert.That(returnUserDomain.PasswordHash,  Is.EqualTo(userDomainToCreate.PasswordHash));
            });

            var userInDB = await _context.Users.FirstOrDefaultAsync(x => x.Login == userDomainToCreate.Login);
            Assert.That(userInDB, Is.Not.Null);

            TestContext.Out.WriteLine($"[SETUP INFO from FirstOrDefaultAsync] - Исходные данные:");
            TestContext.Out.WriteLine($"[SETUP INFO from FirstOrDefaultAsync] - Login: '{userInDB.Login}'");
            TestContext.Out.WriteLine($"[SETUP INFO from FirstOrDefaultAsync] - UserName: '{userInDB.UserName}'");
            TestContext.Out.WriteLine($"[SETUP INFO from FirstOrDefaultAsync] - Email: '{userInDB.Email}'");
            TestContext.Out.WriteLine($"[SETUP INFO from FirstOrDefaultAsync] - Phone: '{userInDB.Phone}'");
            TestContext.Out.WriteLine($"[SETUP INFO from FirstOrDefaultAsync] - PasswordHash: '{userInDB.PasswordHash}'");

            Assert.Multiple(() =>
            {
                Assert.That(userInDB.Login,         Is.EqualTo(userDomainToCreate.Login));
                Assert.That(userInDB.UserName,      Is.EqualTo(userDomainToCreate.UserName));
                Assert.That(userInDB.Email,         Is.EqualTo(userDomainToCreate.Email));
                Assert.That(userInDB.Phone,         Is.EqualTo(userDomainToCreate.Phone));
                Assert.That(userInDB.PasswordHash,  Is.EqualTo(userDomainToCreate.PasswordHash));
            });
        }

        #endregion


        #region Get

        [Test]
        public async Task GetHashByLoginAsync_ShouldFindHash_AndReturnedHashFromStorage()
        {
            string secondLogin = "StorageLogin";
            string password = "StoragePassword11!!";

            var user = CreateSampleUserDomain(login: secondLogin, password: password);

            string expectedHas = user.PasswordHash;

            await _userRepository.CreateAsync(user);

            var actualStorageHash = await _userRepository.GetHashByLoginAsync(user.Login);

            TestContext.Out.WriteLine($"Новый хэш {expectedHas}");
            TestContext.Out.WriteLine($"Старый хэш {actualStorageHash}");

            Assert.That(actualStorageHash, Is.EqualTo(expectedHas));
        }

        [Test]
        public void GetHashByLoginAsync_ShouldFindHash_AndNotReturnedHashFromStorage_AndThrowsArgumentNullException()
        {
            var ex = Assert.ThrowsAsync<ArgumentNullException>(async () => await _userRepository.GetHashByLoginAsync("login"));

            Assert.That(ex.ParamName, Is.EqualTo("Пользователь не найден!!"));
        }

        #endregion


        #region ExistLogin

        [Test]
        public async Task ExistLogin_WhereExistLogin_ShouldReturnTrue()
        {
            var testLogin = "StyxExistLogin";
            var userDomain = CreateSampleUserDomain(login: testLogin);

            await _userRepository.CreateAsync(userDomain);

            var result = await _userRepository.ExistByLoginAsync(userDomain.Login);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task ExistLogin_WhereExistLogin_ShouldReturnFalse()
        {
            var testLogin = "StyxExistLogin";

            var result = await _userRepository.ExistByLoginAsync(testLogin);

            Assert.That(result, Is.False);
        }

        #endregion


        #region ExistEmail

        [Test]
        public async Task ExistEmail_WhereExistEmail_ShouldReturnTrue()
        {
            var testEmail = "styx.exist@mail.ru";
            var userDomain = CreateSampleUserDomain(email: testEmail);

            await _userRepository.CreateAsync(userDomain);

            var result = await _userRepository.ExistByEmailAsync(userDomain.Email);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task ExistEmail_WhereExistEmail_ShouldReturnFalse()
        {
            var testEmail = "styx.exist@mail.ru";

            var result = await _userRepository.ExistByEmailAsync(testEmail);

            Assert.That(result, Is.False);
        }

        #endregion


        #region ExistPhone

        [Test]
        public async Task ExistPhone_WhereExistPhone_ShouldReturnTrue()
        {
            var testPhone = "+7123456789";
            var userDomain = CreateSampleUserDomain(phone: testPhone);

            await _userRepository.CreateAsync(userDomain);

            var result = await _userRepository.ExistByPhoneAsync(userDomain.Phone);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task ExistPhone_WhereExistPhone_ShouldReturnFalse()
        {
            var testPhone = "+7123456789";

            var result = await _userRepository.ExistByPhoneAsync(testPhone);

            Assert.That(result, Is.False);
        }

        #endregion
    }
}
