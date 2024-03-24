namespace AutenticationBlazorWebApi.Models.Responses
{
    public record GeneralListResponse<T>(bool Flag, string Message = null!, IList<T> Items = null!);

}
