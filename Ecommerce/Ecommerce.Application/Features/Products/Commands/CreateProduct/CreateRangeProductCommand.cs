using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Ecommerce.Application.Wrappers;
using Ecommerce.Domain.Entities;
using Ecommerce.Application.Interfaces.Repositories.ProductCrqs;

namespace Ecommerce.Application.Features.Products.Commands.CreateProduct
{
    public class CreateRangeProductCommand : List<CreateProductCommand>, IRequest<Response<List<CreateRangeProductResponse>>>
    {

        public class CreateRangeProductCommandHandler : IRequestHandler<CreateRangeProductCommand, Response<List<CreateRangeProductResponse>>>
        {
            private readonly IMapper _mapper;
            private readonly IWriteProductRepositoryAsync _writeProductRepository;
            private readonly IReadProductRepositoryAsync _readProductRepository;
            public CreateRangeProductCommandHandler(
                IMapper mapper,
                IReadProductRepositoryAsync readProductRepository,
                IWriteProductRepositoryAsync productRepository
                )
            {
                _mapper = mapper;
                _writeProductRepository = productRepository;
                _readProductRepository = readProductRepository;
            }
            public async Task<Response<List<CreateRangeProductResponse>>> Handle(CreateRangeProductCommand request, CancellationToken cancellationToken)
            {
                var validator = new CreateProductCommandValidator(_readProductRepository);
                var productsResponse = new List<CreateRangeProductResponse>();

                foreach (var product in request)
                {
                    var validationResult = await ValidateProduct(validator, product);
                    if (validationResult != null)
                    {
                        productsResponse.Add(validationResult);
                    }
                }

                var validProducts = request.Where(product => productsResponse.All(productResponse => productResponse.Barcode != product.Barcode)).ToList();
                var products = _mapper.Map<List<Product>>(validProducts);
                await _writeProductRepository.AddRangeAsync(products);
                productsResponse.AddRange(validProducts.Select(product => new CreateRangeProductResponse
                {
                    Barcode = product.Barcode,
                    Name = product.Name,
                    Description = product.Description,
                    Rate = product.Rate,
                    Price = product.Price,
                    Message = "Product created successfully",
                    Success = true
                }));

                await Task.CompletedTask;
                return new Response<List<CreateRangeProductResponse>>(productsResponse);
            }

            private async Task<CreateRangeProductResponse> ValidateProduct(CreateProductCommandValidator validator, CreateProductCommand product)
            {
                var productValidationResults = await validator.ValidateAsync(product);
                if (!productValidationResults.IsValid)
                {
                    var validationResults = productValidationResults.Errors.Select(failure => new ValidationResult(failure.ErrorMessage, new List<string> { failure.PropertyName })).ToList();
                    return new CreateRangeProductResponse
                    {
                        Barcode = product.Barcode,
                        Name = product.Name,
                        Description = product.Description,
                        Rate = product.Rate,
                        Price = product.Price,
                        Message = string.Join(", ", validationResults.Select(vr => vr.ErrorMessage)),
                        Success = false
                    };
                }

                return null;
            }
        }
    }
}
