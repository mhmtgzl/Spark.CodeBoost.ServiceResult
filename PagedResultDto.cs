namespace Spark.CodeBoost.ServiceResult;

public class PagedResultDto<T>
{

    /// <summary>
    /// Kayıtlar
    /// </summary>
    public IEnumerable<T> Items { get; set; }

    /// <summary>
    /// Tablodaki tüm kayıtların sayısı
    /// </summary>
    public long TotalCount { get; set; }

    /// <summary>
    /// Her sayfa için dönülmesi istenen maksimum kayıt sayısı
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Aktif sayfa
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Toplam sayfa sayısı
    /// </summary>
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    /// <summary>
    /// Dönen kayıt sayısı
    /// </summary>
    public long ItemsCount => Items.Count();
}