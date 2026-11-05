using System.Runtime.CompilerServices;

// The composition root wires the module's internal services, as UserAccess's does; every handler here is internal so
// that nothing outside this module constructs one.
[assembly: InternalsVisibleTo("Sergin.Platform")]
