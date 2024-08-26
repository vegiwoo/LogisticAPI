namespace LogisticsAPI.Models
{
    public enum MessengerType 
    {
        Telegram
    }

    public enum Marketplace { WB, OZ }

     public enum SKURansomsStatus
    {
        Delivered,  // Доставлен на ПВЗ
        WithDrawn,  // Забран с ПВЗ
        Canceled,   // Отменен 
        OnWay       // В пути
    }

    public enum ParsingTypeCode
    {
        IntTypeCode,                // 0
        DoubleTypeCode,             // 1
        DataOnlyTypeCode,           // 2
        StringTypeCode,             // 3
        SKURansomsStatus,           // 4
        Marketplace,                // 5
        SellmentStatus              // 6
    }

    enum SourceCellsType 
    {
        Required,
        Filtration,
        NonFiltration
    }
}