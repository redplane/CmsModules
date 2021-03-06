﻿using System;
using System.Linq;
using System.Reflection;
using CmsModules.Shares.Constants;
using CmsModules.Shares.Models.MailHosts;
using MailModule.Models.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CmsModules.ManagementUi.Converters
{
    public class MailHostConverter : JsonConverter
    {
        #region Methods

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var mailHost = value as IMailHost;
            var mailHostType = ResolveMailHostType(mailHost?.Type);

            if (mailHostType == null)
                return;

            serializer.Serialize(writer, mailHost, mailHostType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var jToken = JToken.ReadFrom(reader);
            var mailHostTypeToken = jToken.SelectToken(nameof(IMailHost.Type).ToLower()) ?? jToken.SelectToken(nameof(IMailHost.Type));
            if (mailHostTypeToken == null)
                return default;

            var mailHostTypeName = mailHostTypeToken.Value<string>();
            var mailHostType = ResolveMailHostType(mailHostTypeName);

            if (mailHostType == null)
                return default;

            return ToMailHost(jToken, mailHostType);
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        protected virtual IMailHost ToMailHost(JToken jToken, Type type)
        {
            var mi = typeof(JToken)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(m => m.Name == "ToObject" && m.GetParameters().Length == 0 && m.IsGenericMethod)
                ?.MakeGenericMethod(type);

            return (IMailHost) mi?.Invoke(jToken, null);
        }

        protected virtual Type ResolveMailHostType(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                return null;

            switch (type)
            {
                case MailHostKindConstants.Smtp:
                    return typeof(SmtpHost);

                case MailHostKindConstants.MailGun:
                    return typeof(MailGunHost);
            }

            return null;
        }

        #endregion
    }
}