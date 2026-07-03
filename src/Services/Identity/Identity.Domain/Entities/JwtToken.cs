using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Domain.Entities;

public sealed record JwtToken(
    string AccessToken,
    DateTime ExpiresAt);