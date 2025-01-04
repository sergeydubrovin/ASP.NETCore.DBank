namespace dbank.Application.Abstractions
{
    public interface IPayService
    {
        Task Create();
        Task GetById();
        Task GetByUser();  
    }
}
