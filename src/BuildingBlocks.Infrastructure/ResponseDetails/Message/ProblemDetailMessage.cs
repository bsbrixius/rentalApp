using System;
using System.Text;

namespace BuildingBlocks.Infrastructure.ResponseDetails.Message
{
    public static class ProblemDetailMessage
    {
        public class SingleProblemDetailMessage : ISingleEntityMessage
        {
            public StringBuilder Message { get; set; }
        }

        public class ManyProblemDetailMessage : IManyEntityMessage
        {
            public StringBuilder Message { get; set; }
        }

        public static IManyEntityMessage Many(Type entityType = null)
        {
            var problemDetailMessage = new ManyProblemDetailMessage()
            {
                Message = new StringBuilder("The entities ")
            };
            if (entityType != null) problemDetailMessage.Message.Append($"of type [{nameof(entityType)}] ");
            return problemDetailMessage;

        }

        public static ISingleEntityMessage Single(Type entityType = null)
        {
            var problemDetailMessage = new SingleProblemDetailMessage()
            {
                Message = new StringBuilder("The entity ")
            };
            if (entityType != null) problemDetailMessage.Message.Append($"of type [{nameof(entityType)}] ");
            return problemDetailMessage;
        }
        public static string NotFound(this IProblemDetailMessage problemDetailMessage)
        {
            problemDetailMessage.Message.Append("could not be found.");
            return problemDetailMessage.Message.ToString();
        }

        public static ISingleEntityMessage With(this ISingleEntityMessage problemDetailMessage, string fieldName, object fieldValue)
        {
            problemDetailMessage.Message.Append($"with the ${fieldName}: {fieldValue} ");
            return problemDetailMessage;

        }
        public static ISingleEntityMessage WithId(this ISingleEntityMessage problemDetailMessage, int id)
        {
            problemDetailMessage.Message.Append($"with the Id: {id} ");
            return problemDetailMessage;
        }

        public static ISingleEntityMessage WithId(this ISingleEntityMessage problemDetailMessage, Guid id)
        {
            problemDetailMessage.Message.Append($"with the Id: {id} ");
            return problemDetailMessage;
        }
    }
}
