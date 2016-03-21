namespace Monitex

open Logibit
open Logibit.Hawk
open Logibit.Hawk.Types

open HttpClient

open System
open System.Text

open Newtonsoft.Json

type CreateOrderRequest() =

    let mutable amount : decimal = 0M
    let mutable currency : string = "btc"
    let mutable data : string = null
    let mutable priority : int = 0
    let mutable ttl : int = 15 // minutes
    let mutable urlCallBack : string = null
    let mutable urlSuccess : string = null
    let mutable urlFailure : string = null

    member x.Amount with get() = amount and set(v) = amount <- v
    member x.Currency with get() = currency and set(v) = currency <- v
    member x.Data with get() = data and set(v) = data <- v
    member x.Priority with get() = priority and set(v) = priority <- v
    member x.Ttl with get() = ttl and set(v) = ttl <- v
    member x.UrlCallback with get() = urlCallBack and set(v) = urlCallBack <- v
    member x.UrlSuccess with get() = urlSuccess and set(v) = urlSuccess <- v
    member x.UrlFailure with get() = urlFailure and set(v) = urlFailure <- v

type CreateOrderResponse() =
    let mutable id : string = null
    let mutable total    : decimal = 0.00M
    let mutable currency       : string = "btc"
    let mutable btcRequired    : decimal = 0.00M
    let mutable data  : string  = null
    let mutable createdOn    : DateTime = DateTime.UtcNow
    let mutable expiresOn    : DateTime = DateTime.UtcNow
    let mutable status    : string = "active"
    let mutable last_update    : DateTime = DateTime.UtcNow
    let mutable url    : string = null

    [<JsonProperty("Id")>]
    member x.Id with get() = id and set(v) = id <- v

    [<JsonProperty("Total")>]
    member x.Total with get() = total and set(v) = total <- v
  
    [<JsonProperty("Currency")>]
    member x.Currency with get() = currency and set(v) = currency <- v
  
    [<JsonProperty("BtcRequired")>]
    member x.BtcRequired with get() = btcRequired and set(v) = btcRequired <- v

    [<JsonProperty("Data")>]
    member x.Data with get() = data and set(v) = data <- v

    [<JsonProperty("CreatedOn")>]
    member x.CreatedOn with get() = createdOn and set(v) = createdOn <- v

    [<JsonProperty("ExpiresOn")>]
    member x.ExpiresOn with get() = expiresOn and set(v) = expiresOn <- v

    [<JsonProperty("Status")>]
    member x.Status with get() = status and set(v) = status <- v

    [<JsonProperty("LastUpdate")>]
    member x.LastUpdate with get() = last_update and set(v) = last_update <- v

    [<JsonProperty("Url")>]
    member x.Url with get() = url and set(v) = url <- v

type Client(serverUrl : string, id : string, key : string, ?data : string, ?ttl : int) =
    
    let endPointUrl = serverUrl + "/createOrder"

    let cred =
        { id        = id
          key       = key
          algorithm = SHA256 }

    let call mac data =

      let request = 
        createRequest Post endPointUrl
        |> withHeader (Custom {name = "Hawk"; value = mac })
        |> withBody data

      getResponseBody request

    member this.CreateOrder(amount: decimal,currency : string) : CreateOrderResponse =

      match Hawk.Client.header (new Uri(endPointUrl)) Hawk.Types.HttpMethod.POST (Hawk.Client.ClientOptions.mkSimple(cred)) with
      | Choice1Of2 hawk ->
        let newOrder = new CreateOrderRequest()
        newOrder.Amount <- amount
        newOrder.Currency <- currency
        let serializedObject = JsonConvert.SerializeObject(newOrder)
        let response = call hawk.mac serializedObject
        JsonConvert.DeserializeObject<CreateOrderResponse>(response)
      | Choice2Of2 _    -> failwith "Couldn't create Hawk header."
