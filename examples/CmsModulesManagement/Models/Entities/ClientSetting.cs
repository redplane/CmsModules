using System;

namespace MailWeb.Models.Entities
{
    public class ClientSetting
    {
        #region Constructor

        public ClientSetting(Guid id)
        {
            Id = id;
        }

        #endregion

        #region Properties

        public Guid Id { get; }

        public string Name { get; set; }

        public string ActiveMailClient { get; set; }

        #endregion
    }
}