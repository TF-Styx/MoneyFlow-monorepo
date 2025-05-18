using MoneyFlow.AuthenticationService.Domain.ValueObjects;

namespace Domain.Test
{
    internal class ValueObjectTests
    {
        #region Email

        /// <summary>
        /// Попытка создания ВЕРНОГО адреса электронной почты
        /// </summary>
        [Test]
        public void Email_TryCreate_WhenEmailCreate()
        {
            EmailAddress.TryCreate("alesha.popovich@mail.ru", out EmailAddress? emailAddress);

            TestContext.Out.WriteLine($"Создание верного почтового адреса: '{emailAddress}'");

            Assert.That(emailAddress, Is.Not.Null);
        }

        /// <summary>
        /// Попытка создания НЕВЕРНОГО адреса электронной почты
        /// </summary>
        [Test]
        public void Email_TryCreate_WhenEmailNotCreate()
        {
            string email_0_Test = " ";
            var email_0_result = EmailAddress.TryCreate(email_0_Test, out EmailAddress? emailAddress_0);

            string? email_1_Test = null;
            var email_1_result = EmailAddress.TryCreate(email_0_Test, out EmailAddress? emailAddress_1);

            string email_2_Test = "Alesha Popovich.mail";
            var email_2_result = EmailAddress.TryCreate(email_2_Test, out EmailAddress? emailAddress_2);

            TestContext.Out.WriteLine($"[SETUP INFO from Password.TryCreate_0]; Введен почтовый адрес: '{email_0_Test}';");
            if (!email_0_result.IsValid)
                for (int i = 0; i < email_0_result.ErrorList.Count; i++)
                    TestContext.Out.WriteLine($"[SETUP INFO from EmailAddress.TryCreate_0] {email_0_result.ErrorList[i]};\n");

            TestContext.Out.WriteLine($"[SETUP INFO from Password.TryCreate_1]; Введен почтовый адрес: '{email_1_Test}';");
            if (!email_1_result.IsValid)
                for (int i = 0; i < email_1_result.ErrorList.Count; i++)
                    TestContext.Out.WriteLine($"[SETUP INFO from EmailAddress.TryCreate_1] {email_1_result.ErrorList[i]};\n");

            TestContext.Out.WriteLine($"[SETUP INFO from Password.TryCreate_2]; Введен почтовый адрес: '{email_2_Test}';");
            if (!email_2_result.IsValid)
                for (int i = 0; i < email_2_result.ErrorList.Count; i++)
                    TestContext.Out.WriteLine($"[SETUP INFO from EmailAddress.TryCreate_2] {email_2_result.ErrorList[i]};\n");

            Assert.Multiple(() =>
            {
                Assert.That(emailAddress_0, Is.Null);
                Assert.That(emailAddress_2, Is.Null);
            });
        }

        #endregion

        #region Login

        /// <summary>
        /// Попытка создания ВЕРНОГО логина
        /// </summary>
        [Test]
        public void Login_TryCreate_WhenLoginCreate()
        {
            Login.TryCreate("AleshaPopovich", out Login? login);

            TestContext.Out.WriteLine($"Создание верного логина: '{login}'");

            Assert.That(login, Is.Not.Null);
        }

