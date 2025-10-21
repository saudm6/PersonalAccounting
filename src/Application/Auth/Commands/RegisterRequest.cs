using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace PersonalAccounting.Application.Auth;

public record RegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password
    );
