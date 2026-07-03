using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Domain.Entities;

public sealed record GeneratedRefreshToken(
    string Token,
    DateTime ExpiresAt);