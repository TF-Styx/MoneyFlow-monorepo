using MoneyFlow.AuthenticationService.Application.Enums;
using MoneyFlow.AuthenticationService.Application.Interfaces.Realization;

namespace Application.Test
{
    internal class PasswordHasherTest
    {
        private ArgonPasswordHasher _passwordHasher;

        [SetUp]
        public void SetUp()
        {
            _passwordHasher = new ArgonPasswordHasher();
        }

        private static bool IsValidBase64(string input)
        {
            try
            {
                Convert.FromBase64String(input);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        [Test]
        public void ArgonPasswordHasher_HashPassword_ResultCorrectlyFormattedString() //"Argon2id:2:3:65536:someSalt:someHash" - Пример проверяемой строки
        {
            string hash = _passwordHasher.HashPassword("Argon2TestHashed!!");

            var partsHash = hash.Split(':');
            TestContext.Out.WriteLine(hash);

            Assert.Multiple(() =>
            {
                Assert.That(hash, Is.Not.Null, "Строка не должна быть пустой");
                Assert.That(partsHash[0], Is.EqualTo(Hashers.Argon2id.ToString()), "Первая часть строки должна быть 'Argon2id'");
                Assert.That(int.TryParse(partsHash[1], out _), Is.True, "Вторая часть строки должна быть числом потоков используемых для хэширования");
                Assert.That(int.TryParse(partsHash[2], out _), Is.True, "Первая часть строки должна быть числом итераций произведённых для хэширования");
                Assert.That(int.TryParse(partsHash[3], out _), Is.True, "Первая часть строки должна быть число памяти выделенной для хэширования");
                Assert.That(partsHash[4], Is.Not.Empty, "Соль не должна быть пустой");
                Assert.That(IsValidBase64(partsHash[4]), Is.True, "Соль должна быть валидной Base64 строкой");
                Assert.That(partsHash[5], Is.Not.Empty, "Хэш не должна быть пустой");
                Assert.That(IsValidBase64(partsHash[5]), Is.True, "Хэш должен быть валидной Base64 строкой");
            });
        }

        [Test]
        public void ArgonPasswordHasher_HashPassword_EmptyPassword_ThrowArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _passwordHasher.HashPassword(""));

            Assert.That(ex.ParamName, Is.EqualTo(""));
        }

        [Test]
        public void ArgonPasswordHasher_HashPassword_NullPassword_ThrowArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _passwordHasher.HashPassword(null));

            Assert.That(ex.ParamName, Is.EqualTo(null));
        }

        [Test]
        public void ArgonPasswordHasher_VerifyPassword_ResultTrue()
        {
            string password = "Argon2TestHashed!!";

            var hash = _passwordHasher.HashPassword(password);
            bool result = _passwordHasher.VerifyPassword(password, hash);

            Assert.That(result, Is.True);
        }

        [Test]
        public void ArgonPasswordHasher_VerifyPassword_EmptyPassword_ThrowArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _passwordHasher.VerifyPassword("", "Argon2id:2:3:65536:someSalt:someHash"));

            Assert.That(ex.ParamName, Is.EqualTo(""));
        }

        [Test]
        public void ArgonPasswordHasher_VerifyPassword_NullPassword_ThrowArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _passwordHasher.VerifyPassword(null, "Argon2id:2:3:65536:someSalt:someHash"));

            Assert.That(ex.ParamName, Is.EqualTo(null));
        }

        [Test]
        public void ArgonPasswordHasher_VerifyPassword_EmptyHash_ThrowArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _passwordHasher.VerifyPassword("Argon2TestHashed!!", ""));

            Assert.That(ex.ParamName, Is.EqualTo(""));
        }

        [Test]
        public void ArgonPasswordHasher_VerifyPassword_NullHash_ThrowArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _passwordHasher.VerifyPassword("Argon2TestHashed!!", null));

            Assert.That(ex.ParamName, Is.EqualTo(null));
        }
    }
}
