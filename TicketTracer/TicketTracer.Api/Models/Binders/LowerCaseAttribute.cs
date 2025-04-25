using Microsoft.AspNetCore.Mvc;

namespace TicketTracer.Api.Models.Binders;

internal class LowerCaseAttribute() : ModelBinderAttribute(typeof(LowerCaseModelBinder));