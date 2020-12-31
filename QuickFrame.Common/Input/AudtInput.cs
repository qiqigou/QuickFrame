namespace QuickFrame.Common
{
    /// <summary>
    /// 审核审批输入模型
    /// </summary>
    public class AudtInput<TKey> : WithStampDataInput
        where TKey : notnull
    {
        [StringColumn]
        public TKey KeyValue { get; set; } = default!;
    }
}
