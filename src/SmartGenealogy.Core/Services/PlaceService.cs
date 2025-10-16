namespace SmartGenealogy.Core.Services;

/// <summary>
/// Place service.
/// </summary>
public class PlaceService(
    PlaceRepository placeRepository,
    MediaLinkRepository mediaLinkRepository,
    IAlertService alertService)
    : IPlaceService
{
    /// <summary>
    /// Retrieves a list of master places from the database.
    /// </summary>
    /// <returns>A list of <see cref="Place"/> objects.</returns>
    public async Task<List<Place>?> ListMasterPlacesAsync()
    {
        var placeList = await placeRepository.ListMasterAsync();
        return placeList;
    }

    /// <summary>
    /// Get place by Id.
    /// </summary>
    /// <param name="id">Place identifier.</param>
    /// <returns>A <see cref="Place"/> object if found; otherwise, null.</returns>
    public async Task<Place?> GetPlaceAsync(int id)
    {
        var place = await placeRepository.GetAsync(id);
        return place;
    }

    /// <summary>
    /// Saves a place to the database. If the place Id is 0, a new place is created; otherwise, the existing place is updated.
    /// </summary>
    /// <param name="place">The place to save.</param>
    /// <returns>The Id of the saved place.</returns>
    public async Task<int> SavePlaceAsync(Place place)
    {
        var placeId = await placeRepository.SaveItemAsync(place);
        return placeId;
    }

    /// <summary>
    /// Deletes a place from the database.
    /// </summary>
    /// <param name="place">The <see cref="Place"/> to delete.</param>
    /// <returns><see langword="true"> if deleted, otherwise <see langword="false"/>.</returns>
    public async Task<bool> DeletePlaceAsync(Place place)
    {
        if (place == null)
            return false;

        bool isConfirmed = true;

        var deletionMessage = new StringBuilder();
        deletionMessage.AppendLine("This place has the following links:");
        deletionMessage.AppendLine();

        // Check for place details
        if (place.PlaceDetails!.Any())
        {
            deletionMessage.AppendLine("* Place details");
            isConfirmed = false;
        }

        // Check for media links
        if (place.MediaLinks!.Any() && isConfirmed)
        {
            deletionMessage.AppendLine("* Media links");
            isConfirmed = false;
        }
        deletionMessage.AppendLine();
        deletionMessage.AppendLine("Do you still want to delete this place?");

        // Check if confirmation box needs to surface
        if (!isConfirmed)
            isConfirmed = await alertService.ShowAlertAsync("Delete place", deletionMessage.ToString(), "Yes", "No");

        // Delete items
        if (isConfirmed)
        {
            foreach (var mediaLink in place.MediaLinks!)
            {
                await mediaLinkRepository.DeleteItemAsync(mediaLink);
            }
            foreach (var placeDetail in place.PlaceDetails!)
            {
                await placeRepository.DeleteItemAsync(placeDetail);
            }
            await placeRepository.DeleteItemAsync(place!);
            return true;
        }
        return false;
    }
}