        /// <summary>
        /// Попытка создания НЕВЕРНОГО логина
        /// </summary>
        [Test]
        public void Login_TryCreate_WhenLoginNotCreate()
        {
            string login_0_Test = " ";
            var login_0_result = Login.TryCreate(login_0_Test, out Login? login_0);

            string login_1_Test = "Alesha.Popovich";
            var login_1_result = Login.TryCreate(login_1_Test, out Login? login_1);

            string login_2_Test = "Alesha Popovich";
            var login_2_result = Login.TryCreate(login_2_Test, out Login? login_2);

            string login_3_Test = "Алеша Попович";
            var login_3_result = Login.TryCreate(login_3_Test, out Login? login_3);

            TestContext.Out.WriteLine($"[SETUP INFO from Login.TryCreate_0]; Введен логин: '{login_0_Test}';");
            if (!login_0_result.IsValid)
                for (int i = 0; i < login_0_result.ErrorList.Count; i++)
                    TestContext.Out.WriteLine($"[SETUP INFO from Login.TryCreate_0] {login_0_result.ErrorList[i]};\n");

            TestContext.Out.WriteLine($"[SETUP INFO from Login.TryCreate_1]; Введен логин: '{login_1_Test}';");
            if (!login_1_result.IsValid)
                for (int i = 0; i < login_1_result.ErrorList.Count; i++)
                    TestContext.Out.WriteLine($"[SETUP INFO from Login.TryCreate_1] {login_1_result.ErrorList[i]};\n");

            TestContext.Out.WriteLine($"[SETUP INFO from Login.TryCreate_2]; Введен логин: '{login_2_Test}';");
            if (!login_2_result.IsValid)
                for (int i = 0; i < login_2_result.ErrorList.Count; i++)
                    TestContext.Out.WriteLine($"[SETUP INFO from Login.TryCreate_2] {login_2_result.ErrorList[i]};\n");

            TestContext.Out.WriteLine($"[SETUP INFO from Login.TryCreate_3]; Введен логин: '{login_3_Test}';");
            if (!login_3_result.IsValid)
                for (int i = 0; i < login_3_result.ErrorList.Count; i++)
                    TestContext.Out.WriteLine($"[SETUP INFO from Login.TryCreate_5] {login_3_result.ErrorList[i]};\n");

            Assert.Multiple(() =>
            {
                Assert.That(login_0, Is.Null);
                Assert.That(login_1, Is.Null);
                Assert.That(login_2, Is.Null);
                Assert.That(login_3, Is.Null);
            });
        }

        #endregion

        #region Password

        /// <summary>
        /// Попытка создания ВЕРНОГО пароля
        /// </summary>
        [Test]
        public void Password_TryCreate_WhenPasswordCreate()
        {
            Password.TryCreate("TESTpassword1!", out Password? password);

            TestContext.Out.WriteLine($"Создание верного пароля: '{password}'");

            Assert.That(password, Is.Not.Null);
        }

