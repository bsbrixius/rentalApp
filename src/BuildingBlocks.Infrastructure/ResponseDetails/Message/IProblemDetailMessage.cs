using System.Text;

namespace BuildingBlocks.Infrastructure.ResponseDetails.Message
{
    public interface IProblemDetailMessage
    {
        public abstract StringBuilder Message { get; set; }
    }
}
