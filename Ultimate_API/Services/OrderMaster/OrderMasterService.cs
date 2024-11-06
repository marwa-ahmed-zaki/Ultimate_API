
//using Ultimate_API.RepositoryPattern.UnitOfWork;

//namespace Ultimate_API.Services.OrderMaster
//{
//    public class OrderMasterService : IOrderMasterService
//    {
//        private readonly IUnitOfWork _unitOfWork;

//        public OrderMasterService(IUnitOfWork unitOfWork)
//        {
//            _unitOfWork = unitOfWork;
//        }

//        public async Task<OrderMaster> GetOrderByIdAsync(int id)
//        {
//            return await _unitOfWork.Repository<OrderMaster, int>().GetByIdAsync(id);
//        }

//        public async Task<IEnumerable<OrderMaster>> GetAllOrdersAsync()
//        {
//            return await _unitOfWork.Repository<OrderMaster, int>().GetAllAsync();
//        }

//        public async Task CreateOrderAsync(OrderMaster order)
//        {
//            await _unitOfWork.Repository<OrderMaster, int>().AddAsync(order);
//            await _unitOfWork.CompleteAsync();
//        }

//        public async Task UpdateOrderAsync(OrderMaster order)
//        {
//            _unitOfWork.Repository<OrderMaster, int>().UpdateAsync(order);
//            await _unitOfWork.CompleteAsync();
//        }

//        public async Task DeleteOrderAsync(int id)
//        {
//            _unitOfWork.Repository<OrderMaster, int>().DeleteAsync(id);
//            await _unitOfWork.CompleteAsync();
//        }
//    }
//}
