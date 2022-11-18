using System.Net.Http.Headers;
using heitech.ShopwareIntegration.State.Integration;
using heitech.ShopwareIntegration.State.Integration.Models;
using heitech.ShopwareIntegration.State.Integration.Models.Data;
using heitech.ShopwareIntegration.State.Interfaces;

namespace heitech.ShopwareIntegration.State.DetailModels.Media;

/// <summary>
/// See for more information: https://shopware.stoplight.io/docs/admin-api/ZG9jOjEyNjI1Mzkw-media-handling
/// This is the Tree entry for the Media Relation of Product, Media and Product_media
/// </summary>
public static class MediaBase
{
    public sealed record MediaUploadContext(MediaId MediaId, string ProductId, FileInfo File);

    public readonly struct MediaId
    {
        /// <summary>
        /// The string value of the Id ("N" formatted Guid in .NET)
        /// </summary>
        public string Value { get; }
        /// <summary>
        /// Creates a new MediaId with the appropriate Format 
        /// </summary>
        public MediaId()
            => Value = BaseEntity.CreateId;

        private MediaId(string value)
            => Value = value;

        /// <summary>
        /// If you already got an existing MediaId, verify it with this Validation Method.
        /// </summary>
        /// <param name="existingId"></param>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        public static bool TryCreateFrom(string existingId, out MediaId mediaId)
        {
            mediaId = default;
            var couldBeParsed = Guid.TryParse(existingId, out var _);
            if (couldBeParsed)
                mediaId = new MediaId(existingId);

            return couldBeParsed;
        }
    }
    
    // Media handling:
    /*
     step 1 - push media with patch on /product/id with body { "media" : [ { "id" : "product_media(BaseEntity).CreateId", "media" : { "id" : "media(BaseEntity).CreateId" } } ] }
     where the 2nd media entity represents the actual Media entity in the database, the first refers to the join entity called product_media.
      
     step 2 - url /api/_action/media/cfbd5018d38d41d8adca10d94fc8bdf0/upload?extension=jpg
     with header: content-type image/$extension from route
     and use a binary file body (byteArrayContent e.g.)
     */

    /// <summary>
    /// Inserts Media file in a two step process:
    /// <para>1.) The product entity is Patched which creates a Product - Product_Media - Meda Relationship</para>
    /// <para>2.) Uploading the File as ByteStream to the actual Media object itself</para>
    /// <para>Be aware that the second step can fail for different reasons, but as this is not a transactional process, you then only need to specify the MediaId and the file to the UploadMediaFile Method</para>
    /// </summary>
    /// <param name="stateManager"></param>
    /// <param name="productId"></param>
    /// <param name="file"></param>
    public static async Task<MediaId> InsertProductMediaFileForProductAsync(this IStateManager stateManager, string productId, FileInfo file)
    {
        var mediaId = new MediaId();
        try
        {
            var mediaContext = new MediaUploadContext(mediaId, productId, file);
            await stateManager.InsertProductMediaFile(mediaContext);
            await stateManager.UploadMediaFileToRelationAsync(mediaContext);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return mediaId;
        }

        return mediaId;
    }

    private static async Task InsertProductMediaFile(this IStateManager stateManager, MediaUploadContext uploadContext)
    {
        // get the details of product, so that the Patch becomes available
        // we don´t need all the properties, but the interface is what it is here :D 
        var productResult = await stateManager.GetDetail<ProductDetails>(uploadContext.ProductId, 1);
        if (productResult.IsSuccess is false)
            return;

        // step 1
        await stateManager.InsertProductMediaWithEmptyMediaFile(productResult.Model, uploadContext.MediaId);

        // step 2
        await stateManager.UploadMediaFileToRelationAsync(uploadContext);
    }

    private static async Task InsertProductMediaWithEmptyMediaFile(this IStateManager stateManager, ProductDetails productDetails,  MediaBase.MediaId mediaId)
    {
        var productMediaId = BaseEntity.CreateId;
        var productPatch = PatchedValue.From(
            productDetails,
            new
            {
                media = new object[] {
                    new
                    {
                        id = productMediaId, 
                        media = new
                        {
                            id = mediaId.Value
                        }
                    }
                }
            }
        );
        var patchContext = DataContext.Update(productPatch, 1);
        _ = await stateManager.UpdateAsync<ProductDetails>(patchContext);
    }

    /// <summary>
    /// Use this Method only directly if you know about the MediaId and the attempt to create a relation failed! Else this is called only internally 
    /// </summary>
    /// <param name="stateManager"></param>
    /// <param name="uploadContext"></param>
    public static async Task UploadMediaFileToRelationAsync(this IStateManager stateManager, MediaUploadContext uploadContext)
    {
        var jpg = uploadContext.File.Extension[1..];
        // cast is not ideal, but for now and internal use it is kinda ok
        var url = $"_action/media/{uploadContext.MediaId.Value}/upload?extension={jpg}";
        if (stateManager is StateManager internalManager)
        {
            var client = internalManager.ShopwareClient;
            using var fileContent = new ByteArrayContent(await File.ReadAllBytesAsync(uploadContext.File.FullName));
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse($"image/{jpg}");

            var httpRequestMessage = client.CreateHttpRequest(url, HttpMethod.Post, fileContent);
            _ = await client.SendAsync<DataEmpty>(httpRequestMessage);
        }
    }
}