        /// <summary>
        /// Попытка создания НЕВЕРНОГО пароля
        /// </summary>
        [Test]
        public void Password_TryCreate_WhenPasswordNotCreate()
        {
            string password_0_Text = " ";
            var password_0_result = Password.TryCreate(password_0_Text, out Password? password_0);

            string? password_1_Text = null;
            var password_1_result = Password.TryCreate(password_1_Text, out Password? password_1);

            string password_2_Text = "alesha.popovich1";
            var password_2_result = Password.TryCreate(password_2_Text, out Password? password_2);

            string password_3_Text = "ALESHA.POPOVICH1";
            var password_3_result = Password.TryCreate(password_3_Text, out Password? password_3);

            string password_4_Text = "ALESHA.popovich";
            var password_4_result = Password.TryCreate(password_4_Text, out Password? password_4);

            string password_5_Text = "ALESHApopovich1";
            var password_5_result = Password.TryCreate(password_5_Text, out Password? password_5);

            string password_6_Text = "ALE.pop1";
            var password_6_result = Password.TryCreate(password_6_Text, out Password? password_6);


            TestContext.Out.WriteLine($"[SETUP INFO from Password.TryCreate_0]; Введен пароль: '{password_0_Text}';");
            if (!password_0_result.IsValid)
                for (int i = 0; i < password_0_result.ErrorList.Count; i++)
                    TestContext.Out.WriteLine($"[SETUP INFO from Password.TryCreate_0]; Ошибка: '{password_0_result.ErrorList[i]}';\n");

            TestContext.Out.WriteLine($"[SETUP INFO from Password.TryCreate_1]; Введен пароль: '{password_1_Text ?? "null"}';");
            if (!password_1_result.IsValid)
                for (int i = 0; i < password_1_result.ErrorList.Count; i++)
                    TestContext.Out.WriteLine($"[SETUP INFO from Password.TryCreate_1]; Ошибка: '{password_1_result.ErrorList[i]}';\n");

            TestContext.Out.WriteLine($"[SETUP INFO from Password.TryCreate_2]; Введен пароль: '{password_2_Text}';");
            if (!password_2_result.IsValid)
                for (int i = 0; i < password_2_result.ErrorList.Count; i++)
                    TestContext.Out.WriteLine($"[SETUP INFO from Password.TryCreate_2]; Ошибка: '{password_2_result.ErrorList[i]}';\n");

            TestContext.Out.WriteLine($"[SETUP INFO from Password.TryCreate_3]; Введен пароль: '{password_3_Text}';");
            if (!password_3_result.IsValid)
                for (int i = 0; i < password_3_result.ErrorList.Count; i++)
                    TestContext.Out.WriteLine($"[SETUP INFO from Password.TryCreate_3]; Ошибка: '{password_3_result.ErrorList[i]}';\n");

            TestContext.Out.WriteLine($"[SETUP INFO from Password.TryCreate_4]; Введен пароль: '{password_4_Text}';");
            if (!password_4_result.IsValid)
                for (int i = 0; i < password_4_result.ErrorList.Count; i++)
                    TestContext.Out.WriteLine($"[SETUP INFO from Password.TryCreate_4]; Ошибка: '{password_4_result.ErrorList[i]}';\n");

            TestContext.Out.WriteLine($"[SETUP INFO from Password.TryCreate_5]; Введен пароль: '{password_5_Text}';");
            if (!password_5_result.IsValid)
                for (int i = 0; i < password_5_result.ErrorList.Count; i++)
                    TestContext.Out.WriteLine($"[SETUP INFO from Password.TryCreate_5]; Ошибка: '{password_5_result.ErrorList[i]}';\n");

            TestContext.Out.WriteLine($"[SETUP INFO from Password.TryCreate_6]; Введен пароль: '{password_6_Text}';");
            if (!password_6_result.IsValid)
                for (int i = 0; i < password_6_result.ErrorList.Count; i++)
                    TestContext.Out.WriteLine($"[SETUP INFO from Password.TryCreate_6]; Ошибка: '{password_6_result.ErrorList[i]}';\n");

            Assert.Multiple(() =>
            {
                Assert.That(password_0, Is.Null);
                Assert.That(password_1, Is.Null);
                Assert.That(password_2, Is.Null);
                Assert.That(password_3, Is.Null);
                Assert.That(password_4, Is.Null);
                Assert.That(password_5, Is.Null);
                Assert.That(password_6, Is.Null);
            });
        }

        #endregion

        #region PhoneNumber

        /// <summary>
        /// Попытка создания ВЕРНОГО номера телефона
        /// </summary>
        [Test]
        public void PhoneNumber_TryCreate_WhenPhoneNumberCreate()
        {
            string phoneNumber_0_Test = "+71234567890";
            PhoneNumber.TryCreate(phoneNumber_0_Test, out PhoneNumber? phoneNumber_0);
            TestContext.Out.WriteLine($"[SETUP INFO from PhoneNumber.TryCreate_0]; Введен номер телефона: '{phoneNumber_0_Test}';");
            TestContext.Out.WriteLine($"Создание верного номера телефона: {phoneNumber_0};\n");

            string phoneNumber_1_Test = "+7(123) 456-78-90";
            PhoneNumber.TryCreate(phoneNumber_1_Test, out PhoneNumber? phoneNumber_1);
            TestContext.Out.WriteLine($"[SETUP INFO from PhoneNumber.TryCreate_1]; Введен номер телефона: '{phoneNumber_1_Test}';");
            TestContext.Out.WriteLine($"Создание верного номера телефона: {phoneNumber_1};\n");

            string phoneNumber_2_Test = "+7(123) сто % 456-78-90";
            PhoneNumber.TryCreate(phoneNumber_2_Test, out PhoneNumber? phoneNumber_2);
            TestContext.Out.WriteLine($"[SETUP INFO from PhoneNumber.TryCreate_2]; Введен номер телефона: '{phoneNumber_2_Test}';");
            TestContext.Out.WriteLine($"Создание верного номера телефона: {phoneNumber_2};\n");

            Assert.Multiple(() =>
            {
                Assert.That(phoneNumber_0, Is.Not.Null);
                Assert.That(phoneNumber_1, Is.Not.Null);
                Assert.That(phoneNumber_2, Is.Not.Null);
            });
        }

