namespace Services.Abstracts
{
    public interface IImagesServices
    {
        Task<string> AddImage(string imgUrl, int apartmentID);
    }
}
