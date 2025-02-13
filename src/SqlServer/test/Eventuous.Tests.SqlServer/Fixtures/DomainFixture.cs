using Eventuous.Sut.App;
using NodaTime;
using static Eventuous.Tests.SqlServer.Fixtures.IntegrationFixture;

namespace Eventuous.Tests.SqlServer.Fixtures;

public static class DomainFixture {
    static DomainFixture() => TypeMap.RegisterKnownEventTypes();
        
    public static Commands.ImportBooking CreateImportBooking() {
        var from = Instance.Auto.Create<LocalDate>();

        return Instance.Auto.Build<Commands.ImportBooking>()
            .With(x => x.CheckIn, from)
            .With(x => x.CheckOut, from.PlusDays(2))
            .Create();
    }
}