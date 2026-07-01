namespace Identity.Domain.Common;

#region "Old code"
//public abstract class AuditableEntity : Entity
//{
//    public DateTime CreatedAt { get; protected set; }

//    public string CreatedBy { get; protected set; } = string.Empty;

//    public DateTime? UpdatedAt { get; protected set; }

//    public string? UpdatedBy { get; protected set; }
//}
#endregion "Old code"

public abstract class AuditableEntity : Entity
{
    /// <summary>
    /// Thời gian tạo bản ghi (UTC)
    /// </summary>
    public DateTime CreatedAt { get; protected set; }

    /// <summary>
    /// User tạo bản ghi
    /// </summary>
    public Guid? CreatedBy { get; protected set; }

    /// <summary>
    /// Thời gian cập nhật gần nhất (UTC)
    /// </summary>
    public DateTime? UpdatedAt { get; protected set; }

    /// <summary>
    /// User cập nhật gần nhất
    /// </summary>
    public Guid? UpdatedBy { get; protected set; }

    /// <summary>
    /// Soft Delete
    /// </summary>
    public bool IsDeleted { get; protected set; }

    /// <summary>
    /// Thời điểm xóa
    /// </summary>
    public DateTime? DeletedAt { get; protected set; }

    /// <summary>
    /// User thực hiện xóa
    /// </summary>
    public Guid? DeletedBy { get; protected set; }

    /// <summary>
    /// Optimistic Concurrency
    /// </summary>
    public byte[] RowVersion { get; protected set; } = Array.Empty<byte>();

    #region Methods

    public virtual void SetCreated(Guid? userId)
    {
        CreatedAt = DateTime.UtcNow;
        CreatedBy = userId;
    }

    public virtual void SetUpdated(Guid? userId)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = userId;
    }

    public virtual void SoftDelete(Guid? userId)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = userId;
    }

    public virtual void Restore()
    {
        IsDeleted = false;
        DeletedAt = null;
        DeletedBy = null;
    }

    #endregion
}