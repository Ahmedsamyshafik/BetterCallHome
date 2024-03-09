namespace Services.Abstracts
{
    public interface IImagesServices
    {
        Task<string> AddImage(string imgname, string imgPath, int apartmentID);
    }
}
