using System.Configuration;

namespace Biblio.Configuration
{
    public class EventStoreSectionHandler : ConfigurationSection
    {
        [ConfigurationProperty("uri", DefaultValue = "127.0.0.1", IsRequired = true)]
        public string Uri
        {
            get { return (string)this["uri"]; }
            set { this["uri"] = value; }
        }

        [ConfigurationProperty("port", DefaultValue = 1113, IsRequired = true)]
        public int Port
        {
            get { return (int)this["port"]; }
            set { this["port"] = value; }
        }

        [ConfigurationProperty("user", DefaultValue = "admin", IsRequired = true)]
        public string User
        {
            get { return (string)this["user"]; }
            set { this["user"] = value; }
        }

        [ConfigurationProperty("password", DefaultValue = "changeit", IsRequired = true)]
        public string Password
        {
            get { return (string)this["password"]; }
            set { this["password"] = value; }
        }
    }
}
