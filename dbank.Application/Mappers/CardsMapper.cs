using DBank.Application.Models.Cards;
using DBank.Domain.Entities;

namespace DBank.Application.Mappers;

public static class CardsMapper
{
    public static CardDto ToDto(this CardEntity entity)
    {
        return new CardDto
        {
            CustomerId = entity.CustomerId,
            Card = entity.Card,
        };
    }

    public static CardEntity ToEntity(this CardDto dto)
    {
        return new CardEntity
        {
            Card = dto.Card!
        };
    }
}
