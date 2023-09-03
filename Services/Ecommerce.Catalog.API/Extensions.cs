using Ecommerce.Catalog.API.Models;

namespace Catalog.API
{
    public static class Extensions
    {
        public static CatalogItem asItem (this CatalogItemInsertDto catalogItemDto)
        {
            return new CatalogItem()
            {
                CatalogDateOfCreation = DateTime.Now,
                CatalogDescription = catalogItemDto.CatalogDescription,
                CatalogMrp = catalogItemDto.CatalogMrp,
                CatalogName = catalogItemDto.CatalogName,
                CatalogType = catalogItemDto.CatalogType,
                DiscountedPrice = catalogItemDto.DiscountedPrice == null ? catalogItemDto.CatalogMrp : double.Parse(catalogItemDto.DiscountedPrice),
                CatalogImages = catalogItemDto.CatalogImages,
                CatalogId = Guid.NewGuid(),
            };
        }

        public static CatalogItem asItem(this CatalogItemUpdateDto catalogItemUpdateDto, CatalogItem oldItem)
        {
            return new CatalogItem()
            {
                CatalogDescription = catalogItemUpdateDto.CatalogDescription ?? oldItem.CatalogDescription,
                CatalogMrp = catalogItemUpdateDto.CatalogMrp == null ? oldItem.CatalogMrp : double.Parse(catalogItemUpdateDto.CatalogMrp),
                CatalogName = catalogItemUpdateDto.CatalogName ?? oldItem.CatalogName,
                CatalogType = catalogItemUpdateDto.CatalogType ?? oldItem.CatalogType,
                DiscountedPrice = catalogItemUpdateDto.DiscountedPrice == null ? oldItem.CatalogMrp : double.Parse(catalogItemUpdateDto.DiscountedPrice),
                CatalogImages = catalogItemUpdateDto.CatalogImages ?? oldItem.CatalogImages,
                CatalogId = oldItem.CatalogId,
                CatalogDateOfCreation = oldItem.CatalogDateOfCreation
            };
        }

        public static CatalogItemInsertDto asDto(this CatalogItem catalogItem)
        {
            return new CatalogItemInsertDto()
            {
                CatalogDescription = catalogItem.CatalogDescription,
                CatalogMrp = catalogItem.CatalogMrp,
                CatalogName = catalogItem.CatalogName,
                CatalogType = catalogItem.CatalogType,
                DiscountedPrice =catalogItem.DiscountedPrice.ToString(),
                CatalogImages = catalogItem.CatalogImages,
                CatalogId = catalogItem.CatalogId
            };
        }
    }
}
