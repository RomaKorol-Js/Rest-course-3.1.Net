using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Domain.Cards;
using FluentAssertions;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Xunit;
using Tests.Data.Cards;


namespace Api.Tests.Integration.Cards;

//public class CardControllerTests : BaseIntegrationTest
//{
    public class CardControllerTests: BaseIntegrationTest, IAsyncLifetime
    {
        private readonly Card _firstTestCard;    
        private readonly Card _secondTestCard;    
        private readonly Card _thirdTestCard;
        private readonly Card _incorectTestCard;

        private readonly string _createCardRoute;
        private readonly string _updateCardRoute;
        private readonly string _updateCardWrongRoute;
        private readonly string _deleteCardRoute;
        private readonly string _deleteCardWrongRoute;

    public async Task InitializeAsync()
    {
        await Context.Cards.AddAsync(_secondTestCard);
        await Context.Cards.AddAsync(_thirdTestCard);
        await Context.SaveChangesAsync();
        Context.ChangeTracker.Clear();
    }
    public async Task DisposeAsync()
    {
        Context.Cards.RemoveRange(Context.Cards);
        await Context.SaveChangesAsync();
        Context.ChangeTracker.Clear();
    }

    public CardControllerTests (IntegrationTestWebFactory factory) : base(factory)
    {
        _firstTestCard = CardTestData.FirstTestsCard();
        _secondTestCard = CardTestData.SecondTestsCard();
        _thirdTestCard = CardTestData.ThirdTestsCard();
        _incorectTestCard = CardTestData.InvalidTestsCard();

        _createCardRoute = "cards";
        _updateCardRoute = $"cards/{_secondTestCard.Id}";
        _deleteCardRoute = $"cards/{_secondTestCard.Id}";
        _deleteCardWrongRoute = $"cards/{_firstTestCard.Id}";
        _updateCardWrongRoute = $"cards/{_firstTestCard.Id}";
    }
    [Fact]
    public async Task ShouldCreateCard()
    {
        var request = new CreateCardDto(Name: _firstTestCard.Name, Description: _firstTestCard.Description, Attack: _firstTestCard.Attack, Health: _firstTestCard.Health, Cost: _firstTestCard.Cost);

        var response = await Client.PostAsJsonAsync(_createCardRoute, request);

        response.IsSuccessStatusCode.Should().BeTrue();

        var ResponseDto = await response.Content.ReadFromJsonAsync<CardDto>();

        ResponseDto.Should().NotBeNull();

        var dbCard = await Context.Cards.FirstOrDefaultAsync(x => x.Id == new CardId(ResponseDto!.Id));
        
        dbCard.Should().NotBeNull();
        dbCard!.Name.Should().Be(request.Name);
        dbCard.Description.Should().Be(request.Description);
        dbCard.Attack.Should().Be(request.Attack);
        dbCard.Health.Should().Be(request.Health);
        dbCard.Cost.Should().Be(request.Cost);
    }
    [Fact]
    public async Task ShouldUpdateCard()
    {
        // Arrange
        var request = new CreateCardDto(Name: _secondTestCard.Name, Description: _thirdTestCard.Description, Attack: _thirdTestCard.Attack, Health: _thirdTestCard.Health, Cost: _thirdTestCard.Cost);
        // Act
        var response = await Client.PutAsJsonAsync(_updateCardRoute, request);
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var responseDto = await response.Content.ReadFromJsonAsync<CardDto>();

        responseDto.Should().NotBeNull();
        responseDto!.Id.Should().Be(_secondTestCard.Id.Value);
        var dbCard = await Context.Cards.FirstOrDefaultAsync(x => x.Id ==
        _secondTestCard.Id);
        dbCard.Should().NotBeNull();
        dbCard!.Name.Should().Be(_secondTestCard.Name);
        dbCard!.Description.Should().Be(_thirdTestCard.Description);
        dbCard.Attack.Should().Be(_thirdTestCard.Attack);
        dbCard.Health.Should().Be(_thirdTestCard.Health);
        dbCard.Cost.Should().Be(_thirdTestCard.Cost);
    }

    [Fact]
    public async Task ShouldNotCreateCardBecauseNameAlreadyTaken()
    {
        var request = new CreateCardDto( Name: _secondTestCard.Name, Description: _secondTestCard.Description, Attack: _secondTestCard.Attack, Health: _secondTestCard.Health, Cost: _secondTestCard.Cost);

        var response = await Client.PostAsJsonAsync(_createCardRoute, request);

        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var dbCard = await Context.Cards.SingleAsync(x => x.Name == _secondTestCard.Name);
        dbCard.Id.Should().Be(_secondTestCard.Id);
    }
    [Fact]
    public async Task ShouldNotUpdateCardBecouseNameIsAlreadyTaken()
    {
        var request = new UpdateCardDto(Name: _thirdTestCard.Name, Description: _secondTestCard.Description, Attack: _secondTestCard.Attack, Health: _secondTestCard.Health, Cost: _secondTestCard.Cost);

        var response = await Client.PutAsJsonAsync(_updateCardRoute, request);

        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var dbCard = await Context.Cards.SingleAsync( x => x.Name == _secondTestCard.Name);
        dbCard.Name.Should().Be(_secondTestCard.Name);
    }
    [Fact]
    public async Task ShouldNotUpdateCardBecauseCardNotFound()
    {
        var request = new UpdateCardDto(Name: _secondTestCard.Name, Description: _secondTestCard.Description, Attack: _secondTestCard.Attack, Health: _secondTestCard.Health, Cost: _secondTestCard.Cost);

        var response = await Client.PutAsJsonAsync(_updateCardWrongRoute, request);

        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var dbCard = await Context.Cards.SingleAsync(x => x.Name == _secondTestCard.Name);
        dbCard.Name.Should().Be(_secondTestCard.Name);
    }
    [Fact]
    public async Task ShouldDeleteCard()
    {
        var dbCard_init = await Context.Cards.SingleAsync(x => x.Id == _secondTestCard.Id);
        dbCard_init.Should().NotBeNull();
        var response = await Client.DeleteAsync(_deleteCardRoute);
        
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var dbCard_final = await Context.Cards.SingleOrDefaultAsync(x => x.Id == _secondTestCard.Id);
        dbCard_final.Should().Be(null);
    }
    [Fact]
    public async Task ShouldNotDeleteCardBecauseCardNotFound()
    {
        var dbCard_init = await Context.Cards.SingleOrDefaultAsync(x => x.Id == _firstTestCard.Id);
        dbCard_init.Should().Be(null);
        var response = await Client.DeleteAsync(_deleteCardWrongRoute);

        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var dbCard_final = await Context.Cards.SingleOrDefaultAsync(x => x.Id == _firstTestCard.Id);
        dbCard_final.Should().Be(null);
    }

    [Fact]
    public async Task ShouldNotCreateCardBecauseRequestIsInvalid()
    {
        var request = new CreateCardDto(Name: _incorectTestCard.Name, Description: _incorectTestCard.Description, Attack: _incorectTestCard.Attack, Health: _incorectTestCard.Health, Cost: _incorectTestCard.Cost);

        var response = await Client.PostAsJsonAsync(_createCardRoute, request);

        var ResponseDto = await response.Content.ReadFromJsonAsync<CardDto>();

        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var dbCard = await Context.Cards.FirstOrDefaultAsync(x => x.Id == new CardId(ResponseDto!.Id));
        dbCard.Should().Be(null);
    }
}