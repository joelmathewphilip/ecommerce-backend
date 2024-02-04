namespace Ecommerce.Shared
{
    public static class Constants
    {
       
        //AccountAPI API Constants
        public static string MongoDbUserDatabaseName = "ecommerce";
        public static string MongoDbUserCollectionName = "user";
        public static string AccountIdentityAudienceSettingName = "JWTAuth:Audience";
        public static string AccountIdentityIssuerSettingName = "JWTAuth:Issuer";
        public static string AccountIdentitySigningKeySettingName = "JWTAuth:Key";

        //Identity API Constants
        public static string MongoDbUserIdentityDatabaseName = "ecommerce";
        public static string MongoDbUserIdentityCollectionName = "authentication";
        public static string IdentityIssuerSettingName = "JWTAuth:Issuer";
        public static string IdentitySecretKeySettingName = "JWTAuth:Key";

        //Catalog API Constants
        public static string CatalogIdentityAudienceSettingName = "JWTAuth:Audience";
        public static string CatalogIdentityIssuerSettingName = "JWTAuth:Issuer";
        public static string CatalogIdentitySigningKeySettingName = "JWTAuth:Key";
        public static string MongoDbCatalogDatabaseName = "ecommerce";
        public static string MongoDbCatalogCollectionName = "catalog";

        public static string OrderingIdentityAudienceSettingName = "JWTAuth:Audience";
        public static string OrderingIdentityIssuerSettingName = "JWTAuth:Issuer";
        public static string OrderingIdentitySigningKeySettingName = "JWTAuth:Key";

        public static string CartServiceQueueName = "cart-create-queue-name";


    }
}
