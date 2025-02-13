using Eventuous.Tests.Postgres.Fixtures;
using static Eventuous.Tests.Postgres.Store.Helpers;

namespace Eventuous.Tests.Postgres.Store;

public class Read {
    readonly IntegrationFixture _fixture = new();

    [Fact]
    public async Task ShouldReadOne() {
        var evt        = CreateEvent();
        var streamName = GetStreamName();
        await _fixture.EventStore.AppendEvent(streamName, evt, ExpectedStreamVersion.NoStream);

        var result = await _fixture.EventStore.ReadEvents(
            streamName,
            StreamReadPosition.Start,
            100,
            default
        );

        result.Length.Should().Be(1);
        result[0].Payload.Should().BeEquivalentTo(evt);
    }

    [Fact]
    public async Task ShouldReadMany() {
        // ReSharper disable once CoVariantArrayConversion
        object[] events = CreateEvents(20).ToArray();
        var streamName = GetStreamName();
        await _fixture.EventStore.AppendEvents(streamName, events, ExpectedStreamVersion.NoStream);
        
        var result = await _fixture.EventStore.ReadEvents(
            streamName,
            StreamReadPosition.Start,
            100,
            default
        );

        var actual = result.Select(x => x.Payload);
        actual.Should().BeEquivalentTo(events);
    }
    
    [Fact]
    public async Task ShouldReadTail() {
        // ReSharper disable once CoVariantArrayConversion
        object[] events = CreateEvents(20).ToArray();
        var streamName = GetStreamName();
        await _fixture.EventStore.AppendEvents(streamName, events, ExpectedStreamVersion.NoStream);
        
        var result = await _fixture.EventStore.ReadEvents(
            streamName,
            new StreamReadPosition(10),
            100,
            default
        );

        var expected = events.Skip(10);
        var actual   = result.Select(x => x.Payload);
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public async Task ShouldReadHead() {
        // ReSharper disable once CoVariantArrayConversion
        object[] events = CreateEvents(20).ToArray();
        var streamName = GetStreamName();
        await _fixture.EventStore.AppendEvents(streamName, events, ExpectedStreamVersion.NoStream);
        
        var result = await _fixture.EventStore.ReadEvents(
            streamName,
            StreamReadPosition.Start, 
            10,
            default
        );

        var expected = events.Take(10);
        var actual   = result.Select(x => x.Payload);
        actual.Should().BeEquivalentTo(expected);
    }
}
