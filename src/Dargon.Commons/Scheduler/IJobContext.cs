namespace Dargon.Commons.AsyncPrimitives {
   public interface IJobContext<TJobRequest, TJobResponse> {
      TJobRequest Request { get; }
      void SetResponse(TJobResponse response);
   }
}