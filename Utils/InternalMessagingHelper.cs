using NAFCore.Common.Types.Initialization;
using NAFCore.Common.Types.Patterns;
using NAFCore.Common.Utils.Diagnostics.Logger;
using NAFCore.Common.Utils.Extensions;
using NAFCore.Common.Utils.Serialization;
using NAFCore.InternalMessaging.Client;
using NAFCore.InternalMessaging.Client.DTOs;
using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogoPaasSampleApp.Utils
{
    /// <summary>
    /// Internal Messaging Utility
    /// </summary>
    public class InternalMessagingHelper : NSingleton<InternalMessagingHelper>
    {
        /// <summary>
        /// The topic for sample message
        /// </summary>
        public const string TOPIC_SAMPLE_MESSAGE = "samplemessagetopic";

        /// <summary>
        /// Gets the own topics information.
        /// </summary>
        /// <returns>List&lt;InternalMessageTopic&gt;.</returns>
        internal List<InternalMessageTopic> GetOwnTopicsInfo()
        {
            var generator = new NJsonSchema.Generation.JsonSchemaGenerator(new NJsonSchema.Generation.JsonSchemaGeneratorSettings() { DefaultReferenceTypeNullHandling = NJsonSchema.ReferenceTypeNullHandling.NotNull, FlattenInheritanceHierarchy = true });
            JsonSchema4 sampleDtoSchema = null;

            Task t = Task.Run(() =>
            {
                sampleDtoSchema = (JsonSchema4.FromTypeAsync<SampleInternalMessageDTO>()).GetAwaiter().GetResult();
            });
            t.Wait();

            return new List<InternalMessageTopic>()
            {
                new InternalMessageTopic(NAFInitializationInfo.Current.AppSecurityID, "Sample message topic from paas sample app", TOPIC_SAMPLE_MESSAGE, sampleDtoSchema),
            };
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="targetTopic">The target topic.</param>
        /// <param name="syncThrowEx">if set to <c>true</c> [synchronize throw ex].</param>
        /// <exception cref="Exception">Message sending operation to Kafka has been timed out.</exception>
        public void SendMessage(IInternalMessageDTO message, string targetTopic, bool syncThrowEx = false)
        {
            if (!InternalMessagingClientManager.Instance().IsEnabled())
                return;

            if (syncThrowEx)
            {
                Exception e = null;
                bool success = false;
                Task t = Task.Run(() =>
                {
                    try
                    {
                        string messageContent = NSerializer.JSONSimple.Serialize(message);
                        InternalMessagingClientManager.Instance().SendMessage(messageContent, targetTopic).GetAwaiter().GetResult();
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        NLogger.Instance().Error("InternalMessagingHelper SendMessage Error", ex);
                        e = ex;
                    }
                });
                t.Wait(5000);

                if (e.Assigned())
                    throw e;
                else if (success == false)
                    throw new Exception("Message sending operation to Kafka has been timed out.");

            }
            else
            {
                Task t = Task.Run(async () =>
                {
                    try
                    {
                        string messageContent = NSerializer.JSONSimple.Serialize(message);

                        await InternalMessagingClientManager.Instance().SendMessage(messageContent, targetTopic);
                    }
                    catch (Exception ex)
                    {
                        NLogger.Instance().Error("InternalMessagingHelper SendMessage Error", ex);
                    }
                });
                t.Wait(5000);
            }
        }
    }

    /// <summary>
    /// Interface IInternalMessageDTO
    /// </summary>
    public interface IInternalMessageDTO
    {
    }
}
