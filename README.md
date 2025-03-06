# Druware.Client

A library of the client objects and tools built for interacting with the Druware
Server API's that are avaialble in parts for building applications as building 
blocks.

## Dependencies

RESTfulFoundation

## History

### 2025/01/24 - v0.5.0

Initial release as nuget pacakge

## License

This project is under the GPLv3 license, see the included LICENSE.txt file

```
Copyright 2019-2025 by:
    Andy 'Dru' Satori @ Satori & Associates, Inc.
    All Rights Reserved
```

dotnet build . --configuration RELEASE 

```
nuget pack -OutputDirectory pub -Properties Configuration=Release
cd pub 
nuget push Druware.Client.0.6.0.nupkg -k "<copy API key here>" 
```
