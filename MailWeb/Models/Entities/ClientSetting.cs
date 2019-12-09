using System;
using MailWeb.Models.ValueObjects;

namespace MailWeb.Models.Entities
{
    public class ClientSetting
    {
        #region Properties

        public Guid Id { get; }

        public string Name { get; set; }

        public MailServiceValueObject ActiveMailService { get; set; }

        #endregion

        #region Constructor

        public ClientSetting(Guid id)
        {
            Id = id;
        }

        #endregion
    }
}