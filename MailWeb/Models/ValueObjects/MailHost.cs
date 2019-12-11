using System;
using System.Reflection;
using MailWeb.Models.Interfaces;

namespace MailWeb.Models.ValueObjects
{
    public class MailHost : IMailHost
    {
        public string Type => GetType().FullName;
    }
}