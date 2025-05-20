using MoneyFlow.AuthenticationService.Domain.DomainModels;
using MoneyFlow.AuthenticationService.Domain.ValueObjects;

namespace Domain.Test
{
    internal class DomainModelTest
    {
        [Test]
        public void UserDomain_Create_WhenUserCreated()
        {
            var loginResult = Login.TryCreate("Styx", out Login? login);
            var emailResult = EmailAddress.TryCreate("alesh.popovich@mail.ru", out EmailAddress? emailAddress);
            var phoneResult = PhoneNumber.TryCreate("+71234567890", out PhoneNumber? phoneNumber);

            if (!loginResult.IsValid)
                for (int i = 0; i < loginResult.ErrorList.Count; i++)
                    TestContext.Out.WriteLine($"[SETUP INFO from Login.TryCreate] {loginResult.ErrorList[i]}");

            if (!emailResult.IsValid)
                for (int i = 0; i < emailResult.ErrorList.Count; i++)
                    TestContext.Out.WriteLine($"[SETUP INFO from EmailAddress.TryCreate] {emailResult.ErrorList[i]}");

            if (!phoneResult.IsValid)
                for (int i = 0; i < phoneResult.ErrorList.Count; i++)
                    TestContext.Out.WriteLine($"[SETUP INFO from PhoneNumber.TryCreate] {phoneResult.ErrorList[i]}");


            var (Domain, Message) = UserDomain.Create(login, "Гриша", "wrigsrgsrf", emailAddress, phoneNumber, 1, 1);

            if (Domain == null)
                TestContext.Out.WriteLine($"[SETUP INFO from UserDomain.Create] {Message}");

            Assert.That(Domain, Is.Not.Null);
        }

        [Test]
        public void UserDomain_Create_WhenUserNotCreated()
        {
            var loginResult = Login.TryCreate("Styx/.><", out Login? login);
            var emailResult = EmailAddress.TryCreate("alesh.popovich", out EmailAddress? emailAddress);
            var phoneResult = PhoneNumber.TryCreate("+7123453453454567890", out PhoneNumber? phoneNumber);

            if (!loginResult.IsValid)
                for (int i = 0; i < loginResult.ErrorList.Count; i++)
                    TestContext.Out.WriteLine($"[SETUP INFO from Login.TryCreate] {loginResult.ErrorList[i]}");

            if (!emailResult.IsValid)
                for (int i = 0; i < emailResult.ErrorList.Count; i++)
                    TestContext.Out.WriteLine($"[SETUP INFO from EmailAddress.TryCreate] {emailResult.ErrorList[i]}");

            if (!phoneResult.IsValid)
                for (int i = 0; i < phoneResult.ErrorList.Count; i++)
                    TestContext.Out.WriteLine($"[SETUP INFO from PhoneNumber.TryCreate] {phoneResult.ErrorList[i]}");


            var (Domain, Message) = UserDomain.Create(login, "", "", emailAddress, phoneNumber, 1, 1);

            if (Domain == null)
                TestContext.Out.WriteLine($"[SETUP INFO from UserDomain.Create] {Message}");

            Assert.That(Domain, Is.Null);
        }


    }
}
