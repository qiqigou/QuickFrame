namespace QuickFrame.Common
{
    /// <summary>
    /// 唯一识别码接口
    /// </summary>
    public interface IIdWorker
    {
        long GetId64();

        string GetIdString();
    }
}
