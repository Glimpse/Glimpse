namespace Glimpse.Core.Validator
{
    public enum LifecycleEvent
    {
        BeginRequest,
        EndRequest,
        Handler,
        PostRequestHandlerExecute,
        PreSendRequestHeaders,
        PostReleaseRequestState
    }
}