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
 * the Trustwin.Client API Library. If not, see <https://www.gnu.org/licenses/>.
 *
 * Copyright 2019-2023 by:
 *    Satori & Associates, Inc.
 *    All Rights Reserved
 ******************************************************************************/

/* History
 *   Modified By  How
 *   -------- --- --------------------------------------------------------------
 *   23/10/25 ars Added documentation and license header, cleaned up all code to
 *                remove extraneous code and verbosity
 ******************************************************************************/

using System.Text.Json.Serialization;
using RESTfulFoundation;

namespace Druware.Client;


/// <summary>
/// the Login entity handles all of the work of establishing a credentialed user
/// session. Very few elements of the system can be used without consuming this
/// and thus, it is also the performance gateway and critical gateway through
/// the system.  
/// </summary>
public class Mfa : RESTObject
{
    /// <summary>
    /// the path to this endpoint
    /// </summary>
    public const string Path = "/api/login/mfa";


    /// <summary>
    /// the UserName that the session will be associated with, typically the
    /// initial email address of the account.
    /// </summary>
    [JsonPropertyName("requiresTwoFactor")]
    public bool RequiresTwoFactor { get; set; } = false;
    
    /// <summary>
    /// the UserName that the session will be associated with, typically the
    /// initial email address of the account.
    /// </summary>
    [JsonPropertyName("userName")]
    public string? UserName { get; set; }
        
    /// <summary>
    /// the Password required to login.  Both userName and password are
    /// required to login.
    /// </summary>
    [JsonPropertyName("mfaToken")] 
    public string? MfaToken { get; set; }
}

/// <summary>
/// the Login entity handles all of the work of establishing a credentialed user
/// session. Very few elements of the system can be used without consuming this
/// and thus, it is also the performance gateway and critical gateway through
/// the system.  
/// </summary>
public class Login : RESTObject
{
    /// <summary>
    /// the path to this endpoint
    /// </summary>
    public const string Path = "/api/login/";

    /// <summary>
    /// the UserName that the session will be associated with, typically the
    /// initial email address of the account.
    /// </summary>
    [JsonPropertyName("userName")]
    public string? UserName { get; set; }
        
    /// <summary>
    /// the Password required to login.  Both userName and password are
    /// required to login.
    /// </summary>
    [JsonPropertyName("password")] 
    public string? Password { get; set; }
}

public static class LoginExtension
{
    public static async Task<User?> LoginAsync(
        this Login login,
        RESTConnection connection,
        Action<User?>? completion = null,
        Action<string?>? failure = null)
    {
        Result<User?>? r = await connection.PostAsync(Client.Login.Path, login, (Result<User?>? r) =>
            {
                if (r == null) return;
                if (!r.Succeeded)
                {
                    failure?.Invoke(string.Join("\n", r.Info ?? []));
                    return;
                }
                
                if (r.Entity != null)
                {
                    Console.WriteLine(r?.Entity);
                    var user = r?.Entity as User ?? null;
                    completion?.Invoke(user);
                    return;
                }

                if (r.Info?.Contains("MFA Required") ?? false)
                {
                    Console.WriteLine(r.Info);
                    var user = new User();
                    user.TwoFactorEnabled = true;
                    completion?.Invoke(user);
                }
            },
            (message) =>
            {
                failure?.Invoke(message);
            });
        
        if (r is not { Succeeded: true }) return null;
                
        if (r.Entity != null)
        {
            var user = r?.Entity as User ?? null;
            return user;
        }

        if (r.Info?.Contains("MFA Required") ?? false)
        {
            var user = new User();
            user.TwoFactorEnabled = true;
            return user;
        }
        return null; // user;
    }
    
    public static User? Login(
        this Login login,
        RESTConnection connection)
    {
        var user = connection.Post<User?, Login>(Client.Login.Path, login);
        return user;
    }
    
    public static async Task<User?> MfaAsync(
        this Login login,
        RESTConnection connection, 
        string mfaToken,
        Action<User?>? completion = null,
        Action<string?>? failure = null)
    {
        var mfa = new Mfa
        {
            UserName = login.UserName,
            MfaToken = mfaToken
        };

        var result = await connection.PostAsync(Client.Mfa.Path, mfa, (Result<User?>? r) =>
            {
                var user = r?.Entity as User ?? null;
                completion?.Invoke(user);
            },
            (message) =>
            {
                failure?.Invoke(message);
            });
        return result?.Entity as User ?? null;
    }
    
    public static User? Mfa(
        this Login login,
        string mfaToken,
        RESTConnection connection)
    {
        var mfa = new Mfa
        {
            UserName = login.UserName,
            MfaToken = mfaToken
        };
        var result = connection.Post<Result<User?>?, Mfa?>(Client.Mfa.Path, mfa);
        return (User?)result?.Entity ?? null;
    }
}