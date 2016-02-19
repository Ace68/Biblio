using NUnit.Framework;

namespace Biblio.Configuration.Test
{
    [TestFixture]
    public class EventStoreConfigurationTest
    {
        [Test]
        public void GetEventStoreUri()
        {
            Assert.AreEqual("127.0.0.1", BiblioConfiguration.EventStoreSection.Uri);
        }

        [Test]
        public void GetEventStorePort()
        {
            Assert.AreEqual(2113, BiblioConfiguration.EventStoreSection.Port);
        }

        [Test]
        public void GetEventStoreUser()
        {
            Assert.AreEqual("admin", BiblioConfiguration.EventStoreSection.User);
        }

        [Test]
        public void GetEventStorePassword()
        {
            Assert.AreEqual("changeit", BiblioConfiguration.EventStoreSection.Password);
        }
    }
}
