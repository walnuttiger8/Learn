//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Learn
{
    using System;
    using System.Collections.Generic;
    
    public partial class ServicePhotos
    {
        public int Id { get; set; }
        public Nullable<int> ServiceId { get; set; }
        public Nullable<int> PhotoId { get; set; }
    
        public virtual Photos Photos { get; set; }
        public virtual Services Services { get; set; }
    }
}
