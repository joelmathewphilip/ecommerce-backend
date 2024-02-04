using Ecommerce.Catalog.API.Models;

namespace Catalog.API
{
    public static class Extensions
    {
        public static CatalogItem toItem (this CatalogItemInsertRequestDto catalogItemDto)
        {
            return new CatalogItem()
            {
                CatalogDateOfCreation = DateTime.Now,
                CatalogDescription = catalogItemDto.CatalogDescription,
                CatalogMrp = catalogItemDto.CatalogMrp,
                CatalogName = catalogItemDto.CatalogName,
                CatalogType = catalogItemDto.CatalogType,
                CatalogImages = catalogItemDto.CatalogImages,
                CatalogQuantity = catalogItemDto.CatalogQuantity,
                CatalogId = Guid.NewGuid(),

            };
        }

        public static CatalogItem toItem(this CatalogItemUpdateRequestDto catalogItemUpdateDto, CatalogItem oldItem)
        {
            return new CatalogItem()
            {
                CatalogDescription = catalogItemUpdateDto.CatalogDescription ?? oldItem.CatalogDescription,
                CatalogMrp =  catalogItemUpdateDto.CatalogMrp,
                CatalogName = catalogItemUpdateDto.CatalogName ?? oldItem.CatalogName,
                CatalogType = catalogItemUpdateDto.CatalogType ?? oldItem.CatalogType,
                CatalogImages = catalogItemUpdateDto.CatalogImages ?? oldItem.CatalogImages,
                CatalogId = oldItem.CatalogId,
                CatalogQuantity = catalogItemUpdateDto.CatalogQuantity,
                CatalogDateOfCreation = oldItem.CatalogDateOfCreation
            };
        }

        public static CatalogItemResponseDto toDto(this CatalogItem catalogItem)
        {
            return new CatalogItemResponseDto()
            {
                CatalogDescription = catalogItem.CatalogDescription,
                CatalogDateOfCreation = catalogItem.CatalogDateOfCreation,
                CatalogMrp = catalogItem.CatalogMrp,
                CatalogName = catalogItem.CatalogName,
                CatalogType = catalogItem.CatalogType,
                DiscountedPrice = catalogItem.DiscountedPrice,
                CatalogImages = catalogItem.CatalogImages,
                CatalogId = catalogItem.CatalogId,
                CatalogQuantity = catalogItem.CatalogQuantity
            };
        }

        /*public static CatalogItemUpdateDto toUpdateDto(this CatalogItem catalogItem)
        {
            return new CatalogItemUpdateDto()
            {
                CatalogDescription = catalogItem.CatalogDescription,
                CatalogMrp = catalogItem.CatalogMrp,
                CatalogName = catalogItem.CatalogName,
                CatalogType = catalogItem.CatalogType,
                CatalogDiscountedPrice = catalogItem.DiscountedPrice,
                CatalogImages = catalogItem.CatalogImages,
                CatalogId = catalogItem.CatalogId
            };
        }*/
    }
}
