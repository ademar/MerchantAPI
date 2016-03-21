# MerchantAPI

This is a .NET client for Monitex's Bitcoin Payments API

Sample client code in C#

```csharp
var client = new Monitex.Client("https://api.bitbuy.ca"
  // Do not use these keys, create a new pair of API keys in the control panel
  , "HAWK_ID"
  , "HAWK_KEY" );

client.Data = invoice.Id;
client.Ttl  = 30; // minutes

var order = client.CreateOrder(invoice.Amount,"CAD");

RedirectToUrl(order.url);
```