        [Test]
        public void PhoneNumber_TryCreate_WhenPhoneNumberNotCreate()
        {
            string phoneNumber_0_Test = " ";
            var phoneNumber_0_result = PhoneNumber.TryCreate(phoneNumber_0_Test, out PhoneNumber? phoneNumber_0);

            string? phoneNumber_1_Test = null;
            var phoneNumber_1_result = PhoneNumber.TryCreate(phoneNumber_1_Test, out PhoneNumber? phoneNumber_1);

            string phoneNumber_2_Test = "123456";
            var phoneNumber_2_result = PhoneNumber.TryCreate(phoneNumber_2_Test, out PhoneNumber? phoneNumber_2);

            string phoneNumber_3_Test = "123456789123456789";
            var phoneNumber_3_result = PhoneNumber.TryCreate(phoneNumber_3_Test, out PhoneNumber? phoneNumber_3);

            TestContext.Out.WriteLine($"[SETUP INFO from PhoneNumber.TryCreate_0]; Введен номер телефона: '{phoneNumber_0_Test}';");
            if (!phoneNumber_0_result.IsValid)
                for (int i = 0; i < phoneNumber_0_result.ErrorList.Count; i++)
                    TestContext.Out.WriteLine($"[SETUP INFO from PhoneNumber.TryCreate_0]; Ошибка: '{phoneNumber_0_result.ErrorList[i]}';\n");

            TestContext.Out.WriteLine($"[SETUP INFO from PhoneNumber.TryCreate_1]; Введен номер телефона: '{phoneNumber_1_Test}';");
            if (!phoneNumber_1_result.IsValid)
                for (int i = 0; i < phoneNumber_1_result.ErrorList.Count; i++)
                    TestContext.Out.WriteLine($"[SETUP INFO from PhoneNumber.TryCreate_1]; Ошибка: '{phoneNumber_1_result.ErrorList[i]}';\n");

            TestContext.Out.WriteLine($"[SETUP INFO from PhoneNumber.TryCreate_2]; Введен номер телефона: '{phoneNumber_2_Test}';");
            if (!phoneNumber_2_result.IsValid)
                for (int i = 0; i < phoneNumber_2_result.ErrorList.Count; i++)
                    TestContext.Out.WriteLine($"[SETUP INFO from PhoneNumber.TryCreate_2]; Ошибка: '{phoneNumber_2_result.ErrorList[i]}';\n");

            TestContext.Out.WriteLine($"[SETUP INFO from PhoneNumber.TryCreate_3]; Введен номер телефона: '{phoneNumber_3_Test}';");
            if (!phoneNumber_3_result.IsValid)
                for (int i = 0; i < phoneNumber_3_result.ErrorList.Count; i++)
                    TestContext.Out.WriteLine($"[SETUP INFO from PhoneNumber.TryCreate_3]; Ошибка: '{phoneNumber_3_result.ErrorList[i]}';\n");

            Assert.Multiple(() =>
            {
                Assert.That(phoneNumber_0, Is.Null);
                Assert.That(phoneNumber_1, Is.Null);
                Assert.That(phoneNumber_2, Is.Null);
                Assert.That(phoneNumber_3, Is.Null);
            });
        }

        #endregion
    }
}
