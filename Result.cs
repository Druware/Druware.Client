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
 *   23/10/25 ars Added documentation and license header, cleaned up all code to
 *                remove extraneous code and verbosity
 ******************************************************************************/
using System.Text.Json.Serialization;
using RESTfulFoundation;

namespace Druware.Client;

/// <summary>
/// All Druware API object used to return a base of a Result of type.
/// This result is the base properties that exist when a result is returned to
/// client upon request
///
/// **NOTE**
/// This model will be retired and should no longer be used as a base.
/// 
/// </summary>
public class Result<T> : RESTObject  where T : RESTObject?
{
    /// <summary>
    /// an internally required constructor for use when creating an object as a
    /// model for POST operations, as well as a base to enable the underlying
    /// access methods to instantiate object instances from Json models.
    /// </summary>
    public Result()
    {
    }
    
    /// <summary>
    /// determines if the result is successful, or is in an error state
    /// </summary>
    [JsonPropertyName("succeeded")]
    public bool Succeeded { get; set; }

    /// <summary>
    /// informational parameter, used most often in an error condition&lt;
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("info")]
    public List<string>? Info { get; set; }
    
    /// <summary>
    /// informational parameter, used most often in an error condition&lt;
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("entity")]
    public T? Entity { get; set; }
}


