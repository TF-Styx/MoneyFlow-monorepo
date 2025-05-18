using MoneyFlow.Application.DTOs;
using MoneyFlow.Application.Mappers;
using MoneyFlow.Application.UseCaseInterfaces.CategoryCaseInterfaces;
using MoneyFlow.Domain.Interfaces.Repositories;

namespace MoneyFlow.Application.UseCases.CategoryCases
{
    public class CreateCategoryUseCase : ICreateCategoryUseCase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CreateCategoryUseCase(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<(CategoryDTO CategoryDTO, string Message)> CreateAsync(string? categoryName, string? description, string? color, byte[]? image, int idUser)
        {
            var message = string.Empty;

            if (string.IsNullOrWhiteSpace(categoryName)) { return (null, "Вы не заполнили поря!!"); }

            var id = await _categoryRepository.CreateAsync(categoryName, description, color, image, idUser);
            var domain = await _categoryRepository.GetAsync(id);

            return (domain.ToDTO().CategoryDTO, message);
        }
        public (CategoryDTO CategoryDTO, string Message) Create(string? categoryName, string? description, string? color, byte[]? image, int idUser)
        {
            var message = string.Empty;

            if (string.IsNullOrWhiteSpace(categoryName)) { return (null, "Вы не заполнили поря!!"); }

            //var exist = _categoryRepository.Get(categoryName);

            //if (exist != null) { return (null, "Категория с таким именем уже есть!!"); }

            var id = _categoryRepository.Create(categoryName, description, color, image, idUser);
            var domain = _categoryRepository.Get(id);

            return (domain.ToDTO().CategoryDTO, message);
        }
    }
}
