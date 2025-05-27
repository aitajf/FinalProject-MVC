namespace MVC_FinalProject.Models.BlogPost
{
    public class BlogPostEdit
    {
            public int Id { get; set; } // ✅ BlogPost ID
            public string Title { get; set; }
            public string Description { get; set; }
            public string HighlightText { get; set; }
            public int BlogCategoryId { get; set; }
            public List<IFormFile> Images { get; set; } // ✅ Yeni yüklənəcək şəkillər
            public List<BlogPostImg> ExistingImages { get; set; } // ✅ Mövcud şəkillər siyahısı
            public List<int> DeleteImageIds { get; set; } // ✅ Silinəcək şəkillərin ID-ləri
            public string MainImageUrl { get; set; } // ✅ İstifadəçi əsas şəkili seçə bilər
     

    }
}
