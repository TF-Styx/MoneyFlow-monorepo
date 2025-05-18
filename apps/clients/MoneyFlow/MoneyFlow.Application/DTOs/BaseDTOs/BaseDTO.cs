namespace MoneyFlow.Application.DTOs.BaseDTOs
{
    public abstract class BaseDTO<T> where T : BaseDTO<T>
    {
        protected BaseDTO()
        {

        }

        public T SetProperty(Action<T> setAction)
        {
            setAction((T)this);
            return (T)this;
        }
    }
}
