/* This file is part of the Druware.Client API Library
 *
 * The Druware.Client API Library is free software: you can redistribute it
 * and/or modify it under the terms of the GNU General Public License as
 * published by the Free Software Foundation, either version 3 of the License,
 * or (at your option) any later version.
 *
 * The Druware.Client API Library is distributed in the hope that it will be
 * useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General
 * Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with
 * the Druware.Client API Library. If not, see <https://www.gnu.org/licenses/>.
 *
 * Copyright 2019-2023 by:
 *    Satori & Associates, Inc.
 *    All Rights Reserved
 ******************************************************************************/

/* History
 *   Modified By  How
 *   -------- --- --------------------------------------------------------------
 *   23/10/27 ars Added documentation and license header, cleaned up all code to
 *                remove extraneous code and verbosity
 ******************************************************************************/

using System.Text.Json.Serialization;
using RESTfulFoundation;

namespace Druware.Client;

public partial class Registration : RESTObject
{
    /// <summary>
    /// the path to this endpoint
    /// </summary>
    private const string Path = "/api/register/";

    /// <summary>
    /// default constructor required to support the Json interfaces for
    /// deserialization
    /// </summary>
    public Registration()
    {
    }

    /// <summary>
    /// an auxiliary constructor that would allow for passing in the RESTConnection
    /// as desired. This functionality is currently unused. 
    /// </summary>
    /// <param name="restConnection"></param>
    public Registration(RESTConnection? restConnection = null)
    { }
    
    /// <summary>
    /// the First Name of the user ( friendly )
    /// </summary>
    [JsonPropertyName("firstName")] 
    public string? FirstName { get; set; }
    /// <summary>
    /// the Last Name/ Surname of the user
    /// </summary>
    [JsonPropertyName("lastName")] public string? LastName { get; set; }
    
    /// <summary>
    /// the email to be used for this account. it will be confirmed during the
    /// process, and it MUST be unique ( see CheckEmail() function ) below.
    /// </summary>
    [JsonPropertyName("email")] public string? Email { get; set; }
    
    /// <summary>
    /// the Password required to login.  Both userName and password are
    /// required to login.
    /// </summary>
    [JsonPropertyName("password")] 
    public string? Password { get; set; }

    /// <summary>
    /// the Password required to login.  Both userName and password are
    /// required to login.
    /// </summary>
    [JsonPropertyName("confirmPassword")] 
    public string? ConfirmPassword { get; set; }
}

/// <summary>
/// additional implementation methods for ease of access and usage.
/// </summary>
public partial class Registration
{
    public async Task<Result<RESTObject?>?> Request(
        RESTConnection connection,
        Action<Result<RESTObject?>?>? completion = null,
        Action<string?>? failure = null)
    {
        var result = await connection.PostAsync(Path, this,
            (Result<RESTObject?>? r) => { completion?.Invoke(r); },
            (message) => { failure?.Invoke(message); });
        return result;
    }

    public static async Task<Result<RESTObject?>?> Confirm(
        RESTConnection connection,
        string email,
        string token,
        Action<Result<RESTObject?>?>? completion = null,
        Action<string?>? failure = null)

    {
        var result = await connection.GetAsync(
            Path,
            null, 
            $"?email={email}&token={token}",
            (Result<RESTObject?>? r) => completion?.Invoke(r),
            (message) => failure?.Invoke(message)
        );
        return result;    
    }
}